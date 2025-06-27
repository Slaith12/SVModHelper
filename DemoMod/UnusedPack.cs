using SVModHelper;
using SVModHelper.ModContent;

namespace DemoMod
{
    public class UnusedPack : AModPack
    {
        public override string DisplayName => "Unused";

        public override string Description => "Adds previously unused cards and artifacts to the pool.";

        public override Il2CppCollections.HashSet<CardName> cards => new HashSet<CardName>()
        {
            CardName.Avarice,
            CardName.Shine,
            CardName.Discipline,
            CardName.Expose,
            CardName.Pyroclasm
        }.ToILCPP();

        public override Il2CppCollections.HashSet<ArtifactName> artifacts => new HashSet<ArtifactName>()
        {
            ArtifactName.FermiReaction,
            ArtifactName.RisingStar
        }.ToILCPP();
    }
}
