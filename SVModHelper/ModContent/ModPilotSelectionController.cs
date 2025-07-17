using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SVModHelper.ModContent
{
    [HarmonyPatch(typeof(PilotSelectionController), nameof(PilotSelectionController.GetAllPlayerDataSOs))]
    internal static class ModPilotSelectionController
	{
        public static void Postfix(PilotSelectionController __instance, Il2CppCollections.List<PlayerDataSO> __result)
        {
	        foreach (AModPilot modPilot in ModContentManager.moddedPilotDict.Keys.Where(modPilot => modPilot.ClassName == __instance.PlayerDataSOs._items.First().ClassName))
	        {
		        PlayerDataSO playerDataSO = ScriptableObject.CreateInstance<PlayerDataSO>();

		        playerDataSO.starbucksAmount = 75;

		        playerDataSO.ClassName = modPilot.ClassName;
		        playerDataSO.PilotName = ModContentManager.moddedPilotDict[modPilot];

		        foreach (var card in modPilot.StartingCards)
		        {
			        PlayerCardData cardData = new PlayerCardData(card);
					playerDataSO.AddCardToDeck(cardData);
		        }

		        foreach (var arti in modPilot.StartingArtifacts)
		        {
					playerDataSO.AddArtifact(arti);
		        }

				__result.Add(playerDataSO);
            }
        }
    }
}
