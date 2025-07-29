namespace SVModHelper
{
    [HarmonyPatch(typeof(ContentGetter), nameof(ContentGetter.GetEncounterDifficultyMods))]
    internal class EncounterModifierFixer
    {
        public static Il2CppCollections.List<ArtifactName> encounterModList;

        public static void SetMods(IEnumerable<ArtifactName> extraModifiers, IEnumerable<ArtifactName> removedModifiers)
        {
            encounterModList = null;
            var newList = ContentGetter.GetEncounterDifficultyMods();
            foreach(ArtifactName artifact in extraModifiers)
            {
                if(!newList.Contains(artifact))
                {
                    newList.Add(artifact);
                }
            }
            foreach(ArtifactName artifact in removedModifiers)
            {
                newList.Remove(artifact);
            }
            encounterModList = newList;
        }

        public static bool Prefix(ref Il2CppCollections.List<ArtifactName> __result)
        {
            if (encounterModList == null)
                return true;
            __result = encounterModList;
            return false;
        }
    }
    [HarmonyPatch(typeof(ContentGetter), nameof(ContentGetter.GetPossibleCurseMods))]
    internal class CurseModifierFixer
    {
        public static Il2CppCollections.List<ArtifactName> curseModList;

        public static void SetMods(IEnumerable<ArtifactName> extraModifiers, IEnumerable<ArtifactName> removedModifiers)
        {
            curseModList = null;
            var newList = ContentGetter.GetPossibleCurseMods();
            foreach (ArtifactName artifact in extraModifiers)
            {
                if (!newList.Contains(artifact))
                {
                    newList.Add(artifact);
                }
            }
            foreach (ArtifactName artifact in removedModifiers)
            {
                newList.Remove(artifact);
            }
            curseModList = newList;
        }

        public static bool Prefix(ref Il2CppCollections.List<ArtifactName> __result)
        {
            if (curseModList == null)
                return true;
            __result = curseModList;
            return false;
        }
    }
}
