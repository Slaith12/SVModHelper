using SVModHelper.ModContent;
using Il2CppPixelCrushers.DialogueSystem;

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
			if(ModContentManager.moddedPilotNames.TryGetValue(pilotSelectionController.CurrentPlayerData.PilotName, out string name))
            {
                __instance.PilotName.SetText(name);
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

	[HarmonyPatch(typeof(DialogueDatabase), nameof(DialogueDatabase.SetupCaches))]
	internal static class DialogueDatabaseFixer
	{
		public static void Postfix(DialogueDatabase __instance)
		{
			if (ModContentManager.moddedPilots.Count == 0 || __instance.actorNameCache.ContainsKey(ModContentManager.MINPILOTID.ToString()))
				return;
			for(int i = 0; i < ModContentManager.moddedPilots.Count; i++)
			{
				Actor newActor = new Actor()
				{
					fields = new(),
					Name = (i + ModContentManager.MINPILOTID).ToString(),
					id = i + (int)ModContentManager.MINPILOTID,
					IsPlayer = true
				};
                __instance.actorNameCache[newActor.Name] = newActor;
				if (__instance.GetActor(newActor.id) == null)
					__instance.actors.Add(newActor);
			}
		}
	}

	[HarmonyPatch(typeof(ConversationModel), nameof(ConversationModel.SetParticipants))]
	internal static class DialogueCharacterInfoFixer
	{
		public static void Postfix(ConversationModel __instance)
		{
			foreach(var pilotNamePair in ModContentManager.moddedPilotNames)
			{
				int id = (int)pilotNamePair.Key;
				string name = pilotNamePair.Value;
				if(!__instance.m_characterInfoCache.ContainsKey(id))
				{
					__instance.m_characterInfoCache[id] = new CharacterInfo(id, pilotNamePair.Key.ToString(), null, CharacterType.PC, null);
				}
				__instance.m_characterInfoCache[id].Name = name;
			}
		}
	}
}
