using UnityEngine;

namespace SVModHelper.ModContent
{
    public abstract class AModPack : AModContent
    {
        public ItemPackName PackName => ModContentManager.GetModPackName(GetType());

        /// <summary>
        /// The name that's shown for this pack. Only used when no localization is provided for the current locale.
        /// </summary>
        public abstract string DisplayName { get; }
        /// <summary>
        /// The description that's shown for this pack (shown in Bud's Factory). Only used when no localization is provided for the current locale.
        /// </summary>
        public abstract string Description { get; }
        /// <summary>
        /// The name that's shown for this pack on different locales. Falls back to DisplayName for any locales that are missing localizations.
        /// </summary>
        public virtual Dictionary<string, string> LocalizedNames => new();
        /// <summary>
        /// The description that's shown for this pack on different locales. Falls back to Description for any locales that are missing localizations.
        /// </summary>
        public virtual Dictionary<string, string> LocalizedDescriptions => new();
        public virtual Sprite Sprite => GetStandardSprite(GetType().Name + ".png", warnOnFail: true);

        public abstract Il2CppCollections.HashSet<CardName> cards { get; }
        public abstract Il2CppCollections.HashSet<ArtifactName> artifacts { get; }
        public virtual bool isHidden => false;

        internal ItemPack Convert()
        {
            ItemPackName name = PackName;
            if (name == ModContentManager.INVALIDPACKID)
                throw new InvalidOperationException($"Attempted to use un-registered pack {GetType()}.");
            return new ItemPack(PackName, cards, artifacts, isHidden);
        }

        public static implicit operator ItemPack(AModPack modPack)
        {
            return modPack.Convert();
        }
    }
}
