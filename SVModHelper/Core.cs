using MelonLoader;
using Il2CppInterop.Runtime.Injection;
using SVModHelper.ModContent;
using System.Reflection;
using UnityEngine;

[assembly: MelonInfo(typeof(SVModHelper.Core), "StarVaders Mod Helper", "0.2.3", "Slaith", "https://github.com/Slaith12/SVModHelper/releases")]
[assembly: MelonGame("Pengonauts", "StarVaders")]
//for some reason this fails on the nightly 0.7.2 version
//[assembly: VerifyLoaderVersion(0, 7, 2, true)]
namespace SVModHelper
{
    internal class Core : MelonMod
    {
        public override void OnEarlyInitializeMelon()
        {
            ModSaveManager.allowModDataSave = false;
            base.OnEarlyInitializeMelon();

            Melon<Core>.Logger.Msg("Loading default sprites");
            SpriteHelper.InitDefaultSprites();
            SpriteHelper.ResetSpriteCaches();
            Melon<Core>.Logger.Msg("Sprites loaded.");

        }

        public override void OnInitializeMelon()
        {
            RegisterTypeOptions enumLinkOptions = new RegisterTypeOptions() { Interfaces = new Il2CppInterfaceCollection([typeof(Il2CppSystem.Collections.IEnumerator)]) };
            ClassInjector.RegisterTypeInIl2Cpp<EnumeratorLink>(enumLinkOptions);

            Melon<Core>.Logger.Msg("Loading initial mod data.");
            ModSaveManager.LoadInitialModData();
            Melon<Core>.Logger.Msg("Mod data loaded.");

            List<SVMod> mods = RegisteredMelons.Where(mod => mod is SVMod).Cast<SVMod>().ToList();
            bool error = false;

            foreach (SVMod mod in mods)
            {
                try
                {
                    mod.EarlyRegisterMod();
                }
                catch (Exception ex)
                {
                    Melon<Core>.Logger.Error($"The following error occured when early registering {mod.Info.Name}:\n{ex}");
                    error = true;
                }
            }
            if (error)
            {
                Melon<Core>.Logger.Warning("Closing game due to mods failing to load.");
                Melon<Core>.Logger.Warning("Please update or remove erroneous mods before restarting.");
                Application.Quit();
            }

            foreach (SVMod mod in mods)
            {
                try
                {
                    mod.RegisterMod();
                }
                catch (Exception ex)
                {
                    Melon<Core>.Logger.Error($"The following error occured when registering {mod.Info.Name}:\n{ex}");
                    error = true;
                }
            }
            if (error)
            {
                Melon<Core>.Logger.Warning("Closing game due to mods failing to load.");
                Melon<Core>.Logger.Warning("Please update or remove erroneous mods before restarting.");
                Application.Quit();
            }

            foreach (SVMod mod in mods)
            {
                try
                {
                    mod.LateRegisterMod();
                }
                catch (Exception ex)
                {
                    Melon<Core>.Logger.Error($"The following error occured when late registering {mod.Info.Name}:\n{ex}");
                    error = true;
                }
            }
            if (error)
            {
                Melon<Core>.Logger.Warning("Closing game due to mods failing to load.");
                Melon<Core>.Logger.Warning("Please update or remove erroneous mods before restarting.");
                Application.Quit();
            }
        }

        public override void OnLateInitializeMelon()
        {
            ModContentManager.ApplyMods();
            ModContentManager.FillMissingContent();
            ModContentManager.postInit = true;
            ModSaveManager.allowModDataSave = true;
            ModContentManager.PrintModCardList();
        }
    }
}