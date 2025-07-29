namespace SVModHelper.ModContent
{
    public class ItemModification
    {
        internal SVMod m_Source;
        //public SVMod sourceMod => m_Source;
        public ItemName targetItem;
        public int priority;

        public string displayName;
        public string description;
        public ItemViewDataSO newViewData;

        public HashSet<EntityTrait> extraTraits = new();
        public HashSet<EntityTrait> extraHiddenTraits = new();

        public int? health;
        public int? starbucksReward;
        public bool? isShielded;
        public bool? isInvulnerable;
        public bool? isStatic;
        public bool? isNotShieldable;
        public bool? isNotMoveSelectable;
        public bool? isNotSwapSelectable;
        public bool? isNotStatusEffectSelectable;
        public bool? hasRegeneratingShield;
        public int? doomLevel;

        public ItemModification(ItemName target, int priority = 0)
        {
            this.targetItem = target;
            this.priority = priority;
        }

        public void CopyTo(ItemModification other)
        {
            if (displayName != null)
                other.displayName = displayName;
            if (description != null)
                other.description = description;
            if (newViewData != null)
                other.newViewData = newViewData;

            if (extraTraits != null)
                other.extraTraits.UnionWith(extraTraits);
            if (extraHiddenTraits != null)
                other.extraHiddenTraits.UnionWith(extraHiddenTraits);

            if (health != null)
                other.health = health;
            if (starbucksReward != null)
                other.starbucksReward = starbucksReward;
            if (isShielded != null)
                other.isShielded = isShielded;
            if (isInvulnerable != null)
                other.isInvulnerable = isInvulnerable;
            if (isStatic != null)
                other.isStatic = isStatic;
            if (isNotShieldable != null)
                other.isNotShieldable = isNotShieldable;
            if (isNotMoveSelectable != null)
                other.isNotMoveSelectable = isNotMoveSelectable;
            if (isNotSwapSelectable != null)
                other.isNotSwapSelectable = isNotSwapSelectable;
            if (isNotStatusEffectSelectable != null)
                other.isNotStatusEffectSelectable = isNotStatusEffectSelectable;
            if (hasRegeneratingShield != null)
                other.hasRegeneratingShield = hasRegeneratingShield;
            if (doomLevel != null)
                other.doomLevel = doomLevel;
        }

        internal void ApplyTo(ItemEntityModel item)
        {
            item.Traits.UnionWith(extraTraits.ToILCPPEnumerable());
            item.HiddenTraits.UnionWith(extraHiddenTraits.ToILCPPEnumerable());

            if (health != null)
                item.Health = health.Value;
            if (starbucksReward != null)
                item.StarbucksReward = starbucksReward.Value;
            if (isShielded != null)
                item.IsShielded = isShielded.Value;
            if (isInvulnerable != null)
                item.IsInvulnerable = isInvulnerable.Value;
            if (isStatic != null)
                item.IsStatic = isStatic.Value;
            if (isNotShieldable != null)
                item.IsNotShieldable = isNotShieldable.Value;
            if (isNotMoveSelectable != null)
                item.IsNotMoveSelectable = isNotMoveSelectable.Value;
            if (isNotSwapSelectable != null)
                item.IsNotSwapSelectable = isNotSwapSelectable.Value;
            if (isNotStatusEffectSelectable != null)
                item.IsNotStatusEffectSelectable = isNotStatusEffectSelectable.Value;
            if (hasRegeneratingShield != null)
                item.HasRegeneratingShield = hasRegeneratingShield.Value;
            if (doomLevel != null)
                item.DoomLevel = doomLevel.Value;
        }
    }
}
