using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVModHelper.ModContent
{
    [HarmonyPatch(typeof(ArtifactFactory), nameof(ArtifactFactory.CreateArtifactModel))]
    internal static class ModArtifactFactory
    {
        public static bool Prefix(ArtifactName artifactName, ref ArtifactModel __result, ArtifactFactory __instance)
        {
            if (artifactName >= ModContentManager.MINARTIFACTID)
            {
	            MelonLogger.Msg($"Creating Artifact - {artifactName}");

				ArtifactID artifactID = new ArtifactID(__instance._artifactCount);
                __instance._artifactCount++;

                var type = ModContentManager.moddedArtifactDict.FirstOrDefault(pair => pair.Value == artifactName).Key;

                if (type == null)
                {
                    MelonLogger.Error($"ArtifactName {artifactName} not found in modded artifact dictionary.");
                    __result = null;
                    return false;
                }

				var modArtifact = Activator.CreateInstance(type, true) as IHasArtifactID;

                ModArtifactModelDef artifactModelDef = new ModArtifactModelDef(modArtifact, artifactName);
                ArtifactModel artifactModel = new ArtifactModel(artifactModelDef, artifactID);

                artifactModelDef.SetOnCreateID(artifactModel.ID);
                __result = artifactModel;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(PlayerDataSO), nameof(PlayerDataSO.AddArtifact))]
    internal static class OnArtifactObtain
    {
        public static void Postfix(ArtifactName artifactName, PlayerDataSO __instance)
        {
            IHasArtifactID modArtifact = ModContentManager.GetModArtifactInstance(artifactName);
            if(modArtifact != null)
            {
                modArtifact.OnObtain(__instance);
            }
        }
    }

    [HarmonyPatch(typeof(ContentGetter), nameof(ContentGetter.GetAllArtifacts))]
    internal static class ModdedGetAllArtifacts
    {
        public static void Postfix(ref Il2CppCollections.List<ArtifactModel> __result)
        {
            ArtifactFactory artifactFactory = new ArtifactFactory();
            foreach(ArtifactName artifactName in ModContentManager.moddedArtifactDict.Values)
                __result.Add(artifactFactory.CreateArtifactModel(artifactName));
        }
    }
}
