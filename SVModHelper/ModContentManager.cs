﻿using Il2CppStarVaders;
using Il2CppStarVaders;
using MelonLoader;
using SVModHelper.ModContent;
using SVModHelper.ModContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SVModHelper
{
    public static class ModContentManager
    {
        internal static bool postInit;

        internal static List<CardModification> cardModifications;
        internal static List<ArtifactModification> artifactModifications;
        internal static List<ComponentModification> componentModifications;
        internal static List<ItemModification> itemModifications;
        internal static List<PackModification> packModifications;
        internal static List<SpellModification> spellModifications;

        internal static Dictionary<CardName, CardModification> activeCardMods;
        internal static Dictionary<ArtifactName, ArtifactModification> activeArtifactMods;
        internal static Dictionary<ComponentName, ComponentModification> activeComponentMods;
        internal static Dictionary<ItemName, ItemModification> activeItemMods;
        internal static Dictionary<ItemPackName, PackModification> activePackMods;
        internal static Dictionary<ArtifactName, SpellModification> activeSpellMods;

        internal static List<AModCard> moddedCards;
        internal static Dictionary<Type, CardName> moddedCardDict;
        internal static Dictionary<CardName, CardViewData> moddedCardVDs;

        internal static List<IHasArtifactID> moddedArtifacts;
        internal static Dictionary<Type, ArtifactName> moddedArtifactDict;
        internal static Dictionary<ArtifactName, Sprite> moddedArtifactVDs;

        internal static List<AModComponent> moddedComponents;
        internal static Dictionary<Type, ComponentName> moddedComponentDict;
        internal static Dictionary<ComponentName, Sprite> moddedComponentVDs;

        internal static List<AModItem> moddedItems;
        internal static Dictionary<Type, ItemName> moddedItemDict;
        internal static Dictionary<ItemName, ItemViewDataSO> moddedItemVDs;

        internal static List<AModPack> moddedPacks;
        internal static Dictionary<Type, ItemPackName> moddedPackDict;
        internal static Dictionary<ItemPackName, Sprite> moddedPackVDs;

        internal static Dictionary<PilotName, AModPilot> moddedPilotDict;

        internal static Dictionary<PilotName, Dictionary<string, Sprite>> moddedPilotSprites;

		internal static Dictionary<Type, string> moddedTaskIDs;
        internal static Dictionary<string, AModTask> moddedTaskInstances;

        internal static Dictionary<string, byte[]> contentData;

        internal const CardName MINCARDID = (CardName)15000;
        internal const ArtifactName MINARTIFACTID = (ArtifactName)15000;
        internal const ComponentName MINCOMPID = (ComponentName)15000;
        internal const ItemName MINITEMID = (ItemName)15000;
        internal const EnemyName MINENEMYID = (EnemyName)15000;
        internal const ItemPackName MINPACKID = (ItemPackName)15000;
        internal const PilotName MINPILOTID = (PilotName)1000;


        public const CardName INVALIDCARDID = (CardName)(-1);
        public const ArtifactName INVALIDARTIFACTID = (ArtifactName)(-1);
        public const ComponentName INVALIDCOMPID = (ComponentName)(-1);
        public const ItemName INVALIDITEMID = (ItemName)(-1);
        public const EnemyName INVALIDENEMYID = (EnemyName)(-1);
        public const ItemPackName INVALIDPACKID = (ItemPackName)(-1);
        public const PilotName INVALIDPILOTID = (PilotName)(-1);
        public const string INVALIDTASKID = "";

        static ModContentManager()
        {
            postInit = false;

            cardModifications = new();
            artifactModifications = new();
            componentModifications = new();
            itemModifications = new();
            packModifications = new();
            spellModifications = new();

            moddedCards = new();
            moddedCardDict = new();
            moddedCardVDs = new();

            moddedArtifacts = new();
            moddedArtifactDict = new();
            moddedArtifactVDs = new();

            moddedComponents = new();
            moddedComponentDict = new();
            moddedComponentVDs = new();

            moddedItems = new();
            moddedItemDict = new();
            moddedItemVDs = new();

            moddedPacks = new();
            moddedPackDict = new();
            moddedPackVDs = new();

            moddedPilotDict = new();
            moddedPilotSprites = new();

            moddedTaskIDs = new();
            moddedTaskInstances = new();

            contentData = new();
        }

        #region Modifications
        internal static void ApplyMods()
        {
            ApplyCardMods();
            ApplyArtifactMods();
            ApplyComponentMods();
            ApplyItemMods();
            ApplyPackMods();
        }

        private static void ApplyCardMods()
        {
            activeCardMods = new();
            foreach(CardModification cardMod in cardModifications)
            {
                if(!activeCardMods.TryGetValue(cardMod.targetCard, out CardModification activeMod))
                {
                    activeMod = new CardModification(cardMod.targetCard);
                    activeCardMods.Add(cardMod.targetCard, activeMod);
                }
                cardMod.CopyTo(activeMod);
            }

            foreach(CardModification activeMod in activeCardMods.Values)
            {
                if (activeMod.displayName != null)
                    SetCardTitle(activeMod.targetCard, activeMod.displayName);
                if (activeMod.description != null)
                    SetCardDesc(activeMod.targetCard, activeMod.description);
                if (activeMod.cardView != null)
                    SetCardImage(activeMod.targetCard, activeMod.cardView);
            }
        }

        private static void ApplyArtifactMods()
        {
            HashSet<ArtifactName> extraEncounterModifiers = new();
            HashSet<ArtifactName> extraCurseModifiers = new();
            HashSet<ArtifactName> removedEncounterModifiers = new();
            HashSet<ArtifactName> removedCurseModifiers = new();

            activeArtifactMods = new();
            foreach (IHasArtifactID artifact in moddedArtifacts)
            {
                if (artifact is AModArtifact modArtifact)
                {
                    if (modArtifact.IsEncounterModifier)
                    {
                        extraEncounterModifiers.Add(modArtifact.ArtifactName);
                    }
                    if(modArtifact.IsCurseModifier)
                    {
                        extraCurseModifiers.Add(modArtifact.ArtifactName);
                    }
                }
            }
            foreach (ArtifactModification artifactMod in artifactModifications)
            {
                if (!activeArtifactMods.TryGetValue(artifactMod.targetArtifact, out ArtifactModification activeMod))
                {
                    activeMod = new ArtifactModification(artifactMod.targetArtifact);
                    activeArtifactMods.Add(artifactMod.targetArtifact, activeMod);
                }
                artifactMod.CopyTo(activeMod);
            }

            foreach (ArtifactModification activeMod in activeArtifactMods.Values)
            {
                if (activeMod.displayName != null)
                    SetArtifactTitle(activeMod.targetArtifact, activeMod.displayName);
                if (activeMod.description != null)
                    SetArtifactDesc(activeMod.targetArtifact, activeMod.description);
                if (activeMod.sprite != null)
                    SetArtifactImage(activeMod.targetArtifact, activeMod.sprite);

                if(activeMod.isEncounterModifier == true)
                {
                    extraEncounterModifiers.Add(activeMod.targetArtifact);
                }
                else if(activeMod.isEncounterModifier == false)
                {
                    extraEncounterModifiers.Remove(activeMod.targetArtifact);
                    removedEncounterModifiers.Add(activeMod.targetArtifact);
                }
                if(activeMod.isCurseModifier == true)
                {
                    extraCurseModifiers.Add(activeMod.targetArtifact);
                }
                else
                {
                    extraCurseModifiers.Remove(activeMod.targetArtifact);
                    removedCurseModifiers.Add(activeMod.targetArtifact);
                }
            }

            EncounterModifierFixer.SetMods(extraEncounterModifiers, removedEncounterModifiers);
            CurseModifierFixer.SetMods(extraCurseModifiers, removedCurseModifiers);
        }

        private static void ApplyComponentMods()
        {
            activeComponentMods = new();
            foreach (ComponentModification componentMod in componentModifications)
            {
                if (!activeComponentMods.TryGetValue(componentMod.targetComponent, out ComponentModification activeMod))
                {
                    activeMod = new ComponentModification(componentMod.targetComponent);
                    activeComponentMods.Add(componentMod.targetComponent, activeMod);
                }
                componentMod.CopyTo(activeMod);
            }

            foreach (ComponentModification activeMod in activeComponentMods.Values)
            {
                if (activeMod.displayName != null)
                    SetComponentTitle(activeMod.targetComponent, activeMod.displayName);
                if (activeMod.description != null)
                    SetComponentDesc(activeMod.targetComponent, activeMod.description);
                if (activeMod.sprite != null)
                    SetComponentImage(activeMod.targetComponent, activeMod.sprite);
            }
        }

        private static void ApplyItemMods()
        {
            activeItemMods = new();
            foreach(ItemModification itemMod in itemModifications)
            {
                if(!activeItemMods.TryGetValue(itemMod.targetItem, out ItemModification activeMod))
                {
                    activeMod = new ItemModification(itemMod.targetItem);
                    activeItemMods.Add(itemMod.targetItem, activeMod);
                }
                itemMod.CopyTo(activeMod);
            }

            foreach(ItemModification activeMod in activeItemMods.Values)
            {
                if (activeMod.displayName != null)
                    SetItemTitle(activeMod.targetItem, activeMod.displayName);
                if (activeMod.description != null)
                    SetItemDesc(activeMod.targetItem, activeMod.description);
                //this part currently doesn't work
                if (activeMod.newViewData != null)
                    SetItemImage(activeMod.targetItem, activeMod.newViewData);
            }
        }

        private static void ApplyPackMods()
        {
            foreach (AModPack pack in moddedPacks)
            {
                ItemPack newPack = pack.Convert();
                ItemPackData.AllPackData.Add(newPack);
            }

            activePackMods = new();
            foreach (PackModification packMod in packModifications)
            {
                if (!activePackMods.TryGetValue(packMod.targetPack, out PackModification activeMod))
                {
                    activeMod = new PackModification(packMod.targetPack);
                    activePackMods.Add(packMod.targetPack, activeMod);
                }
                packMod.CopyTo(activeMod);
            }

            foreach (PackModification activeMod in activePackMods.Values)
            {
                if (activeMod.displayName != null)
                    SetPackTitle(activeMod.targetPack, activeMod.displayName);
                if (activeMod.description != null)
                    SetPackDesc(activeMod.targetPack, activeMod.description);
                if (activeMod.sprite != null)
                    SetPackImage(activeMod.targetPack, activeMod.sprite);
            }

            foreach(ItemPack pack in ItemPackData.AllPackData)
            {
                if(activePackMods.TryGetValue(pack.ItemPackName, out PackModification mod))
                {
                    mod.ApplyTo(pack);
                }
            }

            //doing this so that A: all changes applied to AllPackData also apply to ClassicPackData
            //and B: so that I don't have to worry about vanilla packs being duplicated instead of shared between the two lists.
            ItemPackData._TrueClassicPackData_k__BackingField = ItemPackData.AllPackData.ToMono().Where(pack => !pack.IsHidden).ToList().ToILCPP();
        }
        #endregion

        #region Cards
        internal static string SetCardTitle(CardName cardName, string title)
        {
            string id = cardName.ToString() + "_CardTitle";
            return SetLocalizedString(id, title);
        }

        internal static string SetCardDesc(CardName cardName, string desc)
        {
            string id = cardName.ToString() + "_CardDesc";
            return SetLocalizedString(id, desc);
        }

        internal static void SetCardImage(CardName cardName, CardViewData cardViewData)
        {
            if(cardViewData != null)
                moddedCardVDs[cardName] = cardViewData;
        }

        public static CardName GetModCardName<T>() where T : AModCard
        {
            return GetModCardName(typeof(T));
        }

        public static CardName GetModCardName(Type cardType)
        {
            if (moddedCardDict.TryGetValue(cardType, out CardName id))
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
        internal static string SetArtifactTitle(ArtifactName artifactName, string title)
        {
            string id = artifactName.ToString() + "_ArtiTitle";
            return SetLocalizedString(id, title);
        }

        internal static string SetArtifactDesc(ArtifactName artifactName, string desc)
        {
            string id = artifactName.ToString() + "_ArtiDesc";
            return SetLocalizedString(id, desc);
        }

        internal static void SetArtifactImage(ArtifactName artifactName, Sprite sprite)
        {
            if (sprite != null)
                moddedArtifactVDs[artifactName] = sprite;
        }

        public static ArtifactName GetModArtifactName<T>() where T : IHasArtifactID
        {
            return GetModArtifactName(typeof(T));
        }

        public static ArtifactName GetModArtifactName(Type artifactType)
        {
            if (moddedArtifactDict.TryGetValue(artifactType, out ArtifactName id))
            {
                return id;
            }
            return INVALIDARTIFACTID;
        }

        public static IHasArtifactID GetModArtifactInstance(ArtifactName artifactName)
        {
            if (artifactName < MINARTIFACTID || artifactName >= MINARTIFACTID + moddedArtifacts.Count)
                return null;
            return moddedArtifacts[artifactName - MINARTIFACTID];
        }
        #endregion

        #region Components
        internal static string SetComponentTitle(ComponentName componentName, string title)
        {
            string id = componentName.ToString() + "_CompTitle";
            return SetLocalizedString(id, title);
        }

        internal static string SetComponentDesc(ComponentName componentName, string desc)
        {
            string id = componentName.ToString() + "_CompDesc";
            return SetLocalizedString(id, desc);
        }

        internal static void SetComponentImage(ComponentName componentName, Sprite sprite)
        {
            if (sprite != null)
                moddedComponentVDs[componentName] = sprite;
        }

        public static ComponentName GetModComponentName<T>() where T : AModComponent
        {
            return GetModComponentName(typeof(T));
        }

        public static ComponentName GetModComponentName(Type componentType)
        {
            if (moddedComponentDict.TryGetValue(componentType, out ComponentName id))
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

        #region Items
        internal static string SetItemTitle(ItemName itemName, string title)
        {
            string id = itemName.ToString() + "_EntityTitle";
            return SetLocalizedString(id, title);
        }

        internal static string SetItemDesc(ItemName itemName, string desc)
        {
            string id = itemName.ToString() + "_EntityDesc";
            return SetLocalizedString(id, desc);
        }

        internal static void SetItemImage(ItemName itemName, ItemViewDataSO viewData)
        {
            if (viewData != null)
                moddedItemVDs[itemName] = viewData;
        }

        public static ItemName GetModItemName<T>() where T : AModItem
        {
            return GetModItemName(typeof(T));
        }

        public static ItemName GetModItemName(Type itemType)
        {
            if (moddedItemDict.TryGetValue(itemType, out ItemName id))
            {
                return id;
            }
            return INVALIDITEMID;
        }

        public static AModItem GetModItemInstance(ItemName itemName)
        {
            if (itemName < MINITEMID || itemName >= MINITEMID + moddedItems.Count)
                return null;
            return moddedItems[itemName - MINITEMID];
        }
        #endregion

        #region Packs
        internal static string SetPackTitle(ItemPackName packName, string title)
        {
            string id = packName.ToString() + "_Misc";
            return SetLocalizedString(id, title);
        }

        internal static string SetPackDesc(ItemPackName packName, string desc)
        {
            string id = packName.ToString() + "_Desc_Misc";
            return SetLocalizedString(id, desc);
        }

        internal static void SetPackImage(ItemPackName packName, Sprite sprite)
        {
            if (sprite != null)
                moddedPackVDs[packName] = sprite;
        }

        public static ItemPackName GetModPackName<T>() where T : AModPack
        {
            return GetModPackName(typeof(T));
        }

        public static ItemPackName GetModPackName(Type packType)
        {
            if (moddedPackDict.TryGetValue(packType, out ItemPackName id))
            {
                return id;
            }
            return INVALIDPACKID;
        }

        public static AModPack GetModPackInstance(ItemPackName packName)
        {
            if (packName < MINPACKID || packName >= MINPACKID + moddedPacks.Count)
                return null;
            return moddedPacks[packName - MINPACKID];
        }

        public static AModPack GetModPackInstance<T>() where T : AModPack
        {
            return GetModPackInstance(GetModPackName<T>());
        }
        #endregion

        #region Pilots
        internal static void SetPilotDesc(PilotName pilotName, string desc)
        {
            // The title in the Pilot Selection scene is not localized.

            SetLocalizedString("PilotDescription" + pilotName.ToString() + "_Misc", desc);
        }


        internal static Sprite GetPilotSprite(PilotName pilotName, string spriteKey)
        {            
	        if (!string.IsNullOrEmpty(spriteKey))
	        {
                if (moddedPilotSprites.TryGetValue(pilotName, out var pilotSprites) && pilotSprites.TryGetValue(spriteKey, out var sprite))
                {
                    if (sprite == null) // This code shouldn't actually be reached, but it's a workaround for now.
                    {
	                    // TODO: Use the sprites cached in moddedPilotSprites instead of creating new ones each time.
	                    // This is a temporary fix because the cached sprites are becoming null at some point post-initialization.

                        MelonLoader.MelonLogger.Msg($"[GetPilotSprite] Sprite for pilot {pilotName} - key '{spriteKey}' is null, creating new sprite instance.");

	                    var newSprite = GetModPilotInstance(pilotName).CreateSprite(spriteKey);
                        SetPilotSprite(pilotName, spriteKey, newSprite);
                        return newSprite;
					}
                    

					return sprite;
                }
			}
            
            return SpriteHelper.GetTransparentSprite();
        }

        internal static void SetPilotSprites(PilotName pilotName, AModPilot pilot)
        {
            if (!moddedPilotSprites.ContainsKey(pilotName))
            {
                moddedPilotSprites[pilotName] = new Dictionary<string, Sprite>();
            }

            // Populate sprites based on the pilot's image properties using filenames as keys
            if (!string.IsNullOrEmpty(pilot.FrontPortrait))
                SetPilotSprite(pilotName, pilot.FrontPortrait, pilot.CreateSprite(pilot.FrontPortrait));

            if (!string.IsNullOrEmpty(pilot.FrontPortraitParallax))
                SetPilotSprite(pilotName, pilot.FrontPortraitParallax, pilot.CreateSprite(pilot.FrontPortraitParallax));

            if (!string.IsNullOrEmpty(pilot.PilotTitleSprite))
                SetPilotSprite(pilotName, pilot.PilotTitleSprite, pilot.CreateSprite(pilot.PilotTitleSprite));

            if (!string.IsNullOrEmpty(pilot.CombatPortraitNeutral))
                SetPilotSprite(pilotName, pilot.CombatPortraitNeutral, pilot.CreateSprite(pilot.CombatPortraitNeutral));

            if (!string.IsNullOrEmpty(pilot.CombatPortraitPositive))
                SetPilotSprite(pilotName, pilot.CombatPortraitPositive, pilot.CreateSprite(pilot.CombatPortraitPositive));

            if (!string.IsNullOrEmpty(pilot.CombatPortraitNegative))
                SetPilotSprite(pilotName, pilot.CombatPortraitNegative, pilot.CreateSprite(pilot.CombatPortraitNegative));

            if (!string.IsNullOrEmpty(pilot.CombatPortraitBurning))
                SetPilotSprite(pilotName, pilot.CombatPortraitBurning, pilot.CreateSprite(pilot.CombatPortraitBurning));

            if (!string.IsNullOrEmpty(pilot.CampaignPortrait))
                SetPilotSprite(pilotName, pilot.CampaignPortrait, pilot.CreateSprite(pilot.CampaignPortrait));

            if (!string.IsNullOrEmpty(pilot.VictoryPhoto))
                SetPilotSprite(pilotName, pilot.VictoryPhoto, pilot.CreateSprite(pilot.VictoryPhoto));
	        
            foreach (var pilotEntry in moddedPilotSprites)
            {
                var pilotNameKey = pilotEntry.Key;
                var spriteDict = pilotEntry.Value;
                foreach (var spriteEntry in spriteDict)
                {
                    var spriteKey = spriteEntry.Key;
                    var sprite = spriteEntry.Value;
                    MelonLoader.MelonLogger.Msg($"[PilotSpriteCheck] Pilot: {pilotNameKey}, SpriteKey: '{spriteKey}', Sprite is null: {sprite == null}");
                }
            }
        }

        private static void SetPilotSprite(PilotName pilotName, string spriteKey, Sprite sprite)
        {
	        if (!moddedPilotSprites.ContainsKey(pilotName))
	        {
		        moddedPilotSprites[pilotName] = new Dictionary<string, Sprite>();
	        }
            if (sprite != null)
            {
                moddedPilotSprites[pilotName][spriteKey] = sprite;

                MelonLoader.MelonLogger.Msg($"[SetPilotSprite] Set sprite for {pilotName} with key {spriteKey}");
            }
	        
        }

		public static PilotName GetModPilotName<T>() where T : AModPilot
        {
            return GetModPilotName(typeof(T));
        }

        public static PilotName GetModPilotName(Type pilotType)
        {
            var pilotEntry = moddedPilotDict.FirstOrDefault(kvp => kvp.Value.GetType() == pilotType);
            return pilotEntry.Value != null ? pilotEntry.Key : INVALIDPILOTID;
        }

        public static AModPilot GetModPilotInstance(PilotName pilotName)
        {
            if (moddedPilotDict.TryGetValue(pilotName, out AModPilot pilot))
            {
                return pilot;
            }
            return null;
        }
        #endregion

        #region Tasks
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

        internal static void CheckInitStatus()
        {
            if (postInit)
                throw new InvalidOperationException("Can not register/modify content after initialization.");
        }

        public static string SetLocalizedString(string stringID, string localizedString)
        {
            CheckInitStatus();
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
