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

        internal static List<AModComponent> moddedComponents;
        internal static Dictionary<Type, ComponentName> moddedComponentDict;
        internal static Dictionary<ComponentName, Sprite> moddedComponentVDs;

        internal static Dictionary<Type, string> moddedTaskIDs;
        internal static Dictionary<string, AModTask> moddedTaskInstances;

        private static Dictionary<string, byte[]> contentData;

        internal const CardName MINCARDID = (CardName)15000;
        internal const ArtifactName MINARTIFACTID = (ArtifactName)15000;
        internal const ComponentName MINCOMPID = (ComponentName)15000;
        internal const ItemName MINITEMID = (ItemName)15000;
        internal const EnemyName MINENEMYID = (EnemyName)15000;
        internal const ItemPackName MINPACKID = (ItemPackName)15000;

        public const CardName INVALIDCARDID = (CardName)(-1);
        public const ArtifactName INVALIDARTIFACTID = (ArtifactName)(-1);
        public const ComponentName INVALIDCOMPID = (ComponentName)(-1);
        public const ItemName INVALIDITEMID = (ItemName)(-1);
        public const EnemyName INVALIDENEMYID = (EnemyName)(-1);
        public const ItemPackName INVALIDPACKID = (ItemPackName)(-1);
        public const string INVALIDTASKID = "";

        static SVModHelper()
        {
            moddedCards = new();
            moddedCardDict = new();
            moddedCardVDs = new();

            moddedArtifacts = new();
            moddedArtifactDict = new();
            moddedArtifactVDs = new();

            moddedComponents = new();
            moddedComponentDict = new();
            moddedComponentVDs = new();

            moddedTaskIDs = new();
            moddedTaskInstances = new();

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
            foreach (Type modComponentDef in modAsm.GetTypes().Where(type => type.IsSubclassOf(typeof(AModComponent))))
            {
                Melon<Core>.Logger.Msg("Registering component " + modComponentDef.Name);
                RegisterComponent(Activator.CreateInstance(modComponentDef) as AModComponent);
            }
            foreach (Type modTaskDef in modAsm.GetTypes().Where(type => type.IsSubclassOf(typeof(AModTask))))
            {
                if(modTaskDef.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.EmptyTypes) == null)
                {
                    Melon<Core>.Logger.Warning($"Unable to register {modTaskDef.Name} due to lack of paramaterless constructor. Task must be registered manually.");
                    continue;
                }
                Melon<Core>.Logger.Msg("Registering task " + modTaskDef.Name);
                RegisterTask(Activator.CreateInstance(modTaskDef, true) as AModTask);
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

            CardName id = moddedCards.Count + MINCARDID;
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
            return INVALIDCARDID;
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

            ArtifactName id = moddedArtifacts.Count + MINARTIFACTID;
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
            return INVALIDARTIFACTID;
        }

        public static AModArtifact GetModArtifactInstance(ArtifactName artifactName)
        {
            if (artifactName < MINARTIFACTID || artifactName >= MINARTIFACTID + moddedArtifacts.Count)
                return null;
            return moddedArtifacts[artifactName - MINARTIFACTID];
        }
        #endregion

        #region Components
        public static ComponentName RegisterComponent(AModComponent modComponentDef, string imageName = null)
        {
            Type componentType = modComponentDef.GetType();
            if (moddedComponentDict.ContainsKey(componentType))
            {
                throw new InvalidOperationException("Can not register the same component multiple times.");
            }

            ComponentName id = moddedComponents.Count + MINCOMPID;
            moddedComponents.Add(modComponentDef);
            moddedComponentDict.Add(componentType, id);

            if (string.IsNullOrEmpty(imageName))
            {
                imageName = componentType.Assembly.GetName().Name + "." + componentType.Name + ".png";
            }
            else
            {
                imageName = componentType.Assembly.GetName().Name + "." + imageName;
            }
            SetComponentTitle(id, modComponentDef.DisplayName);
            SetComponentDesc(id, modComponentDef.Description);
            SetComponentImage(id, imageName);

            return id;
        }

        public static string SetComponentTitle(ComponentName componentName, string title)
        {
            string id = componentName.ToString() + "_CompTitle";
            return SetLocalizedString(id, title);
        }

        public static string SetComponentDesc(ComponentName componentName, string desc)
        {
            string id = componentName.ToString() + "_CompDesc";
            return SetLocalizedString(id, desc);
        }

        public static void SetComponentImage(ComponentName componentName, string imageName, float pixelsPerUnit = 100)
        {
            if (!contentData.TryGetValue(imageName, out byte[] imageData))
                return;

            Texture2D texture = new Texture2D(2, 2) { filterMode = FilterMode.Bilinear };
            texture.LoadImage(imageData);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
            SetComponentImage(componentName, sprite);
        }

        public static void SetComponentImage(ComponentName componentName, Sprite componentViewData)
        {
            moddedComponentVDs[componentName] = componentViewData;
        }

        public static ComponentName GetModComponentName<T>() where T : AModComponent
        {
            if (moddedComponentDict.TryGetValue(typeof(T), out ComponentName id))
            {
                return id;
            }
            return INVALIDCOMPID;
        }

        public static AModComponent GetModComponentInstance(ComponentName componentName)
        {
            if (componentName < MINCOMPID || componentName >= MINCOMPID + moddedComponents.Count)
                return null;
            return moddedComponents[componentName - MINCOMPID];
        }
        #endregion

        #region Tasks

        public static string RegisterTask(AModTask task)
        {
            Type taskType = task.GetType();
            if(moddedTaskIDs.ContainsKey(taskType))
            {
                throw new InvalidOperationException("Can not register the same task multiple times.");
            }

            string id = taskType.FullName;
            moddedTaskIDs.Add(taskType, id);
            moddedTaskInstances.Add(id, task);
            return id;
        }

        public static string GetModTaskID(Type taskType)
        {
            return moddedTaskIDs.GetValueOrDefault(taskType, INVALIDTASKID);
        }

        public static string GetModTaskID<T>() where T : AModTask
        {
            return GetModTaskID(typeof(T));
        }

        public static AModTask GetModTaskInstance(string id)
        {
            return moddedTaskInstances.GetValueOrDefault(id, null);
        }

        #endregion

        public static string SetLocalizedString(string stringID, string localizedString)
        {
            string oldString = NameFixer.extraLocalizedStrings.GetValueOrDefault(stringID);
            NameFixer.extraLocalizedStrings[stringID] = localizedString;
            return oldString;
        }

        public static EncounterValue GetAppropriateCostType(CardModel cardModel = null)
        {
            if (SceneLoader.IsSceneLoaded(SceneName.GlossarySceneAdditive) || SceneLoader.IsSceneLoaded(SceneName.HistoryScene))
            {
                switch (cardModel?.Class)
                {
                    case ClassName.Gunner:
                        return EncounterValue.Heat;
                    case ClassName.Melee:
                        return EncounterValue.Power;
                    case ClassName.Mystic:
                        return EncounterValue.Mana;
                    default:
                        return (EncounterValue)(-1);
                }
            }
            else if (SceneLoader.IsSceneLoaded(SceneName.DailyRunSceneAdditive))
            {
                switch (DailyRunController.ClassName)
                {
                    case ClassName.Gunner:
                        return EncounterValue.Heat;
                    case ClassName.Melee:
                        return EncounterValue.Power;
                    case ClassName.Mystic:
                        return EncounterValue.Mana;
                    default:
                        return (EncounterValue)(-1);
                }
            }
            else if (DataManager.PlayerHasRunInProgress())
            {
                return DataManager.PlayerData.ClassBaseEnergy;
            }
            else if (SceneLoader.IsSceneLoaded(SceneName.ChallengesSceneAdditive) && DataManager.PlayerData != null)
            {
                return DataManager.PlayerData.ClassBaseEnergy;
            }
            else
            {
                switch (cardModel?.Class)
                {
                    case ClassName.Gunner:
                        return EncounterValue.Heat;
                    case ClassName.Melee:
                        return EncounterValue.Power;
                    case ClassName.Mystic:
                        return EncounterValue.Mana;
                    default:
                        return (EncounterValue)(-1);
                }
            }
        }
    }
}
