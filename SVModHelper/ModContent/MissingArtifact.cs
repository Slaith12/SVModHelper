using UnityEngine;

namespace SVModHelper.ModContent
{
    public class MissingArtifact : AModArtifact
    {
        public override string ID => stringID;
        public override string DisplayName => $"{stringID} [Missing]";
        public override string Description => $"This artifact was added by a mod when this run was active.\n" +
                                              $"That mod has either since been disabled or no longer includes this artifact.";
        public override SpriteDescriptor Sprite => new(); //TODO: Replace this so there's a distinct sprite in the run history

        public override ClassName Class => ClassName.Neutral;

        public override Rarity Rarity => Rarity.TempRemoved;

        internal ArtifactName intID;
        internal string stringID;

        public MissingArtifact(ArtifactName artifactName, string id)
        {
            intID = artifactName;
            stringID = id;
        }
    }
}
