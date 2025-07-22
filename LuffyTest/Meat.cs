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
    public class Meat : AModCard
    {
	    public override CardName CardNameOverride => (CardName) 17000;

	    public override string DisplayName => "MEAT!!!";

        public override string Description => "Gain between 1 to 2 Power. If unplayed, purge this card.";

        public override Il2CppCollections.HashSet<CardTrait> Traits => new HashSet<CardTrait>() { CardTrait.Junk }.ToILCPP();
        public override Il2CppCollections.HashSet<CardTrait> HiddenTraits => new HashSet<CardTrait> { CardTrait.Flow }.ToILCPP();

		public override Rarity Rarity => Rarity.Special;
        public override ClassName Class => ClassName.Melee;
        public override int ClassBaseCost => 1;
        public override bool RequiresPlayerEntity => true;

        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new HashSet<MoreInfoWordName>() { MoreInfoWordName.Power }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentTrait> AllowedComponentTraits => new HashSet<ComponentTrait>() { ComponentTrait.Basic, ComponentTrait.SelectionLess }.ToILCPP();
        public override Il2CppCollections.HashSet<ComponentTrait> BlockedComponentTraits => new();
        public override Il2CppCollections.HashSet<ComponentName> AllowedComponentNames => new HashSet<ComponentName>() {}.ToILCPP();
        public override Il2CppCollections.HashSet<ComponentName> BlockedComponentNames => new HashSet<ComponentName>() {}.ToILCPP();

        public override Il2CppCollections.List<ATask> GetPostSelectionTaskList(OnCreateIDValue cardID)
        {
            return new List<ATask>()
            {
                new EncounterValueOperationTask(EncounterValue.BaseEnergy, Operation.Add, UnityEngine.Random.Range(1,3)),
            }.ToILCPP();
        }

        public override Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue cardID) => new List<TriggerEffect>{
	        TriggerEffectHelpers.GetFlowTriggerEffect(cardID),
	        new TriggerEffect(
		        new List<Il2CppSystem.ValueTuple<Trigger, ACondition>> {
			        new(Trigger.PreTask, 
				        new AndCondition(
					        new IsTypeCondition<EndTurnTask>(new RunningTaskValue()),
					        new CardInPileCondition(cardID, Pile.Hand)
				        )
				    )
		        }.ToILCPP(),
                new List<ATask>{
	                new PurgeCardTask(cardID)
                }.ToILCPP()
            )
		}.ToILCPP();
	}
}
