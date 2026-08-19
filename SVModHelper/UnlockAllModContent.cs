namespace SVModHelper
{
    [HarmonyPatch(typeof(LoggerDataSO), nameof(LoggerDataSO.UnlockEverything))]
    internal static class UnlockAllModContent
    {
        private static void Postfix(LoggerDataSO __instance)
        {
            __instance.DiscoveredCards.UnionWith(ModContentManager.GetAllModCardNames().ToILCPPEnumerable());
            __instance.DiscoveredArtifacts.UnionWith(ModContentManager.GetAllModArtifactNames().ToILCPPEnumerable());
        }
    }
}
