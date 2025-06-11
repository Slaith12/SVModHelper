using Il2CppInterop.Runtime.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVModHelper;

namespace SVModHelper.ModContent
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    public sealed class ModTaskInstance : ATask
    {
        public AModTask task;

        public override bool IsTriggerEffectable => task.IsTriggerEffectable;

        public ModTaskInstance(IntPtr ptr) : base(ptr) 
        {
            if (Args == null)
                Args = new();
        }
        public ModTaskInstance(AModTask task) : this(ClassInjector.DerivedConstructorPointer<ModTaskInstance>())
        {
            ClassInjector.DerivedConstructorBody(this);
            this.task = task;
            if (task.args == null)
                Args = new CloneableDict<ArgKey, Il2CppSystem.Object>();
            else
                Args = new CloneableDict<ArgKey, Il2CppSystem.Object>(task.args.ToILCPP());
        }

        public override Il2CppSystem.Collections.IEnumerator Execute()
        {
            return task.Execute(this).ToILCPP();
        }

        public override Il2CppSystem.Object DeepClone()
        {
            return task.DeepClone(this).Cast<Il2CppSystem.Object>();
        }
    }
}
