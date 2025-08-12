using SVModHelper.ModContent;

namespace SVModHelper
{
	[HarmonyPatch(typeof(ContentGetter), nameof(ContentGetter.GetAllPilots))]
	internal static class ModdedGetAllPilots
	{
		public static void Postfix(ref Il2CppCollections.List<PilotName> __result)
		{
			for (int i = 0; i < ModContentManager.moddedPilots.Count; i++)
			{
				__result.Add(i + ModContentManager.MINPILOTID);
			}
		}
	}

	[HarmonyPatch(typeof(PilotDataDictSO), nameof(PilotDataDictSO.GetData))]
	internal static class PilotDataDictFixer
	{
		public static bool Prefix(PilotName entry, PilotDataDictSO __instance, ref PilotDataSO __result)
		{
			PilotDataSO modPilotData = ModContentManager.GetModPilotData(entry);
			if (modPilotData == null)
				return true;

			__result = modPilotData;
			return false;
		}
    }

    [HarmonyPatch(typeof(PilotSelectionController), nameof(PilotSelectionController.Initialize))]
    internal static class PilotSelectionFixer
    {
        public static void Postfix(PilotSelectionController __instance)
        {
            ClassName mechClass = __instance.PlayerDataSOs._items.First().ClassName;
            foreach (var pilot in ModContentManager.moddedPilots.Where(pilot => pilot.ClassName == mechClass))
            {
                __instance.PlayerDataSOs.Add(pilot.GetStarterPlayerData());
            }
        }
    }

    [HarmonyPatch(typeof(PilotView), nameof(PilotView.UpdatePilotPanel))]
	internal static class PilotViewFixer
	{
		public static void Postfix(PilotSelectionController pilotSelectionController, PilotView __instance)
		{
			AModPilot modPilot = ModContentManager.GetModPilotInstance(pilotSelectionController.CurrentPlayerData.PilotName);
			if(modPilot != null)
			{
				__instance.PilotName.SetText(modPilot.DisplayName);
			}
		}
	}

	[HarmonyPatch(typeof(PresetChallengesDictSO), nameof(PresetChallengesDictSO.GetPilotChallenges))]
	internal static class PresetChallengesFixer
	{
		// If it's a modded pilot, return an empty list of specific challenges.
		// In the future, we can allow for custom challenges to be added for modded pilots.
		public static bool Prefix(PilotName pilotName, ref Il2CppCollections.List<ChallengeSO> __result)
		{
			AModPilot modPilot = ModContentManager.GetModPilotInstance(pilotName);
			if (modPilot == null)
				return true;

			__result = new();
			return false;
		}
    }
}
