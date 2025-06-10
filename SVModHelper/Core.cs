using MelonLoader;

[assembly: MelonInfo(typeof(SVModHelper.Core), "StarVaders Mod Helper", "0.0.1", "Slaith", null)]
[assembly: MelonGame("Pengonauts", "StarVaders")]

namespace SVModHelper
{
    public class Core : MelonMod
    {
        public override void OnInitializeMelon()
        {
            foreach(MelonMod mod in RegisteredMelons)
            {
                SVModHelper.RegisterMod(mod);
            }
        }
    }
}