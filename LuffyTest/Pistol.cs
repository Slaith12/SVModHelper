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
	public class Pistol : AModCard
	{


		public override string DisplayName => "Gum-Gum Pistol";

		public override string Description => "Strike an entity exactly 2 tiles away.";

		public override Il2CppCollections.HashSet<CardTrait> Traits => new HashSet<CardTrait>() {CardTrait.Attack}.ToILCPP();

		public override Rarity Rarity => Rarity.Starter;
		public override ClassName Class => ClassName.Melee;
		public override PilotName PilotUnique => (PilotName) 11000;

		public override int ClassBaseCost => 1;
		public override bool RequiresPlayerEntity => true;

		public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new HashSet<MoreInfoWordName>() { }.ToILCPP();

		public override Il2CppCollections.HashSet<ComponentTrait> AllowedComponentTraits => new HashSet<ComponentTrait>() {ComponentTrait.Basic, ComponentTrait.Attack}.ToILCPP();
		public override Il2CppCollections.HashSet<ComponentTrait> BlockedComponentTraits => new();
		public override Il2CppCollections.HashSet<ComponentName> AllowedComponentNames => new HashSet<ComponentName>() { }.ToILCPP();
		public override Il2CppCollections.HashSet<ComponentName> BlockedComponentNames => new HashSet<ComponentName>() { }.ToILCPP();

		public override Il2CppCollections.List<ATask> GetPostSelectionTaskList(OnCreateIDValue cardID)
		{
			return new List<ATask>() {
				new PlayAnimationTask(new PlayerIDValue(), AnimKey.Stinger_Slash),
				new JabTask(new PlayerCoordValue(), new TargetValue()),
			}.ToILCPP();
		}

		public override Il2CppCollections.List<Selection> GetSelections(OnCreateIDValue cardID)
		{
			return new List<Selection>() {
				new Selection(
					new AndCondition(
						new PlayerEntityExistsCondition(),
						new IsTypeCondition<Coord>(new TargetValue()),
						new DistanceCondition
						(
							new TargetValue(),
							new PlayerCoordValue(),
							2,
							2
						),
						new NotCondition(new IsCoordEmptyCondition(new TargetValue()))
					))
			}.ToILCPP();
		}
	}
}