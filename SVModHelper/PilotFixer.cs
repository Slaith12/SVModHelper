using MelonLoader;
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
			foreach (var pilotName in ModContentManager.moddedPilotDict.Values)
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
			if (entry >= ModContentManager.MINPILOTID)
	        {
		        __result = __instance.pilotDataList.ToArray().First(); // Default to Roxy for now.
		        return false;
			}
			return true;
        }
    }

 //   [HarmonyPatch(typeof(PilotView), nameof(PilotView.SetPilotPanelInfo))]
 //   internal static class PilotViewFixer
 //   {
	//    public static bool Prefix(PilotSelectionController pilotSelectionController, PilotView __instance)
	//    {
	//	    PlayerDataSO currentPlayerData = pilotSelectionController.CurrentPlayerData;

	//		// Removing the checks for TotalWins in LoggerData until we can patch it in for modded pilots.
	//		if (currentPlayerData.PilotName >= ModContentManager.MINPILOTID)
	//	    {
	//		    __instance.LockedPanelCanvasGroup.alpha = 0f;
	//		    __instance.PilotPanelCanvasGroup.alpha = 1f;
	//		    __instance.PilotPanelCanvasGroup.blocksRaycasts = true;

	//		    PilotDataSO pilotData = __instance._pilotDataDict.GetData(currentPlayerData.PilotName);

	//		    // Update Images
	//		    __instance.CampaignPortraitImage.sprite = pilotData.CampaignPortrait;
	//			__instance.PilotTitleImage.sprite = pilotData.PilotTitleSprite;

	//			// Update Artifact
	//			__instance.UpdateArtifactView(pilotSelectionController);

	//			//Set Pip Text
	//			__instance.PipText.SetText("" + pilotSelectionController.GetCurrentPilotIndex() + "/" + pilotSelectionController.GetPilotCount());

	//		    // Update pilot complexity
	//		    LocalizationUtils.LocalizeMisc("Complexity", (result) => __instance.SetComplexityText(result, pilotData.Complexity));

	//		    // Update pilot description
	//		    LocalizationUtils.LocalizeMisc("PilotDescription" + currentPlayerData.PilotName.ToString(), UpdateDescriptionText);

	//			return false;
	//	    }
	//	    return true;
	//    }
	//}
}
