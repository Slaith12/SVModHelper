using UnityEngine;

namespace SVModHelper.ModContent
{
    public abstract class AModPack : AModContent
    {
        public ItemPackName PackName => ModContentManager.GetModPackName(GetType());

        public abstract string DisplayName { get; }
        public abstract string Description { get; }
        public virtual Sprite Sprite => GetStandardSprite(GetType().Name + ".png", warnOnFail: true);

        public abstract Il2CppCollections.HashSet<CardName> cards { get; }
        public abstract Il2CppCollections.HashSet<ArtifactName> artifacts { get; }
        public virtual bool isHidden => false;

        internal ItemPack Convert()
        {
            return new ItemPack(PackName, cards, artifacts, isHidden);
        }
    }
}
