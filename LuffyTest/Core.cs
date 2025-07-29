global using Il2CppStarVaders;
global using Il2CppCollections = Il2CppSystem.Collections.Generic;
using MelonLoader;
using SVModHelper;
using SVModHelper.ModContent;
using System;

[assembly: MelonInfo(typeof(LuffyTest.Core), "Luffy - Example Pilot Mod", "1.0.0", "CrabDogg", null)]
[assembly: MelonGame("Pengonauts", "StarVaders")]

namespace LuffyTest
{
	public class Core : SVMod
	{
		public override void OnInitializeMelon()
		{
		}
	}
}