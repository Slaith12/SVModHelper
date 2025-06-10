using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityEngine;

namespace SVModHelper
{
    public static class SVModHelper
    {
        internal static List<ModdedCardDefinition> moddedCards;
        internal static Dictionary<Type, CardName> moddedCardDict;
        internal static Dictionary<CardName, CardViewData> moddedCardVDs;

        private static Dictionary<string, byte[]> contentData;

        const int MINCARDID = 15000;
        const int MINRELICID = 15000;
        const int MINCOMPID = 15000;
        const int MINITEMID = 15000;
        const int MINENEMYID = 15000;

        static SVModHelper()
        {
            moddedCards = new();
            moddedCardDict = new();
            moddedCardVDs = new();
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

            foreach(Type modCardDef in modAsm.GetTypes().Where(type => type.IsSubclassOf(typeof(ModdedCardDefinition))))
            {
                Melon<Core>.Logger.Msg("Registering card " + modCardDef.Name);
                RegisterCard(Activator.CreateInstance(modCardDef) as ModdedCardDefinition);
            }
        }


        public static CardName RegisterCard(ModdedCardDefinition modCardDef, string imageName = null)
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

        public static string SetLocalizedString(string stringID, string localizedString)
        {
            string oldString = NameFixer.extraLocalizedStrings.GetValueOrDefault(stringID);
            NameFixer.extraLocalizedStrings[stringID] = localizedString;
            return oldString;
        }
    }
}
