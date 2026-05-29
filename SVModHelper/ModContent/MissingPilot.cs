using UnityEngine;

namespace SVModHelper.ModContent
{
    public class MissingPilot : AModPilot
    {
        public override string ID => stringID;
        public override string DisplayName => $"{stringID} [Missing]";
        public override string Description => $"This should not be visible. If it is, report this on the discord as a bug with the mod helper.";

        //TODO: replace these 2 sprites so there's a distinct sprite in the run history
        public override Sprite CampaignPortrait => null;
        public override Sprite PilotTitleSprite => null;

        public override Sprite CombatPortraitBurning => null;
        public override Sprite CombatPortraitNegative => null;
        public override Sprite CombatPortraitNeutral => null;
        public override Sprite CombatPortraitPositive => null;
        public override Sprite FrontPortrait => null;
        public override Sprite FrontPortraitParallax => null;
        public override Sprite TrueEndHandshake => null;
        public override Sprite TrueEndLineup => null;
        public override Sprite VictoryPhoto => null;

        public override ClassName ClassName => ClassName.Neutral;
        public override int Complexity => 0;

        public override Il2CppCollections.List<PlayerCardData> StartingCards => new();
        public override Il2CppCollections.List<ArtifactName> StartingArtifacts => new();

        internal PilotName intID;
        internal string stringID;

        public MissingPilot(PilotName pilotName, string id)
        {
            intID = pilotName;
            stringID = id;
        }
    }
}
