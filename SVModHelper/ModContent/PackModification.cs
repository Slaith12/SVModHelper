using UnityEngine;

namespace SVModHelper.ModContent
{
    public class PackModification
    {
        internal SVMod m_Source;
        public SVMod sourceMod => m_Source;
        public ItemPackName targetPack;
        public int priority;

        public string displayName;
        public string description;
        public Sprite sprite;

        public bool? isHidden;

        public HashSet<CardName> extraPackCards = new();
        public HashSet<CardName> excludedPackCards = new();
        public HashSet<ArtifactName> extraPackArtifacts = new();
        public HashSet<ArtifactName> excludedPackArtifacts = new();

        public PackModification(ItemPackName target, int priority = 0)
        {
            this.targetPack = target;
            this.priority = priority;
        }

        public void CopyTo(PackModification other)
        {
            if (displayName != null)
                other.displayName = displayName;
            if (description != null)
                other.description = description;
            if (sprite != null)
                other.sprite = sprite;
            if (isHidden != null)
                other.isHidden = isHidden;
            if (extraPackCards != null)
            {
                other.extraPackCards.UnionWith(extraPackCards);
                other.excludedPackCards.ExceptWith(extraPackCards);
            }
            if (excludedPackCards != null)
            {
                other.excludedPackCards.UnionWith(excludedPackCards);
                other.extraPackCards.ExceptWith(excludedPackCards);
            }
            if (extraPackArtifacts != null)
            {
                other.extraPackArtifacts.UnionWith(extraPackArtifacts);
                other.excludedPackArtifacts.ExceptWith(extraPackArtifacts);
            }
            if (excludedPackArtifacts != null)
            {
                other.excludedPackArtifacts.UnionWith(excludedPackArtifacts);
                other.extraPackArtifacts.ExceptWith(excludedPackArtifacts);
            }
        }

        internal void ApplyTo(ItemPack pack)
        {
            if (isHidden != null)
                pack.IsHidden = isHidden.Value;

            pack.Cards.UnionWith(extraPackCards.ToILCPPEnumerable());
            foreach (CardName card in excludedPackCards)
                pack.Cards.Remove(card);

            pack.Artifacts.UnionWith(extraPackArtifacts.ToILCPPEnumerable());
            foreach (ArtifactName artifact in excludedPackArtifacts)
                pack.Artifacts.Remove(artifact);
        }
    }
}
