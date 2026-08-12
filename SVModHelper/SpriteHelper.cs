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
        private static Dictionary<ItemViewDescriptor, ItemViewDataSO> cachedItemDatas;
        private static Dictionary<(PilotName, PilotSkinName), PilotDataSO> cachedPilotDatas;

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

            LogLevel propogatedLog = GetPropogatedLogLevel(logLevel, true);
            if (LoadTexture(descriptor, out texture, propogatedLog))
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

            LogLevel propogatedLog = GetPropogatedLogLevel(logLevel, false);
            Texture2D texture = GetTexture(descriptor.texture, propogatedLog);
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

            LogLevel propogatedLog = GetPropogatedLogLevel(logLevel, true);
            if (LoadSprite(descriptor, out sprite, propogatedLog))
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
            LogLevel propogatedLog = GetPropogatedLogLevel(logLevel, false);

            Sprite sprite = GetSprite(descriptor.sprite, propogatedLog);
            if(sprite == null) //failed to load sprite
            {
                //GetSprite would've logged the fail [in LoadTexture], so no need to log it here
                return null;
            }
            //The "CardName" property isn't actually important for CardViewData, so just set it to 0 and don't worry about it.
            CardViewData cardViewData = new CardViewData(0, sprite, null);
            cardViewData._outlineSprite = GetSprite(descriptor.sprite, propogatedLog);
            return cardViewData;
        }

        /// <summary>
        /// Loads an PilotDataSO and caches it for later use. If the view data is already cached, the cached data will be overwritten.
        /// </summary>
        /// <param name="pilot">The pilot to construct the data for.</param>
        /// <param name="data">The newly constructed data SO.</param>
        /// <param name="skin">The skin to construct the data for.</param>
        /// <param name="vanillaData">The list of vanilla pilot datas to use as a base.</param>
        /// <param name="logLevel">What types of messages should be printed to the console?</param>
        /// <returns>Returns true if the data was loaded successfully, otherwise false.</returns>
        public static bool LoadPilotData(PilotName pilot, out PilotDataSO data, PilotSkinName skin = PilotSkinName.Standard, PilotDataDictSO vanillaData = null, LogLevel logLevel = LogLevel.Fail)
        {
            if (logLevel == LogLevel.InitialQueryOrFail || logLevel == LogLevel.AllQueriesOrFail)
                Melon<Core>.Logger.Msg($"Calling LoadPilotData for {pilot} ({skin} skin).");
            LogLevel propogatedLog = GetPropogatedLogLevel(logLevel, false);

            if (ModContentManager.moddedPilotDescriptors.TryGetValue((pilot, skin), out var descriptor))
            {
                //ModPilotDescriptors would already have PilotModifications applied to them
                data = descriptor.GetPilotDataSO(propogatedLog);
                cachedPilotDatas[(pilot, skin)] = data;
                return true;
            }
            else
            {
                //No modded pilot found. Check vanilla pilots.
                PilotDataSO vanillaPilot = vanillaData?.pilotDataList.Find(new Func<PilotDataSO, bool>(p => p.PilotName == pilot && p.SkinName == skin));
                if (vanillaPilot == null)
                {
                    if (logLevel != LogLevel.None)
                    {
                        Melon<Core>.Logger.Error($"Unable to find pilot {pilot}.");
                    }
                    data = null;
                    return false;
                }
                if (ModContentManager.activePilotMods.TryGetValue(pilot, out PilotModification mods))
                {
                    mods.ApplyTo(vanillaPilot);
                }

                cachedPilotDatas[(pilot, skin)] = vanillaPilot;
                data = vanillaPilot;
                return true;
            }
        }

        /// <summary>
        /// Gets a cached PilotDataSO for a given pilot. If the PilotDataSO wasn't cached, it is automatically created
        /// </summary>
        /// <param name="pilot">The pilot to get the data for.</param>
        /// <param name="skin">The skin to get the data for.</param>
        /// <param name="vanillaData">The list of vanilla pilot datas to use as a base for constructing the data.</param>
        /// <param name="logLevel">What types of messages should be printed to the console?</param>
        /// <returns>Returns the cached data.</returns>
        public static PilotDataSO GetPilotData(PilotName pilot, PilotSkinName skin = PilotSkinName.Standard, PilotDataDictSO vanillaData = null, LogLevel logLevel = LogLevel.MissOrFail)
        {
            if (logLevel == LogLevel.InitialQueryOrFail || logLevel == LogLevel.AllQueriesOrFail)
                Melon<Core>.Logger.Msg($"Calling GetPilotData for {pilot} ({skin} skin).");

            if (cachedPilotDatas.TryGetValue((pilot, skin), out PilotDataSO data) && data != null)
                return data;

            //Deciding not to log a miss at the pilot level - there will be misses at the sprite level anyway, and this would otherwise catch unmodified vanilla pilots as well
            //if (logLevel == LogLevel.MissOrFail)
            //    Melon<Core>.Logger.Warning($"Cache miss when getting data for pilot {pilot}");
            LogLevel propogatedLog = GetPropogatedLogLevel(logLevel, false);
            if(LoadPilotData(pilot, out data, skin, vanillaData, propogatedLog))
            {
                return data;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Loads an ItemViewData and caches it for later use. If the view data is already cached, the cached data will be overwritten.
        /// </summary>
        /// <param name="descriptor">Descriptor for constructing the data.</param>
        /// <param name="data">The newly constructed data SO.</param>
        /// <param name="logLevel">What types of messages should be printed to the console?</param>
        /// <returns>Returns true if the data was loaded successfully, otherwise false.</returns>
        public static bool LoadItemData(ItemViewDescriptor descriptor, out ItemViewDataSO data, LogLevel logLevel = LogLevel.Fail)
        {
            if (logLevel == LogLevel.InitialQueryOrFail || logLevel == LogLevel.AllQueriesOrFail)
                Melon<Core>.Logger.Msg($"Calling LoadItemData for {descriptor}.");

            if(descriptor == null)
            {
                //exit early without printing warnings
                data = null;
                return false;
            }

            data = ScriptableObject.CreateInstance<ItemViewDataSO>();
            descriptor.ApplyTo(data, logLevel);
            cachedItemDatas[descriptor] = data;
            return true;
        }

        public static ItemViewDataSO GetItemData(ItemViewDescriptor descriptor, LogLevel logLevel = LogLevel.MissOrFail)
        {
            if (logLevel == LogLevel.InitialQueryOrFail || logLevel == LogLevel.AllQueriesOrFail)
                Melon<Core>.Logger.Msg($"Calling GetItemData for {descriptor}.");

            if (descriptor == null)
            {
                //exit early without printing warnings
                return null;
            }

            if (cachedItemDatas.TryGetValue(descriptor, out var data) && data != null)
                return data;
            
            if(logLevel == LogLevel.MissOrFail)
                Melon<Core>.Logger.Warning($"Cache miss when getting item data {descriptor}");

            LogLevel propogatedLog = GetPropogatedLogLevel(logLevel);
            LoadItemData(descriptor, out data, propogatedLog);
            return data;
        }

        internal static LogLevel GetPropogatedLogLevel(LogLevel logLevel, bool surpressMiss = true)
        {
            switch(logLevel)
            {
                case LogLevel.MissOrFail:
                    return surpressMiss ? LogLevel.Fail : LogLevel.MissOrFail;
                case LogLevel.InitialQueryOrFail:
                    return LogLevel.Fail;
                default:
                    return logLevel;
            }
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
            cachedPilotDatas = new();
            cachedItemDatas = new();
            LoadSprite(new SpriteDescriptor(DEFAULT_SHADOW_SPRITE_ID), out _);
            LoadSprite(new SpriteDescriptor(DEFAULT_ENTITY_SPRITE_ID), out _);
            LoadSprite(new SpriteDescriptor(TRANSPARENT_SPRITE_ID), out _);
        }
    }
}
