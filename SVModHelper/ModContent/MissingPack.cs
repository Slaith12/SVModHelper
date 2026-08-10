using UnityEngine;

namespace SVModHelper.ModContent
{
    public class MissingPack : AModPack
    {
        public override string ID => stringID;
        public override string DisplayName => $"{stringID} [Missing]";
        public override string Description => $"This pack was added by a mod when this run was active.\n" +
                                              $"That mod has either since been disabled or no longer includes this pack.";
        public override SpriteDescriptor Sprite => new(); //TODO: Replace this so there's a distinct sprite in the run history

        public override Il2CppCollections.HashSet<CardName> cards => new();
        public override Il2CppCollections.HashSet<ArtifactName> artifacts => new();

        internal ItemPackName intID;
        internal string stringID;

        public MissingPack(ItemPackName packName, string id)
        {
            intID = packName;
            stringID = id;
        }
    }
}
