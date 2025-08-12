namespace SVModHelper.ModContent
{
    [HarmonyPatch(typeof(ArtifactFactory), nameof(ArtifactFactory.CreateArtifactModel))]
    internal static class ModArtifactFactory
    {
        public static bool Prefix(ArtifactName artifactName, ref ArtifactModel __result, ArtifactFactory __instance)
        {
            IHasArtifactID modArtifact = ModContentManager.GetModArtifactInstance(artifactName);

            if (modArtifact == null)
                return true;

            ArtifactID artifactID = new ArtifactID(__instance._artifactCount);
            __instance._artifactCount++;
            ModArtifactModelDef artifactModelDef = new ModArtifactModelDef(modArtifact, artifactName);
            ArtifactModel artifactModel = new ArtifactModel(artifactModelDef, artifactID);

            artifactModelDef.SetOnCreateID(artifactModel.ID);
            __result = artifactModel;
            return false;
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
            for (int i = 0; i < ModContentManager.moddedArtifacts.Count; i++)
                __result.Add(artifactFactory.CreateArtifactModel(i + ModContentManager.MINARTIFACTID));
        }
    }
}
