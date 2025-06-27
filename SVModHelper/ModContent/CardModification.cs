using Il2CppStarVaders;

namespace SVModHelper.ModContent
{
    public class CardModification
    {
        internal SVMod m_Source;
        //public SVMod sourceMod => m_Source;
        public CardName targetCard;
        public int priority;

        //I'm not adding all of the card properties to this class because I think if you start
        //changing some of the functionality of the card you might as well just make a new card instead.
        //That said, if you think there's anything that should be here, feel free to message me or post about it in #modding

        public string displayName;
        public string description;
        public CardViewData cardView;

        public ClassName? newClass;
        public PilotName? newPilot;
        public Rarity? newRarity;
        public bool? isToken;
        public bool? isShowable;

        public HashSet<CardTrait> extraTraits = new();
        public HashSet<CardTrait> extraHiddenTraits = new();

        public HashSet<ComponentTrait> extraCompTraitWL = new();
        public HashSet<ComponentTrait> extraCompTraitBL = new();
        public HashSet<ComponentName> extraCompWL = new();
        public HashSet<ComponentName> extraCompBL = new();

        public CardModification(CardName target, int priority = 0)
        {
            this.targetCard = target;
            this.priority = priority;
        }

        public void CopyTo(CardModification other)
        {
            if (displayName != null)
                other.displayName = displayName;
            if (description != null)
                other.description = description;
            if (cardView != null)
                other.cardView = cardView;
            if (newClass != null)
                other.newClass = newClass;
            if (newPilot != null)
                other.newPilot = newPilot;
            if (newRarity != null)
                other.newRarity = newRarity;
            if (isToken != null)
                other.isToken = isToken;
            if (isShowable != null)
                other.isShowable = isShowable;
            if (extraTraits != null)
                other.extraTraits.UnionWith(extraTraits);
            if (extraHiddenTraits != null)
                other.extraHiddenTraits.UnionWith(extraHiddenTraits);
            if (extraCompTraitWL != null)
                other.extraCompTraitWL.UnionWith(extraCompTraitWL);
            if (extraCompTraitBL != null)
                other.extraCompTraitBL.UnionWith(extraCompTraitBL);
            if (extraCompWL != null)
                other.extraCompWL.UnionWith(extraCompWL);
            if (extraCompBL != null)
                other.extraCompBL.UnionWith(extraCompBL);
        }

        internal void ApplyTo(CardModel card)
        {
            if (newClass != null)
                card.Class = newClass.Value;
            if (newPilot != null)
                card.PilotUnique = newPilot.Value;
            if (newRarity != null)
                card.Rarity = newRarity.Value;
            if (isToken != null)
                card.IsToken = isToken.Value;
            if (isShowable != null)
                card.IsShowable = isShowable.Value;

            card.Traits.UnionWith(extraTraits.ToILCPPEnumerable());
            card.HiddenTraits.UnionWith(extraHiddenTraits.ToILCPPEnumerable());
            card.AllowedComponentTraits.UnionWith(extraCompTraitWL.ToILCPPEnumerable());
            card.BlockedComponentTraits.UnionWith(extraCompTraitBL.ToILCPPEnumerable());
            card.AllowedComponentNames.UnionWith(extraCompWL.ToILCPPEnumerable());
            card.BlockedComponentNames.UnionWith(extraCompBL.ToILCPPEnumerable());
        }
    }
}
