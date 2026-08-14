using UnityEngine;

namespace SVModHelper.ModContent
{
    public class MissingPilot : AModPilot
    {
        public override string ID => stringID;
        public override string DisplayName => $"{stringID} [Missing]";
        public override string Description => $"This should not be visible. If it is, report this on the discord as a bug with the mod helper.";

        //TODO: replace these 2 sprites so there's a distinct sprite in the run history
        public override SpriteDescriptor CampaignPortrait => new();
        public override SpriteDescriptor PilotTitleSprite => new();
                              
        public override SpriteDescriptor CombatPortraitBurning => new();
        public override SpriteDescriptor CombatPortraitNegative => new();
        public override SpriteDescriptor CombatPortraitNeutral => new();
        public override SpriteDescriptor CombatPortraitPositive => new();
        public override SpriteDescriptor FrontPortrait => new();
        public override SpriteDescriptor FrontPortraitParallax => new();
        public override SpriteDescriptor TrueEndHandshake => new();
        public override SpriteDescriptor TrueEndLineup => new();
        public override SpriteDescriptor VictoryPhoto => new();

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
