using SVModHelper.ModContent;
using Il2CppPixelCrushers.DialogueSystem;
using UnityEngine;

namespace SVModHelper
{
	[HarmonyPatch(typeof(ContentGetter), nameof(ContentGetter.GetAllPilots))]
	internal static class ModdedGetAllPilots
	{
		public static void Postfix(ref Il2CppCollections.List<PilotName> __result)
		{
            foreach (PilotName pilot in ModContentManager.GetAllModPilotNames())
                {
				__result.Add(pilot);
			}
		}
	}

	[HarmonyPatch(typeof(ContentGetter), nameof(ContentGetter.GetClassPilots))]
	internal static class ModdedGetClassPilots
	{
		public static void Postfix(ClassName className, ref Il2CppCollections.List<PilotName> __result)
		{
			foreach (var pilot in ModContentManager.moddedPilots.Where(pilot => pilot.ClassName == className))
				__result.Add(pilot.PilotName);
		}
	}

	//patches the delegate used to set the header text for the pilot group
	[HarmonyPatch(typeof(GlossaryPilotGroup.__c__DisplayClass11_0), nameof(GlossaryPilotGroup.__c__DisplayClass11_0._Initialize_b__0))]
	internal static class GlossaryPilotGroupFixer
	{
		public static void Prefix(GlossaryPilotGroup.__c__DisplayClass11_0 __instance)
		{
			//if the pilot name is a number, it's a modded pilot
			if(int.TryParse(__instance.pilotName, out int num))
			{
				AModPilot modPilot = ModContentManager.GetModPilotInstance((PilotName)num);
				if(modPilot != null)
					__instance.pilotName = modPilot.DisplayName;
			}
		}
	}

	[HarmonyPatch(typeof(PilotDataDictSO), nameof(PilotDataDictSO.GetData))]
	internal static class PilotDataDictFixer
	{
		public static bool Prefix(PilotName entry, PilotDataDictSO __instance, ref PilotDataSO __result)
		{
			if(!DataManager.SettingsData.PilotSkin.TryGetValue(entry, out PilotSkinName skin))
			{
				DataManager.SettingsData.PilotSkin.Add(entry, PilotSkinName.Standard);
				skin = PilotSkinName.Standard;
			}
			PilotDataSO modPilotData = SpriteHelper.GetPilotData(entry, skin, __instance, SpriteHelper.LogLevel.InitialQueryOrFail);
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
                __instance.PilotNameText.SetText(name);
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
					__instance.m_characterInfoCache[id] = new Il2CppPixelCrushers.DialogueSystem.CharacterInfo(id, pilotNamePair.Key.ToString(), null, CharacterType.PC, null);
				}
				__instance.m_characterInfoCache[id].Name = name;
			}
		}
	}

	[HarmonyPatch(typeof(EndingSceneController), nameof(EndingSceneController.SetupPilots))]
	internal static class TrueEndingSceneFixer
	{
		public static void Prefix(EndingSceneController __instance)
		{
			foreach (var pilotDataPair in ModContentManager.moddedPilotDescriptors)
			{
				PilotName pilot = pilotDataPair.Key.Item1;

				int index = __instance._pilotEndingData.FindIndex(new Func<PilotEndingImages, bool>(p => p.PilotName == pilot));
				if (index < 0)
				{
					index = __instance._pilotEndingData.Count;
					__instance._pilotEndingData.Add(new PilotEndingImages());
					__instance._pilotEndingData[index].PilotName = pilot;
					//Modded pilots' ending sprites default to Roxy's sprites if not defined.
					PilotEndingImages refData = __instance._pilotEndingData[0];
					__instance._pilotEndingData[index].HandshakeSprite = refData.HandshakeSprite;
					//All lineup objects have the same positions, the sprites themselves are offset.
					//This could be problematic if more than one modded pilot appears on the lineup screen, but currently that's not possible so I'm not going to worry about it.
					//The lineup image's sprites should always be overwritten to something, defaulting to a transparent image.
					__instance._pilotEndingData[index].LineupImage = UnityEngine.Object.Instantiate(refData.LineupImage, refData.LineupImage.transform.parent);
				}

				Sprite handshakeSprite = ModContentManager.GetModPilotHandshakeSprite(pilot);
				if(handshakeSprite != null)
					__instance._pilotEndingData[index].HandshakeSprite = handshakeSprite;

                Sprite lineupSprite = ModContentManager.GetModPilotLineupSprite(pilot);
                if (lineupSprite!= null)
                    __instance._pilotEndingData[index].LineupImage.sprite = lineupSprite;
			}
		}

		public static void Postfix(EndingSceneController __instance)
        {
            //Since the game uses the "pilot win" achievements to determine who shows up in the lineup, by default modded characters never show.
			//This will make it so the current pilot being played is shown. Modded pilots that you've previously won with still won't be shown.
            if (DataManager.PlayerData.PilotName >= ModContentManager.MINPILOTID)
			{
				__instance._pilotEndingData.Find(new Func<PilotEndingImages, bool>(p => p.PilotName == DataManager.PlayerData.PilotName)).LineupImage.gameObject.SetActive(true);
			}
		}
    }

	[HarmonyPatch(typeof(PhotoSpawnMover), nameof(PhotoSpawnMover.SetupPilots))]
	internal static class CreditsSceneFixer
	{
		public static void Prefix(PhotoSpawnMover __instance)
		{
			foreach (var pilotDataPair in ModContentManager.moddedPilotDescriptors)
			{
				PilotName pilot = pilotDataPair.Key.Item1;

				int index = __instance._pilotEndingData.FindIndex(new Func<PilotEndingImages, bool>(p => p.PilotName == pilot));
				if (index < 0)
				{
					index = __instance._pilotEndingData.Count;
					__instance._pilotEndingData.Add(new PilotEndingImages());
					__instance._pilotEndingData[index].PilotName = pilot;
					var refData = __instance._pilotEndingData[0];
					//All lineup objects have the same positions, the sprites themselves are offset.
					//This could be problematic if more than one modded pilot appears on the lineup screen, which is actually possible here, but I'm still not going to worry about it.
					//The lineup image's sprites should always be overwritten to something, defaulting to a transparent image.
					__instance._pilotEndingData[index].LineupImage = UnityEngine.Object.Instantiate(refData.LineupImage, refData.LineupImage.transform.parent);
                }
                Sprite lineupSprite = ModContentManager.GetModPilotLineupSprite(pilot);
                if (lineupSprite != null)
					__instance._pilotEndingData[index].LineupImage.sprite = lineupSprite;
			}
		}
	}
    }
