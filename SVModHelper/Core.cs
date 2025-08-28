using MelonLoader;
using Il2CppInterop.Runtime.Injection;
using SVModHelper.ModContent;
using System.Reflection;

[assembly: MelonInfo(typeof(SVModHelper.Core), "StarVaders Mod Helper", "0.2.0", "Slaith", null)]
[assembly: MelonGame("Pengonauts", "StarVaders")]

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
            foreach (MelonMod mod in RegisteredMelons)
            {
                if (mod is SVMod svMod)
                {
                    Melon<Core>.Logger.Msg("Registering mod " + svMod.Info.Name);
                    try
                    {
                        svMod.RegisterMod();
                    }
                    catch(Exception ex)
                    {
                        Melon<Core>.Logger.Error($"The following error occured when registering {svMod.Info.Name}:\n{ex}");
                    }
                }
            }

            foreach (MelonMod mod in RegisteredMelons)
            {
                if (mod is SVMod svMod)
                {
                    try
                    {
                        svMod.LateRegisterMod();
                    }
                    catch (Exception ex)
                    {
                        Melon<Core>.Logger.Error($"The following error occured when late registering {svMod.Info.Name}:\n{ex}");
                    }
                }
            }
        }

        public override void OnLateInitializeMelon()
        {
            ModContentManager.ApplyMods();
            ModContentManager.postInit = true;
        }
    }
}