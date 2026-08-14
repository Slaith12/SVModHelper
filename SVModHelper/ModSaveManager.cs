using MelonLoader;
using System.Text;
using System.Text.Json;
using UnityEngine;

namespace SVModHelper
{
    struct IDSaveDict
    {
        public List<string> cardIDs;
        public List<string> artifactIDs;
        public List<string> componentIDs;
        public List<string> packIDs;
        public List<string> pilotIDs;
        public List<string> itemIDs;

        public void CreateMissingLists()
        {
            cardIDs ??= new();
            artifactIDs ??= new();
            componentIDs ??= new();
            packIDs ??= new();
            pilotIDs ??= new();
            itemIDs ??= new();
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            str.AppendLine("---Cards---");
            foreach (string id in cardIDs)
                str.AppendLine(id);

            str.AppendLine("---Artifacts---");
            foreach (string id in artifactIDs)
                str.AppendLine(id);

            str.AppendLine("---Components---");
            foreach (string id in componentIDs)
                str.AppendLine(id);

            str.AppendLine("---Packs---");
            foreach (string id in packIDs)
                str.AppendLine(id);

            str.AppendLine("---Pilot---");
            foreach (string id in pilotIDs)
                str.AppendLine(id);

            str.AppendLine("---Items---");
            foreach (string id in itemIDs)
                str.AppendLine(id);

            return str.ToString();
        }
    }

    internal static class ModSaveManager
    {
        public static bool allowModDataSave = false; //set to true after initialization
        const string modDataFolder = "ModData";
        const string modIDsFile = "modIDs.json";

        public static void SaveModDataToProfile(int profileIndex)
        {
            Melon<Core>.Logger.Msg($"Saving mod data to profile {profileIndex}.");
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
            Melon<Core>.Logger.Msg($"Loading mod data from profile {profileIndex}.");
            //SaveManager.GetSaveProfilePath() doesn't seem to work during startup, so it's implemented manually here.
            string folderPath = Path.Combine(
                Application.persistentDataPath, 
                SaveManager.GetGeneralSavePath(), 
                SaveManager._profileFolderPrefix + "_" + profileIndex, 
                modDataFolder);

            string idsFilePath = Path.Combine(folderPath, modIDsFile);
            if(File.Exists(idsFilePath))
            {
                IDSaveDict ids = LoadIDs(idsFilePath);
                //Melon<Core>.Logger.Msg($"IDs from save file:\n{ids}");
                bool compatible = ApplySaveDict(ids);
                if(!compatible)
                {
                    Melon<Core>.Logger.Msg("Closing game due to incompatible mod data. Reopening the game should correct the mod data.");
                    allowModDataSave = false;
                    Application.Quit();
                }
            }
            else
            {
                Melon<Core>.Logger.Warning($"Mod data not found.");
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
            //Melon<Core>.Logger.Msg($"Saving IDs to {filePath}.");
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
            //Melon<Core>.Logger.Msg($"Loading IDs from {filePath}.");
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
            ids.CreateMissingLists();
            return ids;
        }

        private static IDSaveDict ConstructSaveDict()
        {
            return new IDSaveDict()
            {
                cardIDs = ModContentManager.moddedCards.Select(content => content.ID).ToList(),
                artifactIDs = ModContentManager.moddedArtifacts.Select(content => content.ID).ToList(),
                componentIDs = ModContentManager.moddedComponents.Select(content => content.ID).ToList(),
                packIDs = ModContentManager.moddedPacks.Select(content => content.ID).ToList(),
                pilotIDs = ModContentManager.moddedPilots.Select(content => content.ID).ToList(),
                itemIDs = ModContentManager.moddedItems.Select(content => content.ID).ToList(),
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
                    if (!ModContentManager.moddedCardIDDict.TryGetValue(ids.cardIDs[i], out CardName value) ||
                        value != ModContentManager.MINCARDID + i)
                    {
                        Melon<Core>.Logger.Warning($"Conflict found with modded card {i} [{ids.cardIDs[i]}].");
                        return false;
                    }
                }
            }
            for (int i = 0; i < ids.artifactIDs.Count; i++)
            {
                if (ModContentManager.moddedArtifacts.Count <= i)
                {
                    ModContentManager.moddedArtifacts.Add(null);
                    ModContentManager.moddedArtifactIDDict.Add(ids.artifactIDs[i], ModContentManager.MINARTIFACTID + i);
                }
                else
                {
                    if (!ModContentManager.moddedArtifactIDDict.TryGetValue(ids.artifactIDs[i], out ArtifactName value) ||
                        value != ModContentManager.MINARTIFACTID + i)
                    {
                        Melon<Core>.Logger.Warning($"Conflict found with modded artifact {i} [{ids.artifactIDs[i]}].");
                        return false;
                    }
                }
            }
            for (int i = 0; i < ids.componentIDs.Count; i++)
            {
                if (ModContentManager.moddedComponents.Count <= i)
                {
                    ModContentManager.moddedComponents.Add(null);
                    ModContentManager.moddedComponentIDDict.Add(ids.componentIDs[i], ModContentManager.MINCOMPID + i);
                }
                else
                {
                    if (!ModContentManager.moddedComponentIDDict.TryGetValue(ids.componentIDs[i], out ComponentName value) ||
                        value != ModContentManager.MINCOMPID + i)
                    {
                        Melon<Core>.Logger.Warning($"Conflict found with modded component {i} [{ids.componentIDs[i]}].");
                        return false;
                    }
                }
            }
            for (int i = 0; i < ids.packIDs.Count; i++)
            {
                if (ModContentManager.moddedPacks.Count <= i)
                {
                    ModContentManager.moddedPacks.Add(null);
                    ModContentManager.moddedPackIDDict.Add(ids.packIDs[i], ModContentManager.MINPACKID + i);
                }
                else
                {
                    if (!ModContentManager.moddedPackIDDict.TryGetValue(ids.packIDs[i], out ItemPackName value) ||
                        value != ModContentManager.MINPACKID + i)
                    {
                        Melon<Core>.Logger.Warning($"Conflict found with modded pack {i} [{ids.packIDs[i]}].");
                        return false;
                    }
                }
            }
            for (int i = 0; i < ids.pilotIDs.Count; i++)
            {
                if (ModContentManager.moddedPilots.Count <= i)
                {
                    ModContentManager.moddedPilots.Add(null);
                    ModContentManager.moddedPilotIDDict.Add(ids.pilotIDs[i], ModContentManager.MINPILOTID + i);
                }
                else
                {
                    if (!ModContentManager.moddedPilotIDDict.TryGetValue(ids.pilotIDs[i], out PilotName value) ||
                        value != ModContentManager.MINPILOTID + i)
                    {
                        Melon<Core>.Logger.Warning($"Conflict found with modded pilot {i} [{ids.pilotIDs[i]}].");
                        return false;
                    }
                }
            }
            for (int i = 0; i < ids.itemIDs.Count; i++)
            {
                if (ModContentManager.moddedItems.Count <= i)
                {
                    ModContentManager.moddedItems.Add(null);
                    ModContentManager.moddedItemIDDict.Add(ids.itemIDs[i], ModContentManager.MINITEMID + i);
                }
                else
                {
                    if (!ModContentManager.moddedItemIDDict.TryGetValue(ids.itemIDs[i], out ItemName value) ||
                        value != ModContentManager.MINITEMID + i)
                    {
                        Melon<Core>.Logger.Warning($"Conflict found with modded item {i} [{ids.itemIDs[i]}].");
                        return false;
                    }
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
            if(ModSaveManager.allowModDataSave)
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
