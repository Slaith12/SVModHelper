using UnityEngine.Localization.Settings;

namespace SVModHelper
{
    [HarmonyPatch(typeof(LocalizationUtils), nameof(LocalizationUtils.ResetLocalizedStringDict))]
    internal static class NameFixer
    {
        public static Dictionary<string, string> extraLocalizedStrings = new();

        public static void Postfix()
        {
            foreach(var loc in extraLocalizedStrings)
            {
                LocalizationUtils.LocalizedStringsDict.Add(loc.Key, loc.Value);
            }
        }
    }
}
