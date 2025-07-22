using HarmonyLib;
using MelonLoader;
using SVModHelper;
using SVModHelper.ModContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Il2CppSystem.Runtime.Remoting.RemotingServices;

namespace LuffyTest
{
	[HarmonyPatch(typeof(DifficultySelectionController), nameof(DifficultySelectionController.OnStartPressed))]
	internal static class RunStartPatch 
	{
		public static void Postfix()
		{
		}
	}
}
