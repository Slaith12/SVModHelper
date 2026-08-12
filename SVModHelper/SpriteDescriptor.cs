using UnityEngine;

namespace SVModHelper
{
    public struct TextureDescriptor
    {
        public string textureID;
        public FilterMode filter;
        public TextureWrapMode wrapMode;

        public TextureDescriptor(string textureID, FilterMode filter = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp)
        {
            this.textureID = textureID;
            this.filter = filter;
            this.wrapMode = wrapMode;
        }

        public override string ToString()
        {
            return textureID;
        }
    }

    public struct SpriteDescriptor
    {
        public TextureDescriptor texture;
        public Rect? rect;
        public Vector2 pivot;
        public float pixelsPerUnit;
        
        public SpriteDescriptor() : this("")
        {

        }

        public SpriteDescriptor(string textureID, FilterMode textureFilter = FilterMode.Bilinear, TextureWrapMode textureWrap = TextureWrapMode.Clamp,
            Rect? spriteRect = null, Vector2? spritePivot = null, float pixelsPerUnit = 100)
            : this(new TextureDescriptor(textureID, textureFilter, textureWrap), spriteRect, spritePivot, pixelsPerUnit)
        {
        }

        public SpriteDescriptor(TextureDescriptor texture, Rect? rect = null, Vector2? pivot = null, float pixelsPerUnit = 100)
        {
            this.texture = texture;
            this.rect = rect;
            this.pivot = pivot ?? new Vector2(0.5f, 0.5f);
            this.pixelsPerUnit = pixelsPerUnit;
        }

        public bool IsEmpty()
        {
            return texture.textureID == "";
        }

        public override string ToString()
        {
            if (IsEmpty())
                return "[EmptySprite]";
            string str = texture.ToString();
            if (rect != null)
                str += $" [{rect.Value}]";
            return str;
        }
    }

    public struct CardViewDescriptor
    {
        public SpriteDescriptor sprite;
        public SpriteDescriptor outlineSprite;
        //public Material material;

        public CardViewDescriptor() : this(new SpriteDescriptor())
        {

        }

        public CardViewDescriptor(SpriteDescriptor sprite, SpriteDescriptor? outlineSprite = null)
        {
            this.sprite = sprite;
            this.outlineSprite = outlineSprite ?? new SpriteDescriptor();
        }

        public bool IsEmpty()
        {
            return sprite.IsEmpty();
        }

        public override string ToString()
        {
            string str = sprite.ToString();
            if (!outlineSprite.IsEmpty())
                str += $"; Outline: {outlineSprite}";
            return str;
        }
    }

    public class EntityViewDescriptor
    {
        public SpriteDescriptor sprite;
        public SpriteDescriptor shadow;
        public List<SpriteDescriptor> chargeSprites = new();
        public float floatiness = 1;
        public SpawnType spawnType = SpawnType.Normal;
        public SoundName spawnSound = SoundName.None;
        public SoundName deathSound = SoundName.None;
        public SoundName moveSound = SoundName.P_MoveWoosh;
        public SoundName landingSound = SoundName.P_LandingSmack;
        public Il2CppCollections.List<AnimationItem> animationItems = new();
        public float xOffset = 0;
        public float yOffset = 0;
        public float scale = 1;
        public bool hasNewPrompt = false;

        protected void ApplyTo(AEntityViewDataSO viewData, SpriteHelper.LogLevel logLevel = SpriteHelper.LogLevel.Fail)
        {
            Sprite defaultEntitySprite = SpriteHelper.GetSprite(new SpriteDescriptor(SpriteHelper.DEFAULT_ENTITY_SPRITE_ID));
            Sprite defaultShadowSprite = SpriteHelper.GetSprite(new SpriteDescriptor(SpriteHelper.DEFAULT_SHADOW_SPRITE_ID));

            viewData.Sprite = SpriteHelper.GetSprite(sprite, logLevel) ?? defaultEntitySprite;
            viewData.Shadow = SpriteHelper.GetSprite(shadow, logLevel) ?? defaultShadowSprite;
            viewData.ChargeSprites = new();
            foreach (SpriteDescriptor sprite in chargeSprites)
            {
                //# of entries in ChargeSprites should always be the same, so substitute in the default sprite for failed entries
                viewData.ChargeSprites.Add(SpriteHelper.GetSprite(sprite, logLevel) ?? viewData.Sprite);
            }
            viewData.Floatiness = floatiness;
            viewData.SpawnType = spawnType;
            viewData.SpawnSound = spawnSound;
            viewData.DeathSound = deathSound;
            viewData.MoveSound = moveSound;
            viewData.LandingSound = landingSound;
            viewData.AnimationItems = animationItems;
            viewData.xOffset = xOffset;
            viewData.yOffset = yOffset;
            viewData.scale = scale;
            viewData.HasNewPrompt = hasNewPrompt;

            //unused AEntityViewDataSO properties: ShieldMaskSprite, BossPanelSprite
        }

        public override string ToString()
        {
            string str = sprite.ToString();
            if (!shadow.IsEmpty())
                str += $"; Shadow: {shadow}";
            return str;
        }

        public EntityViewDescriptor(SpriteDescriptor sprite, SpriteDescriptor? shadow = null)
        {
            this.sprite = sprite;
            this.shadow = shadow ?? new SpriteDescriptor();
        }
    }

    public class ItemViewDescriptor : EntityViewDescriptor
    {
        public bool isRandomlyRotated = false;
        public List<SpriteDescriptor> alternateRandomIdleSprites = new();
        
        public void ApplyTo(ItemViewDataSO viewData, SpriteHelper.LogLevel logLevel = SpriteHelper.LogLevel.Fail)
        {
            base.ApplyTo(viewData, logLevel);
            viewData.IsRandomlyRotated = isRandomlyRotated;
            viewData.AlternateRandomIdleSprites = new();
            foreach(SpriteDescriptor sprite in alternateRandomIdleSprites)
            {
                //# of entries in this list doesn't matter, so discard any failed entries.
                Sprite spr = SpriteHelper.GetSprite(sprite, logLevel);
                if (spr != null)
                    viewData.AlternateRandomIdleSprites.Add(spr);
            }

            //unused ItemViewDataSO properties: ItemName
        }

        public ItemViewDescriptor(SpriteDescriptor sprite, SpriteDescriptor? shadow = null) : base(sprite, shadow)
        {

        }
    }
}
