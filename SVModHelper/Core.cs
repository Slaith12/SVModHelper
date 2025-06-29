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

            LoggerInstance.Msg("Current possible encounter mods:");
            foreach(ArtifactName name in ContentGetter.GetEncounterDifficultyMods().ToMono())
            {
                LoggerInstance.Msg(name);
            }
            LoggerInstance.Msg("Current possible curse mods:");
            foreach (ArtifactName name in ContentGetter.GetPossibleCurseMods().ToMono())
            {
                LoggerInstance.Msg(name);
            }
        }
    }
}