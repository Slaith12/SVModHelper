using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;
using SVModHelper.ModContent;

namespace SVModHelper
{
    public static class SVModHelper
    {
        internal static List<AModCard> moddedCards;
        internal static Dictionary<Type, CardName> moddedCardDict;
        internal static Dictionary<CardName, CardViewData> moddedCardVDs;

        internal static List<AModArtifact> moddedArtifacts;
        internal static Dictionary<Type, ArtifactName> moddedArtifactDict;
        internal static Dictionary<ArtifactName, Sprite> moddedArtifactVDs;

        private static Dictionary<string, byte[]> contentData;

        internal const CardName MINCARDID = (CardName)15000;
        internal const ArtifactName MINARTIFACTID = (ArtifactName)15000;
        internal const ComponentName MINCOMPID = (ComponentName)15000;
        internal const ItemName MINITEMID = (ItemName)15000;
        internal const EnemyName MINENEMYID = (EnemyName)15000;
        internal const ItemPackName MINPACKID = (ItemPackName)15000;

        static SVModHelper()
        {
            moddedCards = new();
            moddedCardDict = new();
            moddedCardVDs = new();

            moddedArtifacts = new();
            moddedArtifactDict = new();
            moddedArtifactVDs = new();

            contentData = new();
        }

        public static void RegisterMod(MelonMod mod)
        {
            Melon<Core>.Logger.Msg("Registering mod " + mod.Info.Name);
            Assembly modAsm = mod.MelonAssembly.Assembly;

            foreach(string fileName in modAsm.GetManifestResourceNames())
            {
                byte[] arr;
                Melon<Core>.Logger.Msg("  Loading resource " + fileName);
                using (Stream stream = modAsm.GetManifestResourceStream(fileName))
                {
                    if (stream == null)
                        continue;

                    if (stream is MemoryStream memStream)
                    {
                        arr = memStream.ToArray();
                    }
                    else
                    {
                        using (memStream = new MemoryStream())
                        {
                            stream.CopyTo(memStream);
                            arr = memStream.ToArray();
                        }
                    }
                }
                contentData.Add(fileName, arr);
                Melon<Core>.Logger.Msg("  Resource Loaded");
            }

            Melon<Core>.Logger.Msg("All Resources Loaded.");

            foreach(Type modCardDef in modAsm.GetTypes().Where(type => type.IsSubclassOf(typeof(AModCard))))
            {
                Melon<Core>.Logger.Msg("Registering card " + modCardDef.Name);
                RegisterCard(Activator.CreateInstance(modCardDef) as AModCard);
            }
            foreach (Type modArtifactDef in modAsm.GetTypes().Where(type => type.IsSubclassOf(typeof(AModArtifact))))
            {
                Melon<Core>.Logger.Msg("Registering artifact " + modArtifactDef.Name);
                RegisterArtifact(Activator.CreateInstance(modArtifactDef) as AModArtifact);
            }
        }

        #region Cards
        public static CardName RegisterCard(AModCard modCardDef, string imageName = null)
        {
            Type cardType = modCardDef.GetType();
            if(moddedCardDict.ContainsKey(cardType))
            {
                throw new InvalidOperationException("Can not register the same card multiple times.");
            }

            CardName id = (CardName)(moddedCards.Count + MINCARDID);
            moddedCards.Add(modCardDef);
            moddedCardDict.Add(cardType, id);

            if(string.IsNullOrEmpty(imageName))
            {
                imageName = cardType.Assembly.GetName().Name + "." + cardType.Name + ".png";
            }
            else
            {
                imageName = cardType.Assembly.GetName().Name + "." + imageName;
            }
            SetCardTitle(id, modCardDef.DisplayName);
            SetCardDesc(id, modCardDef.Description);
            SetCardImage(id, imageName);

            return id;
        }

        public static string SetCardTitle(CardName cardName, string title)
        {
            string id = cardName.ToString() + "_CardTitle";
            return SetLocalizedString(id, title);
        }

