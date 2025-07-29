using System.Collections;

namespace SVModHelper.ModContent
{
    public abstract class AModTask
    {
        public string customTaskID => ModContentManager.GetModTaskID(GetType());

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
            if(customTaskID == ModContentManager.INVALIDTASKID)
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
