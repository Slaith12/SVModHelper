using Il2CppMS.Internal.Xml.XPath;
using MelonLoader;
using SVModHelper.ModContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SVModHelper
{
	[HarmonyPatch(typeof(ContentGetter), nameof(ContentGetter.GetAllPilots))]
	internal static class ModdedGetAllPilots
	{
		public static void Postfix(ref Il2CppCollections.List<PilotName> __result)
		{
			foreach (var pilotName in ModContentManager.moddedPilotDict.Keys)
			{
				__result.Add(pilotName);
			}
		}
	}

	[HarmonyPatch(typeof(PilotDataDictSO), nameof(PilotDataDictSO.GetData))]
	internal static class PilotDataDictFixer
	{
		public static bool Prefix(PilotName entry, PilotDataDictSO __instance, ref PilotDataSO __result)
		{
			if (ModContentManager.moddedPilotDict.ContainsKey(entry))
			{
				__result = ModContentManager.moddedPilotDict[entry].GetPilotData();
	
				return false;
			}
			return true;
		}
	}

	[HarmonyPatch(typeof(PilotView), nameof(PilotView.UpdatePilotPanel))]
	internal static class PilotViewFixer
	{
		public static void Postfix(PilotSelectionController pilotSelectionController, PilotView __instance)
		{
			if (ModContentManager.moddedPilotDict.ContainsKey(pilotSelectionController.CurrentPlayerData.PilotName))
			{
				__instance.PilotName.SetText(ModContentManager.moddedPilotDict[pilotSelectionController.CurrentPlayerData.PilotName].DisplayName);
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
			if (ModContentManager.moddedPilotDict.ContainsKey(pilotName))
			{
				__result = new();
				return false;
			}

			return true;
		}
	}
}
