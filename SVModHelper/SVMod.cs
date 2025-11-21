using MelonLoader;
using SVModHelper.ModContent;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SVModHelper
{
    public class SVMod : MelonMod
    {
        /// <summary>
        /// <para>Called by the mod helper when your mod is registered, before any mod's RegisterMod() function is called. Should register all resources/data that your content would depend on.</para>
        /// <para>By default, this registers all resources and tasks in your mod's assembly.</para>
        /// <para>If you add custom more info panels, they should be added in this function.</para>
        /// </summary>
        protected internal virtual void EarlyRegisterMod()
        {
            Assembly modAsm = MelonAssembly.Assembly;

            LoggerInstance.Msg("Registering Resources");
            foreach (string fileName in modAsm.GetManifestResourceNames())
            {
                RegisterResource(fileName);
            }
            LoggerInstance.Msg("All Resources Loaded.");

            foreach (Type modTaskDef in modAsm.GetTypes().Where(type => type.IsSubclassOf(typeof(AModTask))))
            {
                try
                {
                    RegisterTask(Activator.CreateInstance(modTaskDef, true) as AModTask);
                }
                catch (Exception ex)
                {
                    LoggerInstance.Error($"The following error occured while registering task {modTaskDef.Name}.\n" + ex);
                }
            }
        }

        /// <summary>
        /// <para>Called by the mod helper when your mod is registered. Should register all content present in your mod.</para>
        /// <para>By default, this registers all AModContent classes in your mod's assembly.</para>
        /// </summary>
        protected internal virtual void RegisterMod()
        {
            Assembly modAsm = MelonAssembly.Assembly;

            LoggerInstance.Msg("Registering Content");
            foreach (Type modCardDef in modAsm.GetTypes().Where(type => type.IsSubclassOf(typeof(AModCard))))
            {
                try
                {
                    RegisterCard(Activator.CreateInstance(modCardDef, true) as AModCard);
                }
                catch (Exception ex)
                {
                    LoggerInstance.Error($"The following error occured while registering card {modCardDef.Name}.\n" + ex);
                }
            }
            foreach (Type modArtifactDef in modAsm.GetTypes().Where(type => type.IsSubclassOf(typeof(AModArtifact))))
            {
                try
                {
                    RegisterArtifact(Activator.CreateInstance(modArtifactDef, true) as AModArtifact);
                }
                catch (Exception ex)
                {
                    LoggerInstance.Error($"The following error occured while registering artifact {modArtifactDef.Name}.\n" + ex);
                }
            }
            foreach (Type modComponentDef in modAsm.GetTypes().Where(type => type.IsSubclassOf(typeof(AModComponent))))
            {
                try
                {
                    RegisterComponent(Activator.CreateInstance(modComponentDef, true) as AModComponent);
                }
                catch (Exception ex)
                {
                    LoggerInstance.Error($"The following error occured while registering component {modComponentDef.Name}.\n" + ex);
                }
            }
            foreach (Type modItemDef in modAsm.GetTypes().Where(type => type.IsSubclassOf(typeof(AModItem))))
            {
                try
                {
                    RegisterItem(Activator.CreateInstance(modItemDef, true) as AModItem);
                }
                catch (Exception ex)
                {
                    LoggerInstance.Error($"The following error occured while registering item {modItemDef.Name}.\n" + ex);
                }
            }
            foreach (Type modPackDef in modAsm.GetTypes().Where(type => type.IsSubclassOf(typeof(AModPack))))
            {
                try
                {
                    RegisterPack(Activator.CreateInstance(modPackDef, true) as AModPack);
                }
                catch (Exception ex)
                {
                    LoggerInstance.Error($"The following error occured while registering pack {modPackDef.Name}.\n" + ex);
                }
            }
            foreach (Type modSpellDef in modAsm.GetTypes().Where(type => type.IsSubclassOf(typeof(AModSpell))))
            {
                try
                {
                    RegisterSpell(Activator.CreateInstance(modSpellDef, true) as AModSpell);
                }
                catch (Exception ex)
                {
                    LoggerInstance.Error($"The following error occured while registering spell {modSpellDef.Name}.\n" + ex);
                }
            }
            foreach (Type modPilot in modAsm.GetTypes().Where(type => type.IsSubclassOf(typeof(AModPilot))))
            {
                try
                {
                    RegisterPilot(Activator.CreateInstance(modPilot) as AModPilot);
                }
                catch (Exception ex)
                {
                    LoggerInstance.Error($"The following error occured while registering pilot {modPilot.Name}.\n" + ex);
                }
            }
        }

        /// <summary>
        /// <para>Called by the mod helper when your mod is registered, after every mod's RegisterMod() function is called. Should perform any actions that require content to be registered beforehand.</para>
        /// <para>Any ContentModifications should be registered here.</para>
        /// <para>By default, does nothing.</para>
        /// </summary>
        protected internal virtual void LateRegisterMod() { }

        protected void RegisterResource(string resourceName)
        {
            Melon<Core>.Logger.Msg("  Loading resource " + resourceName);
            byte[] arr = ResourceHelper.LoadResource(MelonAssembly.Assembly, resourceName);
            ModContentManager.contentData.Add(resourceName, arr);
            Melon<Core>.Logger.Msg("  Resource Loaded");
        }

        #region AModContent
        protected CardName RegisterCard(AModCard modCardDef)
        {
            ModContentManager.CheckInitStatus();
            Melon<Core>.Logger.Msg("Registering card " + modCardDef.GetType().Name);
            Type cardType = modCardDef.GetType();
            if (ModContentManager.moddedCardDict.ContainsKey(cardType))
            {
                throw new InvalidOperationException("Can not register the same card multiple times.");
            }

            CardName id = ModContentManager.moddedCards.Count + ModContentManager.MINCARDID;
            ModContentManager.moddedCards.Add(modCardDef);

            ModContentManager.moddedCardDict.Add(cardType, id);

            ModContentManager.SetCardTitle(id, modCardDef.DisplayName);
            ModContentManager.SetCardDesc(id, modCardDef.Description);
            ModContentManager.SetCardImage(id, modCardDef.CardViewData);
            foreach(var locName in modCardDef.LocalizedNames)
            {
                ModContentManager.SetCardTitle(id, locName.Value, locName.Key);
            }
            foreach (var locDesc in modCardDef.LocalizedDescriptions)
            {
                ModContentManager.SetCardDesc(id, locDesc.Value, locDesc.Key);
            }

            return id;
        }

        protected ArtifactName RegisterArtifact(AModArtifact modArtifactDef)
        {
            ModContentManager.CheckInitStatus();
            Melon<Core>.Logger.Msg("Registering artifact " + modArtifactDef.GetType().Name);
            Type artifactType = modArtifactDef.GetType();
            if (ModContentManager.moddedArtifactDict.ContainsKey(artifactType))
            {
                throw new InvalidOperationException("Can not register the same artifact multiple times.");
            }

            ArtifactName id = ModContentManager.moddedArtifacts.Count + ModContentManager.MINARTIFACTID;

            ModContentManager.moddedArtifacts.Add(modArtifactDef);
            ModContentManager.moddedArtifactDict.Add(artifactType, id);

            ModContentManager.SetArtifactTitle(id, modArtifactDef.DisplayName);
            ModContentManager.SetArtifactDesc(id, modArtifactDef.Description);
            ModContentManager.SetArtifactImage(id, modArtifactDef.Sprite);
            foreach (var locName in modArtifactDef.LocalizedNames)
            {
                ModContentManager.SetArtifactTitle(id, locName.Value, locName.Key);
            }
            foreach (var locDesc in modArtifactDef.LocalizedDescriptions)
            {
                ModContentManager.SetArtifactDesc(id, locDesc.Value, locDesc.Key);
            }

            return id;
        }

        protected ComponentName RegisterComponent(AModComponent modComponentDef)
        {
            ModContentManager.CheckInitStatus();
            Type componentType = modComponentDef.GetType();
            if (ModContentManager.moddedComponentDict.ContainsKey(componentType))
            {
                throw new InvalidOperationException("Can not register the same component multiple times.");
            }

            ComponentName id = ModContentManager.moddedComponents.Count + ModContentManager.MINCOMPID;
            ModContentManager.moddedComponents.Add(modComponentDef);
            ModContentManager.moddedComponentDict.Add(componentType, id);

            ModContentManager.SetComponentTitle(id, modComponentDef.DisplayName);
            ModContentManager.SetComponentDesc(id, modComponentDef.Description);
            ModContentManager.SetComponentImage(id, modComponentDef.Sprite);
            foreach (var locName in modComponentDef.LocalizedNames)
            {
                ModContentManager.SetComponentTitle(id, locName.Value, locName.Key);
            }
            foreach (var locDesc in modComponentDef.LocalizedDescriptions)
            {
                ModContentManager.SetComponentDesc(id, locDesc.Value, locDesc.Key);
            }

            return id;
        }

        protected ItemName RegisterItem(AModItem modItemDef)
        {
            ModContentManager.CheckInitStatus();
            Type itemType = modItemDef.GetType();
            if (ModContentManager.moddedItemDict.ContainsKey(itemType))
            {
                throw new InvalidOperationException("Can not register the same item multiple times.");
            }

            ItemName id = ModContentManager.moddedItems.Count + ModContentManager.MINITEMID;
            ModContentManager.moddedItems.Add(modItemDef);
            ModContentManager.moddedItemDict.Add(itemType, id);

            ModContentManager.SetItemTitle(id, modItemDef.DisplayName);
            ModContentManager.SetItemDesc(id, modItemDef.Description);
            ModContentManager.SetItemImage(id, modItemDef.ItemViewData);
            foreach (var locName in modItemDef.LocalizedNames)
            {
                ModContentManager.SetItemTitle(id, locName.Value, locName.Key);
            }
            foreach (var locDesc in modItemDef.LocalizedDescriptions)
            {
                ModContentManager.SetItemDesc(id, locDesc.Value, locDesc.Key);
            }

            return id;
        }

        protected ItemPackName RegisterPack(AModPack modPackDef)
        {
            ModContentManager.CheckInitStatus();
            Type packType = modPackDef.GetType();
            if (ModContentManager.moddedPackDict.ContainsKey(packType))
            {
                throw new InvalidOperationException("Can not register the same pack multiple times.");
            }

            ItemPackName id = ModContentManager.moddedPacks.Count + ModContentManager.MINPACKID;
            ModContentManager.moddedPacks.Add(modPackDef);
            ModContentManager.moddedPackDict.Add(packType, id);

            ModContentManager.SetPackTitle(id, modPackDef.DisplayName);
            ModContentManager.SetPackDesc(id, modPackDef.Description);
            ModContentManager.SetPackImage(id, modPackDef.Sprite);
            foreach (var locName in modPackDef.LocalizedNames)
            {
                ModContentManager.SetPackTitle(id, locName.Value, locName.Key);
            }
            foreach (var locDesc in modPackDef.LocalizedDescriptions)
            {
                ModContentManager.SetPackDesc(id, locDesc.Value, locDesc.Key);
            }

            return id;
        }

        protected ArtifactName RegisterSpell(AModSpell modSpellDef)
        {
            ModContentManager.CheckInitStatus();
            Melon<Core>.Logger.Msg("Registering spell " + modSpellDef.GetType().Name);
            Type artifactType = modSpellDef.GetType();
            if (ModContentManager.moddedArtifactDict.ContainsKey(artifactType))
            {
                throw new InvalidOperationException("Can not register the same spell multiple times.");
            }

            ArtifactName id = ModContentManager.moddedArtifacts.Count + ModContentManager.MINARTIFACTID;
            ModContentManager.moddedArtifacts.Add(modSpellDef);
            ModContentManager.moddedArtifactDict.Add(artifactType, id);

            ModContentManager.SetArtifactTitle(id, modSpellDef.DisplayName);
            ModContentManager.SetArtifactDesc(id, modSpellDef.Description);
            ModContentManager.SetArtifactImage(id, modSpellDef.Sprite);
            foreach (var locName in modSpellDef.LocalizedNames)
            {
                ModContentManager.SetArtifactTitle(id, locName.Value, locName.Key);
            }
            foreach (var locDesc in modSpellDef.LocalizedDescriptions)
            {
                ModContentManager.SetArtifactDesc(id, locDesc.Value, locDesc.Key);
            }

            return id;
        }

        protected PilotName RegisterPilot(AModPilot modPilot)
        {
            ModContentManager.CheckInitStatus();
            Melon<Core>.Logger.Msg("Registering pilot " + modPilot.GetType().Name);
            Type pilotType = modPilot.GetType();
            if (ModContentManager.moddedPilotDict.ContainsKey(pilotType))
            {
				throw new InvalidOperationException("Can not register the same pilot multiple times.");
            }

            PilotName id = ModContentManager.moddedPilots.Count + ModContentManager.MINPILOTID;
            ModContentManager.moddedPilots.Add(modPilot);
            ModContentManager.moddedPilotDict.Add(pilotType, id);

            ModContentManager.SetPilotName(id, modPilot.DisplayName);
            ModContentManager.SetPilotDesc(id, modPilot.Description);
            ModContentManager.SetPilotViewData(id, PilotSkinName.Standard, modPilot.GetFullPilotData(PilotSkinName.Standard));
            foreach (var locDesc in modPilot.LocalizedDescriptions)
            {
                ModContentManager.SetPilotDesc(id, locDesc.Value, locDesc.Key);
            }
            foreach (var locDialogue in modPilot.LocalizedTrueEndDialogues)
            {
                ModContentManager.SetPilotTrueEndDialogue(id, locDialogue.Value.dialogue1, locDialogue.Value.dialogue2, locale: locDialogue.Key);
            }

            return id;
        }

        protected string RegisterTask(AModTask task)
        {
            ModContentManager.CheckInitStatus();
            Type taskType = task.GetType();
            if (ModContentManager.moddedTaskIDs.ContainsKey(taskType))
            {
                throw new InvalidOperationException("Can not register the same task multiple times.");
            }

            string id = taskType.FullName;
            ModContentManager.moddedTaskIDs.Add(taskType, id);
            ModContentManager.moddedTaskInstances.Add(id, task);
            return id;
        }

        protected MoreInfoWordName RegisterMoreInfoPanel(string id, string defaultDescription, Dictionary<string, string> localizedDescriptions = null, bool overrideIfPresent = true)
        {
            ModContentManager.CheckInitStatus();
            MoreInfoWordName name;
            if(ModContentManager.moddedMoreInfoPanelDict.TryGetValue(id, out name))
            {
                if (!overrideIfPresent)
                    return name;
            }
            else
            {
                name = ModContentManager.moddedMoreInfoPanels.Count + ModContentManager.MINMOREINFOID;
                ModContentManager.moddedMoreInfoPanels.Add(id);
                ModContentManager.moddedMoreInfoPanelDict.Add(id, name);
            }

            ModContentManager.SetMoreInfoDescription(name, defaultDescription);
            if(localizedDescriptions != null)
            {
                foreach((string locale, string desc) in localizedDescriptions)
                {
                    ModContentManager.SetMoreInfoDescription(name, desc, locale);
                }
            }
            return name;
        }
        #endregion

        #region Content Modifications
        protected void RegisterContentMod(CardModification cardMod)
        {
            cardMod.m_Source = this;
            int index;
            for (index = 0; index < ModContentManager.cardModifications.Count && ModContentManager.cardModifications[index].priority < cardMod.priority; index++) ;
            ModContentManager.cardModifications.Insert(index, cardMod);
        }

        protected void RegisterContentMod(ArtifactModification artifactMod)
        {
            artifactMod.m_Source = this;
            int index;
            for (index = 0; index < ModContentManager.artifactModifications.Count && ModContentManager.artifactModifications[index].priority < artifactMod.priority; index++) ;
            ModContentManager.artifactModifications.Insert(index, artifactMod);
        }

        protected void RegisterContentMod(ComponentModification componentMod)
        {
            componentMod.m_Source = this;
            int index;
            for (index = 0; index < ModContentManager.componentModifications.Count && ModContentManager.componentModifications[index].priority < componentMod.priority; index++) ;
            ModContentManager.componentModifications.Insert(index, componentMod);
        }

        protected void RegisterContentMod(ItemModification itemMod)
        {
            itemMod.m_Source = this;
            int index;
            for (index = 0; index < ModContentManager.itemModifications.Count && ModContentManager.itemModifications[index].priority < itemMod.priority; index++) ;
            ModContentManager.itemModifications.Insert(index, itemMod);
        }

        protected void RegisterContentMod(PackModification packMod)
        {
            packMod.m_Source = this;
            int index;
            for (index = 0; index < ModContentManager.packModifications.Count && ModContentManager.packModifications[index].priority < packMod.priority; index++) ;
            ModContentManager.packModifications.Insert(index, packMod);
        }

        protected void RegisterContentMod(PilotModification pilotMod)
        {
            pilotMod.m_Source = this;
            int index;
            for (index = 0; index < ModContentManager.pilotModifications.Count && ModContentManager.pilotModifications[index].priority < pilotMod.priority; index++) ;
            ModContentManager.pilotModifications.Insert(index, pilotMod);
        }
        #endregion

        //TODO: Consolidate content functions here and in AModContent in a separate helper class
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected bool TryGetContentData(string fileName, out byte[] data, bool localName = true, bool warnOnFail = true)
        {
            bool success = ModContentManager.contentData.TryGetValue(GetContentKeyString(fileName, localName), out data);
            if(!success && warnOnFail)
            {
                Melon<Core>.Logger.Error($"Failed to load file - {fileName}. Make sure the file exists, it has been added as an Embedded Resource, and that the path is specified relative to the .csproj file.");
            }
            return success;
        }

        //TODO: Update this function to cache textures for future calls
        protected Texture2D GetTexture(string imageName, FilterMode filter = FilterMode.Bilinear, bool localName = true, bool warnOnFail = true)
        {
            if (!TryGetContentData(imageName, out byte[] data, localName, warnOnFail))
                return null;
            Texture2D texture = new Texture2D(2, 2) { filterMode = filter };
            texture.LoadImage(data);
            return texture;
        }

        //TODO: Update this function to cache sprites for future calls
        protected Sprite GetStandardSprite(string imageName, float pixelsPerUnit = 100, FilterMode filter = FilterMode.Bilinear, bool localName = true, bool warnOnFail = true)
        {
            Texture2D texture = GetTexture(imageName, filter, localName, warnOnFail);
            if (texture == null)
                return null;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
        }

        protected CardViewData GetStandardCardViewData(CardName cardName, string imageName, float pixelsPerUnit = 100, FilterMode filter = FilterMode.Bilinear, bool localName = true, bool warnOnFail = true)
        {
            Sprite sprite = GetStandardSprite(imageName, pixelsPerUnit, filter, localName, warnOnFail);
            if (sprite == null)
                return null;
            return new CardViewData(cardName, sprite, null);
        }

        protected Sprite GetDefaultEntitySprite()
        {
            return GetStandardSprite("SVModHelper.DefaultEntity.png", localName: false);
        }

        protected Sprite GetDefaultShadowSprite()
        {
            return GetStandardSprite("SVModHelper.DefaultShadow.png", localName: false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string GetContentKeyString(string fileName, bool localName = true)
        {
            if (localName)
                return GetType().Assembly.GetName().Name + "." + fileName;
            else
                return fileName;
        }
    }
}
