using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVModHelper.ModContent
{
    public abstract class AModTask
    {
        public IDictionary<ArgKey, Il2CppSystem.Object> args = new Dictionary<ArgKey, Il2CppSystem.Object>();

        /// <summary>
        /// Can trigger effects be triggered by this task?
        /// </summary>
        public virtual bool IsTriggerEffectable => true;

        public abstract IEnumerator Execute(ModTaskInstance taskInstance);

        public virtual ModTaskInstance DeepClone(ModTaskInstance thisInstance)
        {
            ModTaskInstance newTask = new ModTaskInstance(this);
            if(thisInstance.Args != null)
            {
                newTask.Args = thisInstance.Args.DeepClone().Cast<CloneableDict<ArgKey, Il2CppSystem.Object>>();
            }
            else
            {
                Melon<Core>.Logger.Warning("Cloned task instance had null args!");
            }
            return newTask;
        }

        public ModTaskInstance Convert()
        {
            return new ModTaskInstance(this);
        }
    }
}
