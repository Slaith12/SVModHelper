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

namespace LuffyTest
{
	internal class MakeAllCardsInHandFreeThisTurnTask : AModTask
	{
		public MakeAllCardsInHandFreeThisTurnTask()
		{

		}

		public override IEnumerator Execute(ATask taskInstance)
		{
			List<CardID> handCards = taskInstance.EncounterModel.CardPlayModel.GetPile(Pile.Hand).ToMono();

			foreach (CardID card in handCards)
			{
				//draw the card and play it
				yield return taskInstance.TaskEngine.ProcessTask(new MakeCardFreeThisTurnTask(card.BoxIl2CppObject())).Cast<Il2CppSystem.Object>();
			}
		}
	}
}
