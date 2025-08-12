using Il2CppSystem.Collections;

namespace SVModHelper.ModContent
{
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
