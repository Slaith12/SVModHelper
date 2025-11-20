using MelonLoader;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace SVModHelper
{
    internal static class LocalizationFixer
    {
        public static Dictionary<string, Dictionary<string, string>> extraLocalizedStrings = new();
        public const string GLOBALDEFAULT = "_global";

        public static string GetLocalizedString(string key, string locale)
        {
            if (extraLocalizedStrings.TryGetValue(locale, out var localeDict) && localeDict.TryGetValue(key, out string value))
                return value;
            else
                return null;
        }

        public static void SetLocalizedString(string key, string value, string locale)
        {
            if (!extraLocalizedStrings.TryGetValue(locale, out var localeDict))
            {
                localeDict = new();
                extraLocalizedStrings.Add(locale, localeDict);
            }
            extraLocalizedStrings[locale][key] = value;
        }

        public static bool RemoveLocalizedString(string key, string locale)
        {
            if (!extraLocalizedStrings.TryGetValue(locale, out var localeDict))
                return false;
            bool removed = localeDict.Remove(key);
            return removed;
        }

        internal static void PatchExtraLocalization()
        {
            //load defaults first
            if (extraLocalizedStrings.ContainsKey(GLOBALDEFAULT))
            {
                foreach (var loc in extraLocalizedStrings[GLOBALDEFAULT])
                {
                    LocalizationUtils.LocalizedStringsDict[loc.Key] = loc.Value;
                }
            }

            //add locale-specific strings next (override default strings if necessary)
            string currentLocale = LocalizationSettings.SelectedLocale.Identifier.Code;
            if (extraLocalizedStrings.ContainsKey(currentLocale))
            {
                foreach (var loc in extraLocalizedStrings[currentLocale])
                {
                    LocalizationUtils.LocalizedStringsDict[loc.Key] = loc.Value;
                }
            }
        }
    }

    [HarmonyPatch(typeof(BootupController), nameof(BootupController.SetLocalizer))]
    internal static class BootupLocalizationFixer
    {
        private static void Postfix()
        {
            LocalizationFixer.PatchExtraLocalization();
        }
    }

    [HarmonyPatch(typeof(LanguageSelector), nameof(LanguageSelector.LocaleSelected))]
    internal static class LocalizationUpdateFixer
    {
        private static void Postfix()
        {
            LocalizationFixer.PatchExtraLocalization();
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
