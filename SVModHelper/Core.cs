using MelonLoader;

[assembly: MelonInfo(typeof(SVModHelper.Core), "SVModHelper", "1.0.0", "Slaith", null)]
[assembly: MelonGame("Pengonauts", "StarVaders")]
[assembly: HarmonyDontPatchAll]

namespace SVModHelper
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Initialized.");
        }
    }
}