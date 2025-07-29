namespace SVModHelper.ModContent
{
    [HarmonyPatch(typeof(PilotSelectionController), nameof(PilotSelectionController.GetAllPlayerDataSOs))]
    internal static class ModPilotSelectionController
	{
        public static void Postfix(PilotSelectionController __instance, Il2CppCollections.List<PlayerDataSO> __result)
        {
	        foreach (var kvp in ModContentManager.moddedPilotDict.Where(kvp => kvp.Value.ClassName == __instance.PlayerDataSOs._items.First().ClassName))
	        {
				__result.Add(kvp.Value.GetStarterPlayerData());
            }
        }
    }
}
