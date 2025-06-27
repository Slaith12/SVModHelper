using Il2CppSystem.Collections;

namespace SVModHelper.ModContent
{
    //[HarmonyPatch(typeof(CustomTaskLookup), nameof(CustomTaskLookup.ExecuteCustomTask))]
    //internal static class ModTaskExecute
    //{
    //    public static void Postfix(string customTaskID, ATask customTask, ref IEnumerator __result)
    //    {
    //        AModTask taskDef = SVModHelper.GetModTaskInstance(customTaskID);
    //        if(taskDef == null)
    //        {
    //            throw new KeyNotFoundException($"Unable to find modded task {customTaskID}.");
    //        }
    //        __result = taskDef.Execute(customTask).ToILCPP();
    //    }
    //}

    [HarmonyPatch(typeof(CustomTask), nameof(CustomTask.Execute))]
    internal static class ModTaskExecute
    {
        public static void Postfix(CustomTask __instance, ref IEnumerator __result)
        {
            AModTask taskDef = ModContentManager.GetModTaskInstance(__instance.CustomTaskID);
            if (taskDef == null)
            {
                throw new KeyNotFoundException($"Unable to find modded task {__instance.CustomTaskID}.");
            }
            __result = taskDef.Execute(__instance).ToILCPP();
        }
    }
}
