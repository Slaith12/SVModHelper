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

        public abstract string DisplayName { get; }
        public abstract string Description { get; }

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
        public virtual Sprite FrontPortrait => GetStandardSprite(BaseImagePath + "Portrait.png", warnOnFail: false) ?? SpriteHelper.GetTransparentSprite();
        /// <summary>
        /// The sprite displayed in front of the pilot's portrait on the Pilot Selection screen.
        /// Defaults to <code>[BaseImagePath]PortraitParallax.png</code>
        /// </summary>
        public virtual Sprite FrontPortraitParallax => GetStandardSprite(BaseImagePath + "PortraitParallax.png", warnOnFail: false) ?? SpriteHelper.GetTransparentSprite();
        /// <summary>
        /// The sprite used to display the pilot's name on the Pilot Selection screen (standard text is not used for this).
        /// Defaults to <code>[BaseImagePath]Name.png</code>
        /// </summary>
        public virtual Sprite PilotTitleSprite => GetStandardSprite(BaseImagePath + "Name.png", warnOnFail: false) ?? SpriteHelper.GetTransparentSprite();
        /// <summary>
        /// The sprite used in the pilot display during an encounter.
        /// Defaults to <code>[BaseImagePath]CombatNeutral.png</code> or <code>[BaseImagePath]Combat.png</code>
        /// </summary>
        public virtual Sprite CombatPortraitNeutral => GetStandardSprite(BaseImagePath + "CombatNeutral.png", warnOnFail: false)
            ?? GetStandardSprite(BaseImagePath + "Combat.png", warnOnFail: false) ?? SpriteHelper.GetTransparentSprite();
        /// <summary>
        /// The sprite used in the pilot display during an encounter when something good happens.
        /// Defaults to <code>[BaseImagePath]CombatPositive.png</code> or to the neutral combat portrait.
        /// </summary>
        public virtual Sprite CombatPortraitPositive => GetStandardSprite(BaseImagePath + "CombatPositive.png", warnOnFail: false) ?? CombatPortraitNeutral;
        /// <summary>
        /// The sprite used in the pilot display during an encounter when something bad happens.
        /// Defaults to <code>[BaseImagePath]CombatNegative.png</code> or to the neutral combat portrait.
        /// </summary>
        public virtual Sprite CombatPortraitNegative => GetStandardSprite(BaseImagePath + "CombatNegative.png", warnOnFail: false) ?? CombatPortraitNeutral;
        /// <summary>
        /// The sprite used in the pilot display during an encounter when the mech overheats (gunner mech only).
        /// Defaults to <code>[BaseImagePath]CombatBurning.png</code> or to the negative combat portrait.
        /// </summary>
        public virtual Sprite CombatPortraitBurning => GetStandardSprite(BaseImagePath + "CombatBurning.png", warnOnFail: false) ?? CombatPortraitNegative;
        /// <summary>
        /// The sprite used in the pilot display during a campaign outside an encounter.
        /// Defaults to <code>[BaseImagePath]Campaign.png</code>
        /// </summary>
        public virtual Sprite CampaignPortrait => GetStandardSprite(BaseImagePath + "Campaign.png", warnOnFail: false) ?? SpriteHelper.GetTransparentSprite();
        /// <summary>
        /// The sprite used for the photo on the victory screen.
        /// Defaults to <code>[BaseImagePath]Victory.png</code>
        /// </summary>
        public virtual Sprite VictoryPhoto => GetStandardSprite(BaseImagePath + "Victory.png", warnOnFail: false) ?? SpriteHelper.GetTransparentSprite();

        /// <summary>
        /// This pilot's starting cards.
        /// </summary>
        public abstract Il2CppCollections.List<PlayerCardData> StartingCards { get; }

        /// <summary>
        /// This pilot's starting artifact.
        /// </summary>
        public abstract Il2CppCollections.List<ArtifactName> StartingArtifacts { get; }

        public PilotDataSO GetPilotData()
        {
            var result = ScriptableObject.CreateInstance<PilotDataSO>();

            result.StarterData = GetStarterPlayerData();

            result.ClassName = ClassName;
            result.PilotName = PilotName;

            result.Complexity = Complexity;

            // Use centralized sprite management with filenames as keys
            // In the near future, the sprite properties will be updated to use a centralized cache system, similar to above
            result.FrontPortrait = FrontPortrait;
            result.FrontPortraitParallax = FrontPortraitParallax;
            result.PilotTitleSprite = PilotTitleSprite;
            result.CombatPortraitNeutral = CombatPortraitNeutral;
            result.CombatPortraitPositive = CombatPortraitPositive;
            result.CombatPortraitNegative = CombatPortraitNegative;
            result.CombatPortraitBurning = CombatPortraitBurning;
            result.CampaignPortrait = CampaignPortrait;
            result.VictoryPhoto = VictoryPhoto;

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
