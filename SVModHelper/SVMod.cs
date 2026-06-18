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
        /// If true, the mod helper will not save content IDs for future sessions.
        /// This should be used if the mod fails to properly load all its content during startup.
        /// </summary>
        protected internal bool blockModSaves = false;

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
                    blockModSaves = true;
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
                    blockModSaves = true;
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
                    blockModSaves = true;
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
                    blockModSaves = true;
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
                    blockModSaves = true;
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
                    blockModSaves = true;
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
                    blockModSaves = true;
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
            return ModContentManager.RegisterCard(modCardDef, this);
        }

        protected ArtifactName RegisterArtifact(AModArtifact modArtifactDef)
        {
            return ModContentManager.RegisterArtifact(modArtifactDef, this);
        }

        protected ComponentName RegisterComponent(AModComponent modComponentDef)
        {
            return ModContentManager.RegisterComponent(modComponentDef, this);
        }

        protected ItemName RegisterItem(AModItem modItemDef)
        {
            return ModContentManager.RegisterItem(modItemDef, this);
        }

        protected ItemPackName RegisterPack(AModPack modPackDef)
        {
            return ModContentManager.RegisterPack(modPackDef, this);
        }

        protected ArtifactName RegisterSpell(AModSpell modSpellDef)
        {
            return ModContentManager.RegisterArtifact(modSpellDef, this);
        }

        protected PilotName RegisterPilot(AModPilot modPilot)
        {
            return ModContentManager.RegisterPilot(modPilot, this);
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
        protected Texture2D oldGetTexture(string imageName, FilterMode filter = FilterMode.Bilinear, bool localName = true, bool warnOnFail = true)
        {
            if (!TryGetContentData(imageName, out byte[] data, localName, warnOnFail))
                return null;
            Texture2D texture = new Texture2D(2, 2) { filterMode = filter };
            texture.LoadImage(data);
            return texture;
        }

        //TODO: Update this function to cache sprites for future calls
        protected Sprite oldGetStandardSprite(string imageName, float pixelsPerUnit = 100, FilterMode filter = FilterMode.Bilinear, bool localName = true, bool warnOnFail = true)
        {
            Texture2D texture = oldGetTexture(imageName, filter, localName, warnOnFail);
            if (texture == null)
                return null;
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
        }

        protected SpriteDescriptor GetStandardSprite(string imageName, float pixelsPerUnit = 100,
            FilterMode filter = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp,
            Rect? rect = null, Vector2? pivot = null,
            bool localName = true)
        {
            return new SpriteDescriptor(GetContentKeyString(imageName, localName), filter, wrapMode, rect, pivot, pixelsPerUnit);
        }

        protected CardViewData GetStandardCardViewData(CardName cardName, string imageName, float pixelsPerUnit = 100, FilterMode filter = FilterMode.Bilinear, bool localName = true, bool warnOnFail = true)
        {
            Sprite sprite = oldGetStandardSprite(imageName, pixelsPerUnit, filter, localName, warnOnFail);
            if (sprite == null)
                return null;
            return new CardViewData(cardName, sprite, null);
        }

        protected Sprite GetDefaultEntitySprite()
        {
            return oldGetStandardSprite("SVModHelper.DefaultEntity.png", localName: false);
        }

        protected Sprite GetDefaultShadowSprite()
        {
            return oldGetStandardSprite("SVModHelper.DefaultShadow.png", localName: false);
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
