using Il2Cpp;
using MelonLoader;
using SVModHelper;
using SVModHelper.ModContent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Il2CppSystem.Runtime.Remoting.RemotingServices;

namespace LuffyTest
{
	internal class GearFiveTask : AModTask
	{
		public GearFiveTask()
		{

		}

		public override IEnumerator Execute(ATask taskInstance)
		{
			if (taskInstance.TaskEngine.IsPreviewMode)
			{
				yield break;
			}

			var encounterModel = taskInstance.TaskEngine.EncounterModel;
			var cardPlayModel = encounterModel.CardPlayModel;

			var junkCards = cardPlayModel.GetPiles((new List<Pile> { Pile.Hand, Pile.Discard, Pile.Draw, Pile.Pickup }).ToILCPP()).ToMono()
				.Where(cardID => encounterModel.GetModelItem<CardModel>(cardID.ToID()).Traits.Contains(CardTrait.Junk))
				.ToList();

			var taskList = new List<ATask> { };

			foreach (CardID junkCard in junkCards)
			{
				var pile = cardPlayModel.GetPileContainingCard(junkCard);

				if (pile == Pile.Pickup)
				{
					var coord = encounterModel.GridModel.GetPickupCoord(junkCard);
					taskList.Add(new DestroyPickupTask(coord, Pile.Purged));
				}
				else
				{
					taskList.Add(new PurgeCardTask(junkCard.BoxIl2CppObject()));
				}

				var legendaryCard = ContentGetter.GetAllCards().ToMono()
					.Where(card => card.Class == DataManager.PlayerData.ClassName || card.Class == ClassName.Neutral)
					.Where(card => card.PilotUnique == DataManager.PlayerData.PilotName || card.PilotUnique == PilotName.None)
					.Where(card => card.Rarity == Rarity.Legendary)
					.Where(card => !card.IsToken)
					.ToList().ToILCPPEnumerable()
					.PickRandom();

				taskList.Add(new CreateCardTask((int)legendaryCard.CardName, ComponentName:ComponentName.None, Pile: Pile.Hand, rarity: new(), coord: new()));
			}

			taskList.Add(new MakeAllCardsInHandFreeThisTurnTask().Convert());

			yield return taskInstance.TaskEngine.ProcessTaskList(taskList.ToILCPP()).Cast<Il2CppSystem.Object>();
		}
	}
}