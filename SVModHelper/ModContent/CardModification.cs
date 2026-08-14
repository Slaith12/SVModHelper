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
        public Dictionary<string, string> localizedNames = new();
        public Dictionary<string, string> localizedDescriptions = new();
        public CardViewDescriptor? cardView;

        public ClassName? newClass;
        public PilotName? newPilot;
        public Rarity? newRarity;
        public int? newCost;
        public bool? isToken;
        public bool? isShowable;

        public HashSet<CardTrait> extraTraits = new();
        public HashSet<CardTrait> extraHiddenTraits = new();

        public ContextPreviewType? newPreviewType;
        public HashSet<MoreInfoWordName> extraMoreInfoWords = new();
        public HashSet<CardName> extraMoreInfoCards = new();
        public HashSet<ItemName> extraMoreInfoItems = new();
        public HashSet<EnemyName> extraMoreInfoEnemies = new();

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
            foreach(var locName in localizedNames)
            {
                other.localizedNames[locName.Key] = locName.Value;
            }
            foreach (var locDesc in localizedDescriptions)
            {
                other.localizedDescriptions[locDesc.Key] = locDesc.Value;
            }
            if (cardView != null)
                other.cardView = cardView;
            if (newClass != null)
                other.newClass = newClass;
            if (newPilot != null)
                other.newPilot = newPilot;
            if (newRarity != null)
                other.newRarity = newRarity;
            if (newCost != null)
                other.newCost = newCost;
            if (isToken != null)
                other.isToken = isToken;
            if (isShowable != null)
                other.isShowable = isShowable;
            if (newPreviewType != null)
                other.newPreviewType = newPreviewType;
            if (extraTraits != null)
                other.extraTraits.UnionWith(extraTraits);
            if (extraHiddenTraits != null)
                other.extraHiddenTraits.UnionWith(extraHiddenTraits);
            if (extraMoreInfoWords != null)
                other.extraMoreInfoWords.UnionWith(extraMoreInfoWords);
            if (extraMoreInfoCards != null)
                other.extraMoreInfoCards.UnionWith(extraMoreInfoCards);
            if (extraMoreInfoItems != null)
                other.extraMoreInfoItems.UnionWith(extraMoreInfoItems);
            if (extraMoreInfoEnemies != null)
                other.extraMoreInfoEnemies.UnionWith(extraMoreInfoEnemies);
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
            if (newCost != null)
            {
                switch (ModContentManager.GetAppropriateCostType(card))
                {
                    case EncounterValue.Heat:
                        card.HeatCost = newCost.Value;
                        break;
                    case EncounterValue.Power:
                        card.PowerCost = newCost.Value;
                        break;
                    case EncounterValue.Mana:
                        card.ManaCost = newCost.Value;
                        break;
                    default:
                        card.HeatCost = newCost.Value;
                        break;
                }
            }
            if (isToken != null)
                card.IsToken = isToken.Value;
            if (isShowable != null)
                card.IsShowable = isShowable.Value;
            if (newPreviewType != null)
                card.ContextPreviewType = newPreviewType.Value;

            card.Traits.UnionWith(extraTraits.ToILCPPEnumerable());
            card.HiddenTraits.UnionWith(extraHiddenTraits.ToILCPPEnumerable());
            card.MoreInfoWordNames.UnionWith(extraMoreInfoWords.ToILCPPEnumerable());
            card.MoreInfoCardNames.UnionWith(extraMoreInfoCards.ToILCPPEnumerable());
            card.MoreInfoItemNames.UnionWith(extraMoreInfoItems.ToILCPPEnumerable());
            card.MoreInfoEnemyNames.UnionWith(extraMoreInfoEnemies.ToILCPPEnumerable());
            card.AllowedComponentTraits.UnionWith(extraCompTraitWL.ToILCPPEnumerable());
            card.BlockedComponentTraits.UnionWith(extraCompTraitBL.ToILCPPEnumerable());
            card.AllowedComponentNames.UnionWith(extraCompWL.ToILCPPEnumerable());
            card.BlockedComponentNames.UnionWith(extraCompBL.ToILCPPEnumerable());
        }
    }
}