        public static string SetCardDesc(CardName cardName, string desc)
        {
            string id = cardName.ToString() + "_CardDesc";
            return SetLocalizedString(id, desc);
        }

        public static void SetCardImage(CardName cardName, string imageName, float pixelsPerUnit = 100)
        {
            if (!contentData.TryGetValue(imageName, out byte[] imageData))
                return;

            Texture2D texture = new Texture2D(2, 2) { filterMode = FilterMode.Bilinear };
            texture.LoadImage(imageData);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            SetCardImage(cardName, new CardViewData(cardName, sprite, null));
        }

        public static void SetCardImage(CardName cardName, CardViewData cardViewData)
        {
            moddedCardVDs[cardName] = cardViewData;
        }

        public static CardName GetModCardName<T>() where T : AModCard
        {
            if(moddedCardDict.TryGetValue(typeof(T), out CardName id))
            {
                return id;
            }
            return (CardName)(-1);
        }

        public static AModCard GetModCardInstance(CardName cardName)
        {
            if (cardName < MINCARDID || cardName >= MINCARDID + moddedCards.Count)
                return null;
            return moddedCards[cardName - MINCARDID];
        }
        #endregion

        #region Artifacts
        public static ArtifactName RegisterArtifact(AModArtifact modArtifactDef, string imageName = null)
        {
            Type artifactType = modArtifactDef.GetType();
            if (moddedArtifactDict.ContainsKey(artifactType))
            {
                throw new InvalidOperationException("Can not register the same artifact multiple times.");
            }

            ArtifactName id = (ArtifactName)(moddedArtifacts.Count + MINCARDID);
            moddedArtifacts.Add(modArtifactDef);
            moddedArtifactDict.Add(artifactType, id);

            if (string.IsNullOrEmpty(imageName))
            {
                imageName = artifactType.Assembly.GetName().Name + "." + artifactType.Name + ".png";
            }
            else
            {
                imageName = artifactType.Assembly.GetName().Name + "." + imageName;
            }
            SetArtifactTitle(id, modArtifactDef.DisplayName);
            SetArtifactDesc(id, modArtifactDef.Description);
            SetArtifactImage(id, imageName);

            return id;
        }

        public static string SetArtifactTitle(ArtifactName artifactName, string title)
        {
            string id = artifactName.ToString() + "_ArtiTitle";
            return SetLocalizedString(id, title);
        }

        public static string SetArtifactDesc(ArtifactName artifactName, string desc)
        {
            string id = artifactName.ToString() + "_ArtiDesc";
            return SetLocalizedString(id, desc);
        }

        public static void SetArtifactImage(ArtifactName artifactName, string imageName, float pixelsPerUnit = 100)
        {
            if (!contentData.TryGetValue(imageName, out byte[] imageData))
                return;

            Texture2D texture = new Texture2D(2, 2) { filterMode = FilterMode.Bilinear };
            texture.LoadImage(imageData);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            SetArtifactImage(artifactName, sprite);
        }

        public static void SetArtifactImage(ArtifactName artifactName, Sprite artifactViewData)
        {
            moddedArtifactVDs[artifactName] = artifactViewData;
        }

        public static ArtifactName GetModArtifactName<T>() where T : AModArtifact
        {
            if (moddedArtifactDict.TryGetValue(typeof(T), out ArtifactName id))
            {
                return id;
            }
            return (ArtifactName)(-1);
        }

        public static AModArtifact GetModArtifactInstance(ArtifactName artifactName)
        {
            if (artifactName < MINARTIFACTID || artifactName >= MINARTIFACTID + moddedArtifacts.Count)
                return null;
            return moddedArtifacts[artifactName - MINARTIFACTID];
        }
        #endregion

        public static string SetLocalizedString(string stringID, string localizedString)
        {
            string oldString = NameFixer.extraLocalizedStrings.GetValueOrDefault(stringID);
            NameFixer.extraLocalizedStrings[stringID] = localizedString;
            return oldString;
        }
    }
}
