using MelonLoader;
using Il2CppInterop.Runtime.Injection;

[assembly: MelonInfo(typeof(SVModHelper.Core), "StarVaders Mod Helper", "0.0.1", "Slaith", null)]
[assembly: MelonGame("Pengonauts", "StarVaders")]

namespace SVModHelper
{
    internal class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            RegisterTypeOptions enumLinkOptions = new RegisterTypeOptions() { Interfaces = new Il2CppInterfaceCollection([typeof(Il2CppSystem.Collections.IEnumerator)]) };
            ClassInjector.RegisterTypeInIl2Cpp<EnumeratorLink>(enumLinkOptions);
            foreach (MelonMod mod in RegisteredMelons)
            {
                if(mod is SVMod svMod)
                    ModContentManager.RegisterMod(svMod);
            }
        }
    }
}