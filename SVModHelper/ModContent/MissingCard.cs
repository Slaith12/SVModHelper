
namespace SVModHelper.ModContent
{
    public class MissingCard : AModCard
    {
        public override string ID => stringID;
        public override string DisplayName => $"Missing Card: \"{stringID}\"";
        public override string Description => $"This card was added by a mod when this run was active.\n" +
                                              $"That mod has either since been disabled or no longer includes this card.";

        public override Il2CppCollections.HashSet<CardTrait> Traits => new();

        public override ClassName Class => ClassName.Neutral;

        public override Rarity Rarity => Rarity.TempRemoved;

        internal CardName intID;
        internal string stringID;

        public MissingCard(CardName cardName, string id)
        {
            intID = cardName;
            stringID = id;
        }
    }
}
