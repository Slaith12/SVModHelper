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
	public class Gatling : AModCard
	{


		public override string DisplayName => "Gum-Gum Gatling";

		public override string Description => "Strike 9 random tiles in a radius of 3 around you.";

		public override Il2CppCollections.HashSet<CardTrait> Traits => new HashSet<CardTrait>() { CardTrait.Attack }.ToILCPP();
		public override Il2CppCollections.HashSet<CardTrait> HiddenTraits => new HashSet<CardTrait> { CardTrait.Flow }.ToILCPP();

		public override Rarity Rarity => Rarity.Common;
		public override ClassName Class => ClassName.Melee;
		public override PilotName PilotUnique => ModContentManager.GetModPilotName<LuffyPilot>();

		public override int ClassBaseCost => 1;
		public override bool RequiresPlayerEntity => true;

		public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new HashSet<MoreInfoWordName>() { MoreInfoWordName.Flow }.ToILCPP();

		public override Il2CppCollections.HashSet<ComponentTrait> AllowedComponentTraits => new HashSet<ComponentTrait>() { ComponentTrait.Basic, ComponentTrait.Attack, ComponentTrait.SelectionLess }.ToILCPP();
		public override Il2CppCollections.HashSet<ComponentTrait> BlockedComponentTraits => new HashSet<ComponentTrait> { ComponentTrait.Costed, ComponentTrait.Selection, ComponentTrait.Repeated }.ToILCPP();
		public override Il2CppCollections.HashSet<ComponentName> BlockedComponentNames => new HashSet<ComponentName> { ComponentName.Fluid, ComponentName.Echo }.ToILCPP();
		public override Il2CppCollections.HashSet<ComponentName> AllowedComponentNames => new HashSet<ComponentName> { ComponentName.Tactical, ComponentName.Trap }.ToILCPP();

		public override Il2CppCollections.List<ATask> GetPostSelectionTaskList(OnCreateIDValue cardID)
		{
			return new List<ATask>()
			{
				new GatlingTask(new PlayerCoordValue()).Convert()
			}.ToILCPP();
		}


		public override Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue cardID) => new List<TriggerEffect>{
			TriggerEffectHelpers.GetFlowTriggerEffect(cardID)
		}.ToILCPP();
	}
}
