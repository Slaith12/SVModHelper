using Il2CppStarVaders;
using SVModHelper;
using SVModHelper.ModContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuffyTest
{
    public class Rocket : AModCard
    {
        public override CardName CardNameOverride => (CardName)17003;

        public override string DisplayName => "Gum-Gum Rocket";

        public override string Description => "Push your mech in a cardinal direction until you collide. If you collide with an entity, push it 4 tiles away.";

        public override Il2CppCollections.HashSet<CardTrait> Traits => new HashSet<CardTrait>() { CardTrait.Attack, CardTrait.Move }.ToILCPP();

        public override Rarity Rarity => Rarity.Common;
        public override ClassName Class => ClassName.Melee;
        public override PilotName PilotUnique => (PilotName)11000;

        public override int ClassBaseCost => 1;
        public override bool RequiresPlayerEntity => true;

        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new HashSet<MoreInfoWordName> { MoreInfoWordName.Push }.ToILCPP();
        public override Il2CppCollections.HashSet<ComponentTrait> AllowedComponentTraits => new HashSet<ComponentTrait> { ComponentTrait.Basic, ComponentTrait.Attack }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentName> AllowedComponentNames => new HashSet<ComponentName> { }.ToILCPP();
        public override Il2CppCollections.HashSet<ComponentTrait> BlockedComponentTraits => new HashSet<ComponentTrait> { }.ToILCPP();

        public override Il2CppCollections.List<ATask> GetPostSelectionTaskList(OnCreateIDValue cardID)
        {
            return new List<ATask>()
            {
                new TaskQueueTask(new List<ATask> {
                    new RocketTask(new PlayerCoordValue(), new TargetValue()).Convert()
                }.ToILCPP(), relativeCoord: null)
            }.ToILCPP();
        }

        public override Il2CppCollections.List<Selection> GetSelections(OnCreateIDValue cardID) =>
            new List<Selection>
            {
	            new Selection(
		            new AndCondition(
			            new PlayerEntityExistsCondition(),
			            new IsTypeCondition<Coord>(new TargetValue()),
			            new DistanceCondition
			            (
				            new TargetValue(),
				            new PlayerCoordValue(),
				            1,
				            1
			            )
		            ))
			}.ToILCPP();

    }
}
