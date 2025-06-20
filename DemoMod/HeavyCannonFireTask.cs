using MelonLoader;
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
        //meant to only be used with FireBulletTask, which automatically sets the Coord and Direction args.
        public HeavyCannonFireTask()
        {

        }

        public HeavyCannonFireTask(Coord targetCoord, Direction direction = Direction.N)
        {
            SetArg(ArgKey.RelativeCoord, targetCoord.BoxIl2CppObject());
            SetArg(ArgKey.Direction, (int)direction);
        }

        public override IEnumerator Execute(ATask taskInstance)
        {
            Melon<Core>.Logger.Msg("Executing Heavy Cannon Fire Task");
            //This task is used as a child task of FireBulletTask, which is supposed to set the RelativeCoord and Direction args for this
            //However, the RelativeCoord arg is only set if the child task is labelled as a tile effect task
            //CustomTask is not a tile effect task, so the relative coord is never set. So this task can't function properly.
            if (!taskInstance.TryGetArg(ArgKey.RelativeCoord, out Coord targetCoord) || !taskInstance.TryGetArg(ArgKey.Direction, out Direction direction))
                yield break;

            Melon<Core>.Logger.Msg("Finding target");
            EntityID primaryTarget = taskInstance.GridModel.GetEntityAtCoord(targetCoord);
            if (primaryTarget == null)
                yield break;

            Melon<Core>.Logger.Msg("Target found");
            Stack<ATask> pushTasks = new Stack<ATask>();
            int intDirection = (int)direction;
            foreach (Coord tileCoord in taskInstance.GridModel.GetCoordsInDirection(targetCoord, direction, inclusive: true))
            {
                Melon<Core>.Logger.Msg($"Checking coord ({tileCoord.x}, {tileCoord.y}).");
                if (taskInstance.GridModel.IsCoordEmpty(tileCoord))
                    continue;
                Melon<Core>.Logger.Msg($"Found entity! Creating push task.");
                pushTasks.Push(new PushTileEffectTask(tileCoord.BoxIl2CppObject(), intDirection, 10000, parentTask: taskInstance));
            }
            Melon<Core>.Logger.Msg("Executing push tasks.");
            yield return taskInstance.TaskEngine.ProcessTaskList(pushTasks.ToList().ToILCPP());
        }
    }
}
