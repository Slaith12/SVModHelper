using UnityEngine;

namespace SVModHelper.ModContent
{
    public class ArtifactModification
    {
        internal SVMod m_Source;
        //public SVMod sourceMod => m_Source;
        public ArtifactName targetArtifact;
        public int priority;

        //I'm not adding all of the artifact properties to this class because I think if you start
        //changing some of the functionality of the artifact you might as well just make a new artifact instead.
        //That said, if you think there's anything that should be here, feel free to message me or post about it in #modding

        public string displayName;
        public string description;
        public Sprite sprite;

        public Rarity? newRarity;
        public bool? canBeDuplicated;

        public ArtifactModification(ArtifactName target, int priority = 0)
        {
            this.targetArtifact = target;
            this.priority = priority;
        }

        public void CopyTo(ArtifactModification other)
        {
            if (displayName != null)
                other.displayName = displayName;
            if (description != null)
                other.description = description;
            if (sprite != null)
                other.sprite = sprite;
            if (newRarity != null)
                other.newRarity = newRarity;
            if (canBeDuplicated != null)
                other.canBeDuplicated = canBeDuplicated;
        }

        internal void ApplyTo(ArtifactModel artifact)
        {
            if (newRarity != null)
                artifact.Rarity = newRarity.Value;
            if (canBeDuplicated != null)
                artifact.CanBeDuplicated = canBeDuplicated.Value;
        }
    }
}
