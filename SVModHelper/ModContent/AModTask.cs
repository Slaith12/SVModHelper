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
        public string customTaskID => SVModHelper.GetModTaskID(GetType());

        internal IDictionary<ArgKey, Il2CppSystem.Object> args = new Dictionary<ArgKey, Il2CppSystem.Object>();

        /// <summary>
        /// Can trigger effects be triggered by this task?
        /// </summary>
        public virtual bool IsTriggerEffectable => true;

        public abstract IEnumerator Execute(ATask taskInstance);

        //public virtual CustomTask DeepClone(CustomTask thisInstance)
        //{
        //    CustomTask newTask = new CustomTask(this);
        //    if(thisInstance.Args != null)
        //    {
        //        newTask.Args = thisInstance.Args.DeepClone().Cast<CloneableDict<ArgKey, Il2CppSystem.Object>>();
        //    }
        //    else
        //    {
        //        Melon<Core>.Logger.Warning("Cloned task instance had null args!");
        //    }
        //    return newTask;
        //}

        public CustomTask Convert()
        {
            if(customTaskID == SVModHelper.INVALIDTASKID)
            {
                throw new InvalidOperationException($"Attempted to use un-registered task {GetType()}.");
            }
            return new CustomTask(customTaskID, args.ToILCPP());
        }

        protected void SetArg(ArgKey key, Il2CppSystem.Object value)
        {
            args[key] = value;
        }
    }
}
