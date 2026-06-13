using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

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
}
