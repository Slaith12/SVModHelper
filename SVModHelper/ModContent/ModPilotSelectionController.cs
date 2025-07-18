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
	        foreach (var kvp in ModContentManager.moddedPilotDict.Where(kvp => kvp.Value.ClassName == __instance.PlayerDataSOs._items.First().ClassName))
	        {
		        PlayerDataSO playerDataSO = ScriptableObject.CreateInstance<PlayerDataSO>();

		        playerDataSO.starbucksAmount = 75;

		        playerDataSO.ClassName = kvp.Value.ClassName;
		        playerDataSO.PilotName = kvp.Key;

				playerDataSO.startingMaxHeat = 0;


				if (playerDataSO.ClassName == ClassName.Gunner)
				{
					playerDataSO.ClassBaseEnergy = EncounterValue.Heat;
					playerDataSO.startingMaxHeat = 3;
				}
				else if (playerDataSO.ClassName == ClassName.Melee)
				{
					playerDataSO.ClassBaseEnergy = EncounterValue.Power;
					playerDataSO.startingMaxPower = 3;
					playerDataSO.startingPowerCell = 2;
				}
				else if (playerDataSO.ClassName == ClassName.Mystic)
				{
					playerDataSO.ClassBaseEnergy = EncounterValue.Mana;
					playerDataSO.startingMaxMana = 5;
				}

		        foreach (var card in kvp.Value.StartingCards)
				{
					PlayerCardData cardData = new PlayerCardData(card);
					playerDataSO.AddCardToDeck(cardData);
				}

		        foreach (var arti in kvp.Value.StartingArtifacts)
		        {
					playerDataSO.AddArtifact(arti);
		        }

				__result.Add(playerDataSO);
            }
        }
    }
}
