using MelonLoader;
using SVModHelper.ModContent;
using UnityEngine;

namespace SVModHelper
{
    public static class SpriteHelper
    {
        public const string DEFAULT_SHADOW_SPRITE_ID = "SVModHelper.DefaultShadow.png";
        public const string DEFAULT_ENTITY_SPRITE_ID = "SVModHelper.DefaultEntity.png";
        public const string TRANSPARENT_SPRITE_ID = "SVModHelper.TransparentSprite.png";

        public enum LogLevel
        {
            /// <summary>
            /// Don't print anything to the logs
            /// </summary>
            None,
            /// <summary>
            /// Print an error if a texture fails to load
            /// </summary>
            Fail,
            /// <summary>
            /// Print an error if a texture fails to load, and print a warning if a cache miss occurs
            /// </summary>
            MissOrFail,
            /// <summary>
            /// Print a message whenever a SpriteHelper function is called. Becomes LogLevel.Fail for internal calls.
            /// </summary>
            InitialQueryOrFail,
            /// <summary>
            /// Print a message whenever a SpriteHelper function is called, including all internal calls.
            /// </summary>
            AllQueriesOrFail
        }

		private static Sprite _transparentSprite;

        private static Dictionary<TextureDescriptor, Texture2D> cachedTextures;
        private static Dictionary<SpriteDescriptor, Sprite> cachedSprites;

	    public static Sprite GetTransparentSprite()
	    {
			if (_transparentSprite != null)
				return _transparentSprite;

			// Create a 1x1 transparent texture
			Texture2D transparentTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
			transparentTexture.SetPixel(0, 0, new Color(0, 0, 0, 0)); // Fully transparent
			transparentTexture.Apply();

			// Create sprite from the transparent texture
			_transparentSprite = Sprite.Create(transparentTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 100);

			return _transparentSprite;
	    }

        /// <summary>
        /// Loads a texture and caches it for later use. If the texture is already cached, the cached texture will be overwritten.
        /// </summary>
        /// <param name="descriptor">Descriptor for constructing the texture.</param>
        /// <param name="texture">The newly constructed texture.</param>
        /// <param name="logLevel">What types of messages should be printed to the console?</param>
        /// <returns>Returns true if the texture was loaded successfully, otherwise false.</returns>
        public static bool LoadTexture(TextureDescriptor descriptor, out Texture2D texture, LogLevel logLevel = LogLevel.Fail)
        {
            if (logLevel == LogLevel.InitialQueryOrFail || logLevel == LogLevel.AllQueriesOrFail)
                Melon<Core>.Logger.Msg($"Calling LoadTexture for {descriptor}.");

            //cheating a bit for the transparent sprite
            //i'm too lazy to include an actual transparent sprite in the assets
            if (descriptor.textureID == TRANSPARENT_SPRITE_ID)
            {
                texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                texture.SetPixel(0, 0, new Color(0, 0, 0, 0));
                texture.Apply();
            }
            else
            {
                bool success = ModContentManager.contentData.TryGetValue(descriptor.textureID, out byte[] data);
                if (!success)
                {
                    texture = null;
                    if (logLevel != LogLevel.None)
                        Melon<Core>.Logger.Error($"Failed to load file - {descriptor.textureID}. Make sure the file exists, it has been added as an Embedded Resource, and that the path is specified relative to the .csproj file.");
                    return false;
                }
                texture = new Texture2D(2, 2) { filterMode = descriptor.filter, wrapMode = descriptor.wrapMode };
                texture.LoadImage(data);
            }
            cachedTextures[descriptor] = texture;
            return true;
        }

        /// <summary>
        /// Gets a cached texture based on a descriptor. If the texture wasn't cached, it is automatically created.
        /// </summary>
        /// <param name="descriptor">Descriptor for finding/constructing the texture.</param>
        /// <param name="logLevel">What types of messages should be printed to the console?</param>
        /// <returns>Returns the cached texture.</returns>
        public static Texture2D GetTexture(TextureDescriptor descriptor, LogLevel logLevel = LogLevel.MissOrFail)
        {
            if (logLevel == LogLevel.InitialQueryOrFail || logLevel == LogLevel.AllQueriesOrFail)
                Melon<Core>.Logger.Msg($"Calling GetTexture for {descriptor}.");
            if (cachedTextures.TryGetValue(descriptor, out Texture2D texture) && texture != null)
                return texture;

            if (logLevel == LogLevel.InitialQueryOrFail)
                logLevel = LogLevel.Fail;
            if (LoadTexture(descriptor, out texture, logLevel))
            {
                if (logLevel == LogLevel.MissOrFail)
                {
                    Melon<Core>.Logger.Warning($"Cache miss when getting texture {descriptor}.");
                }
                return texture;
            }
            else
            {
                //LoadTexture would've logged the fail, so no need to log it here
                return null;
            }
        }

