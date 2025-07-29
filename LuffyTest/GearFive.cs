using Harmony;
using LuffyTest;
using SVModHelper;
using SVModHelper.ModContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuffyTest
{
	public class GearFive : AModArtifact
	{

		public override string DisplayName => "Gear Five";

		public override string Description =>
			"Entities have a 25% chance to drop a MEAT!!! on their tile when destroyed. \nAfter your play 5 Junk cards, transform all Junk cards into random Legendary cards and draw them. Cards in your hand are free this turn.";

		public override ClassName Class => ClassName.Melee;
		public override PilotName PilotUnique => ModContentManager.GetModPilotName<LuffyPilot>(); // Luffy

		public override Rarity Rarity => Rarity.Starter;
		
		public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new HashSet<MoreInfoWordName>()
		{
		}.ToILCPP();

		public override Il2CppCollections.List<ATask> GetTaskList(OnCreateIDValue artifactID)
		{
			return new List<ATask>()
			{
				new GearFiveTask().Convert(),
			}.ToILCPP();
		}

		public override Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue artifactID)
		{
			List<TriggerEffect> result = new List<TriggerEffect>();

			// Chance for entities to drop Meat when destroyed
			var dropMeatTriggerEffect = new TriggerEffect(
				new List<Il2CppSystem.ValueTuple<Trigger, ACondition>> {
					new(Trigger.PostTask,
						new AndCondition(
							new IsTypeCondition<DestroyEntityTask>(new RunningTaskValue()),
							new IsTypeCondition<Coord>(new TaskArgValue(new RunningTaskValue<DestroyEntityTask>(), ArgKey.Coord)),
							new RandomChanceCondition(0.25f)
						)
					)
				}.ToILCPP(),
				new List<ATask>{
					new DropMeatTask(new TaskArgValue(new RunningTaskValue<DestroyEntityTask>(), ArgKey.Coord)).Convert()
				}.ToILCPP()
			);

			result.Add(dropMeatTriggerEffect);

			// Increase charge on player entity whenever we play a junk card

			List<Il2CppSystem.ValueTuple<Trigger, ACondition>> addChargeCondition = new()
			{
				new (Trigger.PostTask,
					new AndCondition(
						new IsTypeCondition<PlayCardEndTask>(new RunningTaskValue()),
						new CardIDHasTraitCondition(new TaskArgValue(new RunningTaskValue<PlayCardEndTask>(), ArgKey.CardID), (int)CardTrait.Junk)
					)
				)
			};

			List<ATask> chargeTask = new()
			{
				new EntityChargeTask(new PlayerIDValue(), Operation.Add, 1)
			};

			result.Add(new TriggerEffect(addChargeCondition.ToILCPP(), chargeTask.ToILCPP()));

			// Once charge reaches 7, activate the artifact.
			List<Il2CppSystem.ValueTuple<Trigger, ACondition>> processArtifactCond = new()
			{
				new (Trigger.PostTask,
					new AndCondition(
						new EqualsCondition(new EntityChargeValue(new PlayerIDValue()), 5)
					)
				)
			};

			List<ATask> processArtifactTask = new()
			{
				new ProcessArtifactTask(artifactID, true),
			};

			result.Add(new TriggerEffect(processArtifactCond.ToILCPP(), processArtifactTask.ToILCPP(), isSelfRemoving: true));

			return result.ToILCPP();
		}
	}
}
