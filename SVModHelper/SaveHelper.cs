using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;

namespace SVModHelper
{
    internal static class SaveHelper
    {
        static void Finalizer(Exception __exception)
        {
            if (__exception == null)
                return;

            Melon<Core>.Logger.Warning("Error occured in SaveManager.LoadObject() [likely while loading EncounterData]. Attempting to preserve rest of run data.");
            //Harmony wraps finalizers in their own try/catches so I don't need one here.
            DataManager.RunData = SaveManager.LoadFileToScriptableObjectWithJsonUtility<RunDataSO>(SaveManager.GetSaveProfilePath(), SaveManager._runDataSaveName);
            DataManager.RunData.ChallengeData = SaveManager.LoadSOWithJsonConvert<ChallengeSO>(SaveManager.GetSaveProfilePath(), SaveManager._challengeDataSaveName);
        }
    }
}
