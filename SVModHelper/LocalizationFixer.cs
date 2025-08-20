using MelonLoader;

namespace SVModHelper
{
    [HarmonyPatch(typeof(LocalizationUtils), nameof(LocalizationUtils.ResetLocalizedStringDict))]
    internal static class LocalizationFixer
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

    [HarmonyPatch(typeof(LocalizationUtils), nameof(LocalizationUtils.LocalizeSynchronous))]
    internal static class ConversationLocalizationFixer
    {
        public static void Postfix(string key, ref string __result)
        {
            if (!__result.StartsWith("No translation for"))
                return;
            string start = key.Substring(0, key.LastIndexOf('_') + 1);
            string end = key.Substring(key.LastIndexOf('_') + 1);
            if(start.StartsWith("Conversation_") && int.TryParse(end, out var num) && num >= (int)ModContentManager.MINPILOTID) //if the key ends with a number and not a name (indicates it's checking for a modded pilot)
            {
                //use generic dialogue as a default
                __result = LocalizationUtils.LocalizeSynchronous(start + "Pilot");
                LocalizationUtils.LocalizedStringsDict.Add(key, __result);
            }
        }
    }
}
