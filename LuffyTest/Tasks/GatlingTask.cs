using Il2Cpp;
using Il2CppSystem.Linq;
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
	internal class GatlingTask : AModTask
	{
		public GatlingTask()
		{

		}

		public GatlingTask(AValue coord)
		{
			SetArg(ArgKey.Coord, coord);
		}

		public override IEnumerator Execute(ATask taskInstance)
		{
			Coord playerCoord = taskInstance.GetArg<Coord>(ArgKey.Coord);
			var taskEngine = taskInstance.TaskEngine;
			var gridModel = taskEngine.EncounterModel.GridModel;

			if (taskInstance.IsPreviewModeView)
			{
				foreach (Coord c in gridModel.GetAllCoords().ToMono().Where(c => Coord.ManhattanDistance(c, playerCoord) <= 3))
				{
					taskInstance.GridView.GetTileView(c).EnableTargetTile();
				}
				yield break;
			}

			var taskList = new List<ATask> { };

			taskList.Add(new PlayAnimationTask(new PlayerIDValue(), AnimKey.Stinger_Slash));
			taskList.Add(new StartGridFXAtCoordTask(new PlayerCoordValue(), (int)GridFX.Cleave));

			// Get random coords in a radius of 3 around the player
			List<Coord> coordList = gridModel.GetAllCoords().ToMono()
				.Where(coord => Coord.ManhattanDistance(playerCoord, coord) <= 3 && coord != playerCoord)
				.ToList();

			var randCoords = coordList.ToILCPPEnumerable().PickRandom(7, random: taskEngine.EncounterModel.Random).ToList();

			foreach (var c in randCoords)
			{
				taskList.Add(new StrikeTileEffectTask(c.BoxIl2CppObject(), type:GridFX.MeleeStrike));
				taskList.Add(new WaitForSecondsTask(0.03f));
			}

			yield return taskInstance.TaskEngine.ProcessTask(new TaskQueueTask(taskList.ToILCPP(), relativeCoord: null)).Cast<Il2CppSystem.Object>();

			yield break;
		}
	}
}
