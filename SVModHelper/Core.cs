using MelonLoader;
using Il2CppInterop.Runtime.Injection;
using SVModHelper.ModContent;
using System.Reflection;
using UnityEngine;

[assembly: MelonInfo(typeof(SVModHelper.Core), "StarVaders Mod Helper", "0.2.1", "Slaith", "https://github.com/Slaith12/SVModHelper/releases")]
[assembly: MelonGame("Pengonauts", "StarVaders")]
[assembly: VerifyLoaderVersion(0, 7, 2, true)]
namespace SVModHelper
{
    internal class Core : MelonMod
    {
        public override void OnEarlyInitializeMelon()
        {
            base.OnEarlyInitializeMelon();

            Melon<Core>.Logger.Msg("Loading default sprites");
            Assembly assembly = typeof(AModContent).Assembly;
            //I was originally planning on automatically grabbing the shadow sprite from the game directly,
            //but I'm not sure how to do that so I'm just adding the shadow sprite to the build instead.
            byte[] arr = ResourceHelper.LoadResource(assembly, "SVModHelper.shadow.png");
            if (arr == null)
            {
                Melon<Core>.Logger.Error("Unable to load default shadow image.");
            }
            else
            {
                ModContentManager.contentData.Add("SVModHelper.DefaultShadow.png", arr);
            }

            arr = ResourceHelper.LoadResource(assembly, "SVModHelper.EntityUnknown.png");
            if (arr == null)
            {
                Melon<Core>.Logger.Error("Unable to load default entity image.");
            }
            else
            {
                ModContentManager.contentData.Add("SVModHelper.DefaultEntity.png", arr);
            }
            Melon<Core>.Logger.Msg("Sprites loaded.");
        }

        public override void OnInitializeMelon()
        {
            RegisterTypeOptions enumLinkOptions = new RegisterTypeOptions() { Interfaces = new Il2CppInterfaceCollection([typeof(Il2CppSystem.Collections.IEnumerator)]) };
            ClassInjector.RegisterTypeInIl2Cpp<EnumeratorLink>(enumLinkOptions);

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
            ModContentManager.postInit = true;
        }
    }
}