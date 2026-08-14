using MelonLoader;
using UnityEngine;

namespace SVModHelper.ModContent
{
    internal class ModPilotDescriptor
    {
        public PilotName PilotName;
        public ClassName ClassName;
        public Il2CppCollections.List<PlayerCardData> StartingCards;
        public Il2CppCollections.List<ArtifactName> StartingArtifacts;
        public int Complexity;

        /// <summary>
        /// The sprite used for the pilot's portrait on the Pilot Selection screen.
        /// </summary>
        public SpriteDescriptor FrontPortrait;
        /// <summary>
        /// The sprite displayed in front of the pilot's portrait on the Pilot Selection screen.
        /// </summary>
        public SpriteDescriptor FrontPortraitParallax;
        /// <summary>
        /// The sprite used for the pilot's portrait on the Pilot Selection screen when the pilot is not unlocked.
        /// </summary>
        public SpriteDescriptor FrontPortraitLocked;
        /// <summary>
        /// The sprite used to display the pilot's name on the Pilot Selection screen (standard text is not used for this).
        /// </summary>
        public SpriteDescriptor PilotTitleSprite;
        /// <summary>
        /// The sprite used in the pilot display during an encounter.
        /// </summary>
        public SpriteDescriptor CombatPortraitNeutral;
        /// <summary>
        /// The sprite used in the pilot display during an encounter when something good happens.
        /// </summary>
        public SpriteDescriptor CombatPortraitPositive;
        /// <summary>
        /// The sprite used in the pilot display during an encounter when something bad happens.
        /// </summary>
        public SpriteDescriptor CombatPortraitNegative;
        /// <summary>
        /// The sprite used in the pilot display during an encounter when the mech overheats (gunner mech only).
        /// </summary>
        public SpriteDescriptor CombatPortraitBurning;
        /// <summary>
        /// The sprite used in the pilot display during a campaign outside an encounter.
        /// </summary>
        public SpriteDescriptor CampaignPortrait;
        /// <summary>
        /// The sprite used for the photo on the victory screen.
        /// </summary>
        public SpriteDescriptor VictoryPhoto;

        public PilotDataSO GetPilotDataSO(SpriteHelper.LogLevel spriteLogLevel = SpriteHelper.LogLevel.MissOrFail)
        {
            var result = ScriptableObject.CreateInstance<PilotDataSO>();

            result.StarterData = GetStarterPlayerData();

            result.ClassName = ClassName;
            result.PilotName = PilotName;
            result.SkinName = PilotSkinName.Standard;

            result.Complexity = Complexity;

            result.FrontPortrait = SpriteHelper.GetSprite(FrontPortrait, spriteLogLevel) ?? SpriteHelper.GetTransparentSprite();
            result.FrontPortraitParallax = SpriteHelper.GetSprite(FrontPortraitParallax, spriteLogLevel) ?? SpriteHelper.GetTransparentSprite();
            result.PilotTitleSprite = SpriteHelper.GetSprite(PilotTitleSprite, spriteLogLevel) ?? SpriteHelper.GetTransparentSprite();
            result.CombatPortraitNeutral = SpriteHelper.GetSprite(CombatPortraitNeutral, spriteLogLevel) ?? SpriteHelper.GetTransparentSprite();
            result.CombatPortraitPositive = SpriteHelper.GetSprite(CombatPortraitPositive, spriteLogLevel) ?? result.CombatPortraitNeutral;
            result.CombatPortraitNegative = SpriteHelper.GetSprite(CombatPortraitNegative, spriteLogLevel) ?? result.CombatPortraitNeutral;
            result.CombatPortraitBurning = SpriteHelper.GetSprite(CombatPortraitBurning, spriteLogLevel) ?? result.CombatPortraitNegative;
            result.CampaignPortrait = SpriteHelper.GetSprite(CampaignPortrait, spriteLogLevel) ?? SpriteHelper.GetTransparentSprite();
            result.VictoryPhoto = SpriteHelper.GetSprite(VictoryPhoto, spriteLogLevel) ?? SpriteHelper.GetTransparentSprite();

            return result;
        }

        public PlayerDataSO GetStarterPlayerData()
        {
            PlayerDataSO playerDataSO = ScriptableObject.CreateInstance<PlayerDataSO>();

            playerDataSO.starbucksAmount = 75;

            playerDataSO.ClassName = ClassName;
            playerDataSO.PilotName = PilotName;

            playerDataSO.startingMaxHeat = 0;

            if (playerDataSO.ClassName == ClassName.Gunner)
            {
                playerDataSO.ClassBaseEnergy = EncounterValue.Heat;
                playerDataSO.startingMaxHeat = 3;
            }
            else if (playerDataSO.ClassName == ClassName.Melee)
            {
                playerDataSO.ClassBaseEnergy = EncounterValue.Power;
                playerDataSO.startingMaxPower = 3;
                playerDataSO.startingPowerCell = 2;
            }
            else if (playerDataSO.ClassName == ClassName.Mystic)
            {
                playerDataSO.ClassBaseEnergy = EncounterValue.Mana;
                playerDataSO.startingMaxMana = 5;
            }
            else
            {
                Melon<Core>.Logger.Error($"Pilot {GetType().Name} uses invalid class {ClassName}. Various issues may occur.");
            }

            playerDataSO.deckCardDataList = StartingCards;
            playerDataSO.artifactList = StartingArtifacts;

            return playerDataSO;
        }
    }
}
