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
	internal class DropMeatTask : AModTask
	{
		public DropMeatTask()
		{

		}

		public DropMeatTask(AValue coord)
		{
			SetArg(ArgKey.Coord, coord);
		}

		public override IEnumerator Execute(ATask taskInstance)
		{
			Coord coord = taskInstance.GetArg<Coord>(ArgKey.Coord);
			var taskEngine = taskInstance.TaskEngine;
			var gridModel = taskEngine.EncounterModel.GridModel;

			if (gridModel.IsMoveableCoord(coord) && !gridModel.TryGetPickupAtCoord(coord, out var s))
			{
				yield return taskInstance.TaskEngine.ProcessTask(
					new CreateCardTask(17000, Pile:Pile.Pickup, coord: coord.BoxIl2CppObject(), rarity: new Il2CppSystem.Nullable<Rarity>())
				).Cast<Il2CppSystem.Object>();
			}

			yield break;
		}
	}
}
