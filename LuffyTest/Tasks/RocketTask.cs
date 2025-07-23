#nullable enable

using Il2CppInterop.Runtime.Runtime;
using Il2CppLanguage.Lua;
using SVModHelper;
using SVModHelper.ModContent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static MelonLoader.MelonLogger;

namespace LuffyTest	
{
	[Serializable]
	public class RocketTask : AModTask
	{
		public RocketTask()
		{

		}

		// Using a targetCoord instead of Direction because DirectionSelections aren't working at the moment.
		public RocketTask(Il2CppSystem.Object Coord, Il2CppSystem.Object targetCoord)
		{
			SetArg(ArgKey.Coord, Coord);
			SetArg(ArgKey.Coord2, targetCoord);
		}

		public override IEnumerator Execute(ATask taskInstance)
		{
			Coord coord = taskInstance.GetArg<Coord>(ArgKey.Coord);
			Coord targetCoord = taskInstance.GetArg<Coord>(ArgKey.Coord2);

			var direction = Coord.GetDirectionTowards(coord, targetCoord);

			var GridModel = taskInstance.TaskEngine.EncounterModel.GridModel;
			var EncounterModel = taskInstance.TaskEngine.EncounterModel;

			var taskList = new List<ATask> {
				new PushTileEffectTask(coord.BoxIl2CppObject(), (int) direction, 9, parentTask: null)
			};

			// Get collision coord
			EntityID entityID = GridModel.GetEntityAtCoord(coord);

			AEntityModel entityModel = EncounterModel.GetModelItem<AEntityModel>(entityID.Cast<ID>());
			if (entityModel.IsStatic) yield break;

			List<Coord> coordsInDirection = GridModel.GetCoordsInDirection(coord, direction, 9).ToMono();

			List<Coord> nonEmptyCoordInDirection = coordsInDirection.Where(c => !GridModel.IsCoordEmpty(c) || GridModel.GetTileStatusAtCoord(c) == TileStatus.TrueHole).ToList();
			if (nonEmptyCoordInDirection.Count > 0)
			{
				var collidedCoord = nonEmptyCoordInDirection.First();

				if (GridModel.TryGetEntityAtCoord(collidedCoord) is EntityID collidedEntity)
				{
					taskList.Add(new PushTileEffectTask(collidedCoord.BoxIl2CppObject(), (int)direction, 4, parentTask: null));
				}

			}


			yield return taskInstance.TaskEngine.ProcessTask(new TaskQueueTask(taskList.ToILCPP(), relativeCoord: null)).Cast<Il2CppSystem.Object>();
		}
	}

}