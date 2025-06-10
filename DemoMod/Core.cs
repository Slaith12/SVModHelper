using MelonLoader;

[assembly: MelonInfo(typeof(DemoMod.Core), "Demo Mod", "1.0.0", "Slaith", null)]
[assembly: MelonGame("Pengonauts", "StarVaders")]

namespace DemoMod
{
    internal class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }
    }
}