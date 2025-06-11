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
            if (artifactName >= (ArtifactName)15000 && artifactName < (ArtifactName)(SVModHelper.moddedArtifacts.Count + 15000))
            {
                ArtifactID artifactID = new ArtifactID(__instance._artifactCount);
                __instance._artifactCount++;
                ModArtifactModelDef artifactModelDef = new ModArtifactModelDef(SVModHelper.moddedArtifacts[(int)artifactName - 15000], artifactName);
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
            AModArtifact modArtifact = SVModHelper.GetModArtifactInstance(artifactName);
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
            for (int i = 0; i < SVModHelper.moddedArtifacts.Count; i++)
                __result.Add(artifactFactory.CreateArtifactModel((ArtifactName)(i + 15000)));
        }
    }
}
