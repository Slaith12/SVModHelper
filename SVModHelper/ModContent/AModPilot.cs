using MelonLoader;
using UnityEngine;

namespace SVModHelper.ModContent
{
    public abstract class AModPilot : AModContent
    {
        public PilotName PilotName => ModContentManager.GetModPilotName(GetType());

        /// <summary>
        /// The class that this pilot belongs to.
        /// </summary>
        public abstract ClassName ClassName { get; }

        /// <summary>
        /// The internal name that the mod helper and other mods will use to reference this pilot.
        /// This cannot match the ID of any other modded pilot.
        /// </summary>
        public virtual string ID => GetType().Assembly.GetName().Name + "-" + GetType().Name;
        /// <summary>
        /// The pilot's name when displayed in text.
        /// Note that some panels use an image for the pilot's name instead, which is defined in PilotTitleSprite.
        /// </summary>
        public abstract string DisplayName { get; }
        /// <summary>
        /// The description that's shown for this pilot (shown on the pilot select screen). Only used when no localization is provided for the current locale.
        /// </summary>
        public abstract string Description { get; }
        /// <summary>
        /// The description that's shown for this pilot on different locales. Falls back to Description for any locales that are missing localizations.
        /// </summary>
        public virtual Dictionary<string, string> LocalizedDescriptions => new();

        public abstract int Complexity { get; }

        /// <summary>
        /// The base path the mod helper uses to search for pilot images.
        /// This is ignored if directly overriding the sprite properties.
        /// </summary>
        public virtual string BaseImagePath => GetType().Name;
        /// <summary>
        /// The sprite used for the pilot's portrait on the Pilot Selection screen.
        /// Defaults to <code>[BaseImagePath]Portrait.png</code>
        /// </summary>
        public virtual SpriteDescriptor FrontPortrait => GetStandardSprite(BaseImagePath + "Portrait.png");
        /// <summary>
        /// The sprite displayed in front of the pilot's portrait on the Pilot Selection screen.
        /// Defaults to <code>[BaseImagePath]PortraitParallax.png</code>
        /// </summary>
        public virtual SpriteDescriptor FrontPortraitParallax => GetStandardSprite(BaseImagePath + "PortraitParallax.png");
        /// <summary>
        /// The sprite used to display the pilot's name on the Pilot Selection screen (standard text is not used for this).
        /// Defaults to <code>[BaseImagePath]Name.png</code>
        /// </summary>
        public virtual SpriteDescriptor PilotTitleSprite => GetStandardSprite(BaseImagePath + "Name.png");
        /// <summary>
        /// The sprite used in the pilot display during an encounter.
        /// Defaults to <code>[BaseImagePath]CombatNeutral.png</code>
        /// </summary>
        public virtual SpriteDescriptor CombatPortraitNeutral => GetStandardSprite(BaseImagePath + "CombatNeutral.png");
        /// <summary>
        /// The sprite used in the pilot display during an encounter when something good happens.
        /// Defaults to <code>[BaseImagePath]CombatPositive.png</code>
        /// If no sprite is found for this member, the neutral combat portrait is used instead.
        /// </summary>
        public virtual SpriteDescriptor CombatPortraitPositive => GetStandardSprite(BaseImagePath + "CombatPositive.png");
        /// <summary>
        /// The sprite used in the pilot display during an encounter when something bad happens.
        /// Defaults to <code>[BaseImagePath]CombatNegative.png</code>
        /// If no sprite is found for this member, the neutral combat portrait is used instead.
        /// </summary>
        public virtual SpriteDescriptor CombatPortraitNegative => GetStandardSprite(BaseImagePath + "CombatNegative.png");
        /// <summary>
        /// The sprite used in the pilot display during an encounter when the mech overheats (gunner mech only).
        /// Defaults to <code>[BaseImagePath]CombatBurning.png</code>
        /// If no sprite is found for this member, the negative combat portrait is used instead.
        /// </summary>
        public virtual SpriteDescriptor CombatPortraitBurning => GetStandardSprite(BaseImagePath + "CombatBurning.png");
        /// <summary>
        /// The sprite used in the pilot display during a campaign outside an encounter.
        /// Defaults to <code>[BaseImagePath]Campaign.png</code>
        /// </summary>
        public virtual SpriteDescriptor CampaignPortrait => GetStandardSprite(BaseImagePath + "Campaign.png");
        /// <summary>
        /// The sprite used for the photo on the victory screen.
        /// Defaults to <code>[BaseImagePath]Victory.png</code>
        /// </summary>
        public virtual SpriteDescriptor VictoryPhoto => GetStandardSprite(BaseImagePath + "Victory.png");
        /// <summary>
        /// The sprite used on the second panel of the true ending cutscene (the handhake with the alien).
        /// Defaults to <code>[BaseImagePath]Handshake.png</code>
        /// </summary>
        public virtual SpriteDescriptor TrueEndHandshake => GetStandardSprite(BaseImagePath + "Handhake.png");
        /// <summary>
        /// The sprite used on the final panel of the true ending cutscene (the lineup with all pilots).
        /// Defaults to <code>[BaseImagePath]Lineup.png</code>
        /// </summary>
        public virtual SpriteDescriptor TrueEndLineup => GetStandardSprite(BaseImagePath + "Lineup.png");

        /// <summary>
        /// The pilot's dialogue when talking to the Overseer in the true ending. Defaults to a generic sequence for each locale.
        /// </summary>
        public virtual Dictionary<string, (string dialogue1, string dialogue2)> LocalizedTrueEndDialogues => new();

        /// <summary>
        /// This pilot's starting cards.
        /// </summary>
        public abstract Il2CppCollections.List<PlayerCardData> StartingCards { get; }

        /// <summary>
        /// This pilot's starting artifact.
        /// </summary>
        public abstract Il2CppCollections.List<ArtifactName> StartingArtifacts { get; }

        internal ModPilotDescriptor GetFullPilotData(PilotSkinName skinName = PilotSkinName.Standard)
        {
            if (skinName != PilotSkinName.Standard)
                return null;
            ModPilotDescriptor data = new();
            data.PilotName = PilotName;
            data.ClassName = ClassName;
            data.StartingCards = StartingCards;
            data.StartingArtifacts = StartingArtifacts;
            data.Complexity = Complexity;
            data.FrontPortrait = FrontPortrait;
            data.FrontPortraitParallax = FrontPortraitParallax;
            data.FrontPortraitLocked = new();
            data.PilotTitleSprite = PilotTitleSprite;
            data.CombatPortraitNeutral = CombatPortraitNeutral;
            data.CombatPortraitPositive = CombatPortraitPositive;
            data.CombatPortraitNegative = CombatPortraitNegative;
            data.CombatPortraitBurning = CombatPortraitBurning;
            data.CampaignPortrait = CampaignPortrait;
            data.VictoryPhoto = VictoryPhoto;
            return data;
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
