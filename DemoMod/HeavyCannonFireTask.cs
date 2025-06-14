using SVModHelper;
using SVModHelper.ModContent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod
{
    internal class HeavyCannonFireTask : AModTask
    {
        public HeavyCannonFireTask(Coord? targetCoord = null, Direction direction = Direction.N)
        {
            if(targetCoord != null)
                SetArg(ArgKey.Coord, targetCoord.Value.BoxIl2CppObject());
            SetArg(ArgKey.Direction, (int)direction);
        }

        public override IEnumerator Execute(ModTaskInstance taskInstance)
        {
            if (!taskInstance.TryGetArg(ArgKey.Coord, out Coord targetCoord))
                yield break;
            EntityID primaryTarget = taskInstance.GridModel.GetEntityAtCoord(targetCoord);
            if (primaryTarget == null)
                yield break;

            Stack<ATask> pushTasks = new Stack<ATask>();
            Direction direction = taskInstance.GetArg<Direction>(ArgKey.Direction);
            int intDirection = (int)direction;
            foreach (Coord tileCoord in taskInstance.GridModel.GetCoordsInDirection(targetCoord, direction, inclusive: true))
            {
                if (taskInstance.GridModel.IsCoordEmpty(tileCoord))
                    continue;
                pushTasks.Push(new PushTileEffectTask(tileCoord.BoxIl2CppObject(), intDirection, 10000, parentTask: taskInstance));
            }
            yield return taskInstance.TaskEngine.ProcessTaskList(pushTasks.ToList().ToILCPP());
        }
    }
}
