using System.Text.Json;

namespace SVModHelper
{
    struct IDSaveDict
    {
        public List<string> cardIDs;
    }

    internal static class ModSaveManager
    {
        const string modDataFolder = "ModData";
        const string modIDsFile = "modIDs.json";

        public static void SaveModDataToProfile(int profileIndex)
        {
            string folderPath = Path.Combine(SaveManager.GetSaveProfilePath(new(profileIndex)), modDataFolder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            IDSaveDict ids = ConstructSaveDict();
            SaveIDs(ids, Path.Combine(folderPath, modIDsFile));
        }

        public static void LoadModDataFromProfile(int profileIndex)
        {
            string folderPath = Path.Combine(SaveManager.GetSaveProfilePath(new(profileIndex)), modDataFolder);

            string idsFilePath = Path.Combine(folderPath, modIDsFile);
            if(File.Exists(idsFilePath))
            {
                IDSaveDict ids = LoadIDs(idsFilePath);
                //TODO: check if ids are compatible with existing dictionary
                ApplySaveDict(ids);
            }
        }

        public static void LoadInitialModData()
        {
            ModContentManager.moddedCards = new();
            ModContentManager.moddedCardIDDict = new();

            //Game hasn't loaded yet, so SettingsData isn't initialized yet. Have to get the save index manually.
            string settingsPath = Path.Combine(SaveManager.GetGeneralSavePath(), SaveManager._settingsDataSaveName);
            if (!File.Exists(settingsPath))
                return;
            int profileIndex;
            using(FileStream fileStream = File.OpenRead(settingsPath))
            {
                profileIndex = JsonDocument.Parse(fileStream).RootElement.GetProperty("SaveProfileIndex").GetInt32();
            }

            LoadModDataFromProfile(profileIndex);
        }

        private static void SaveIDs(IDSaveDict ids, string filePath)
        {
            JsonSerializerOptions options = new()
            {
                IncludeFields = true,
                WriteIndented = true
            };

            using(FileStream fileStream = File.Create(filePath))
            {
                JsonSerializer.Serialize(fileStream, ids, options);
            }
        }

        private static IDSaveDict LoadIDs(string filePath)
        {
            JsonSerializerOptions options = new()
            {
                IncludeFields = true,
                AllowTrailingCommas = true
            };

            IDSaveDict ids;
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                ids = JsonSerializer.Deserialize<IDSaveDict>(fileStream, options);
            }
            return ids;
        }

        private static IDSaveDict ConstructSaveDict()
        {
            return new IDSaveDict()
            {
                cardIDs = ModContentManager.moddedCards.Select(card => card.ID).ToList()
            };
        }

        /// <summary>
        /// Applies the ID Dictionary from the file to the current mod state
        /// </summary>
        /// <returns>Returns true if the ids were applied with no conflicts. Returns false if there were conflicts.</returns>
        private static bool ApplySaveDict(IDSaveDict ids)
        {
            for(int i = 0; i < ids.cardIDs.Count; i++)
            {
                if (ModContentManager.moddedCards.Count <= i)
                {
                    //recorded card count > current session's card count (extremely rare outside startup)
                    //current card list should already completely match recorded list up to this point
                    //this means the currently checked card shouldn't be a duplicate
                    ModContentManager.moddedCards.Add(null);
                    ModContentManager.moddedCardIDDict.Add(ids.cardIDs[i], ModContentManager.MINCARDID + i);
                }
                else
                {

                }
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.SaveGame))]
    internal static class SaveInjector
    {
        private static void Postfix()
        {
            ModSaveManager.SaveModDataToProfile(DataManager.SettingsData.SaveProfileIndex);
        }
    }

    [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.LoadGameAtIndex))]
    internal static class LoadInjector
    {
        private static void Postfix()
        {
            ModSaveManager.LoadModDataFromProfile(DataManager.SettingsData.SaveProfileIndex);
        }
    }
}
