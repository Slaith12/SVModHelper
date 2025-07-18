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
			if (entry >= ModContentManager.MINPILOTID)
			{
				__result = ModContentManager.moddedPilotDict[entry].GetPilotData();
	
				return false;
			}
			return true;
		}
	}
}