        /// <summary>
        /// Loads a sprite and caches it for later use. If the sprite is already cached, the cached sprite will be overwritten.
        /// </summary>
        /// <param name="descriptor">Descriptor for constructing the sprite.</param>
        /// <param name="sprite">The newly constructed sprite.</param>
        /// <param name="logLevel">What types of messages should be printed to the console?</param>
        /// <returns>Returns true if the sprite was loaded successfully, otherwise false.</returns>
        public static bool LoadSprite(SpriteDescriptor descriptor, out Sprite sprite, LogLevel logLevel = LogLevel.Fail)
        {
            if (logLevel == LogLevel.InitialQueryOrFail || logLevel == LogLevel.AllQueriesOrFail)
                Melon<Core>.Logger.Msg($"Calling LoadSprite for {descriptor}.");

            if (descriptor.IsEmpty())
            {
                //exit early without printing warnings
                sprite = null;
                return false;
            }

            if (logLevel == LogLevel.InitialQueryOrFail)
                logLevel = LogLevel.Fail;
            Texture2D texture = GetTexture(descriptor.texture, logLevel);
            if (texture == null)
            {
                //GetTexture would've logged the fail [in LoadTexture], so no need to log it here
                sprite = null;
                return false;
            }
            Rect rect = descriptor.rect ?? new Rect(0, 0, texture.width, texture.height);
            sprite = Sprite.Create(texture, rect, descriptor.pivot, descriptor.pixelsPerUnit);
            cachedSprites[descriptor] = sprite;
            return true;
        }

        /// <summary>
        /// Gets a cached sprite based on a descriptor. If the sprite wasn't cached, it is automatically created.
        /// </summary>
        /// <param name="descriptor">Descriptor for finding/constructing the sprite.</param>
        /// <param name="logLevel">What types of messages should be printed to the console?</param>
        /// <returns>Returns the cached sprite.</returns>
        public static Sprite GetSprite(SpriteDescriptor descriptor, LogLevel logLevel = LogLevel.MissOrFail)
        {
            if (logLevel == LogLevel.InitialQueryOrFail || logLevel == LogLevel.AllQueriesOrFail)
                Melon<Core>.Logger.Msg($"Calling GetSprite for {descriptor}.");

            if (descriptor.IsEmpty())
            {
                //exit early without printing warnings
                return null;
            }
            if (cachedSprites.TryGetValue(descriptor, out Sprite sprite) && sprite != null)
                return sprite;

            if (logLevel == LogLevel.InitialQueryOrFail)
                logLevel = LogLevel.Fail;
            if (LoadSprite(descriptor, out sprite, logLevel))
            {
                if (logLevel == LogLevel.MissOrFail)
                {
                    Melon<Core>.Logger.Warning($"Cache miss when getting sprite {descriptor}.");
                }
                return sprite;
            }
            else
            {
                //LoadSprite would've logged the fail [in LoadTexture], so no need to log it here
                return null;
            }
        }

        /// <summary>
        /// Creates a CardViewData object from cached sprites based on a descriptor. If the sprites weren't cached, it is automatically created.
        /// </summary>
        /// <param name="descriptor">Descriptor for constructing the card view.</param>
        /// <param name="logLevel">What types of messages should be printed to the console?</param>
        /// <returns>Returns the card view.</returns>
        public static CardViewData GetCardViewData(CardViewDescriptor descriptor, LogLevel logLevel = LogLevel.MissOrFail)
        {
            //CardViewData objects are cheap to construct if the sprites already exist, so they aren't cached directly.

            if (logLevel == LogLevel.InitialQueryOrFail || logLevel == LogLevel.AllQueriesOrFail)
                Melon<Core>.Logger.Msg($"Calling GetCardViewData for {descriptor}.");

            if (descriptor.IsEmpty())
            {
                //exit early without printing warnings
                return null;
            }
            if (logLevel == LogLevel.InitialQueryOrFail)
                logLevel = LogLevel.Fail;

            Sprite sprite = GetSprite(descriptor.sprite, logLevel);
            if(sprite == null) //failed to load sprite
            {
                //GetSprite would've logged the fail [in LoadTexture], so no need to log it here
                return null;
            }
            //The "CardName" property isn't actually important for CardViewData, so just set it to 0 and don't worry about it.
            CardViewData cardViewData = new CardViewData(0, sprite, null);
            cardViewData._outlineSprite = GetSprite(descriptor.sprite, logLevel);
            return cardViewData;
        }
        
		internal static void InitDefaultSprites()
        {
            var assembly = typeof(AModContent).Assembly;
            //I was originally planning on automatically grabbing the shadow sprite from the game directly,
            //but I'm not sure how to do that so I'm just adding the shadow sprite to the build instead.
            byte[] arr = ResourceHelper.LoadResource(assembly, "SVModHelper.shadow.png");
            if (arr == null)
            {
                Melon<Core>.Logger.Error("Unable to load default shadow image.");
            }
            else
            {
                ModContentManager.contentData.Add(DEFAULT_SHADOW_SPRITE_ID, arr);
            }

            arr = ResourceHelper.LoadResource(assembly, "SVModHelper.EntityUnknown.png");
            if (arr == null)
            {
                Melon<Core>.Logger.Error("Unable to load default entity image.");
            }
            else
            {
                ModContentManager.contentData.Add(DEFAULT_ENTITY_SPRITE_ID, arr);
            }
        }

        internal static void ResetSpriteCaches()
        {
            cachedTextures = new();
            cachedSprites = new();
            LoadSprite(new SpriteDescriptor(DEFAULT_SHADOW_SPRITE_ID), out _);
            LoadSprite(new SpriteDescriptor(DEFAULT_ENTITY_SPRITE_ID), out _);
            LoadSprite(new SpriteDescriptor(TRANSPARENT_SPRITE_ID), out _);
        }
    }
}
