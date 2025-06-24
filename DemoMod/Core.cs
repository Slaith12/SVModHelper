global using Il2CppCollections = Il2CppSystem.Collections.Generic;
global using Il2CppStarVaders;

using MelonLoader;
using SVModHelper;

[assembly: MelonInfo(typeof(DemoMod.Core), "Demo Mod", "1.0.0", "Slaith", null)]
[assembly: MelonGame("Pengonauts", "StarVaders")]

namespace DemoMod
{
    internal class Core : SVMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }
    }
}