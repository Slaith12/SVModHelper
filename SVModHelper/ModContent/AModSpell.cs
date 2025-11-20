using UnityEngine;

namespace SVModHelper.ModContent
{
    public abstract class AModSpell : AModContent, IHasArtifactID
    {
        public ArtifactName ArtifactName => ModContentManager.GetModArtifactName(GetType());

        /// <summary>
        /// The name that's shown for this spell. Only used when no localization is provided for the current locale.
        /// </summary>
        public abstract string DisplayName { get; }
        /// <summary>
        /// The description that's shown for this spell. Only used when no localization is provided for the current locale.
        /// </summary>
        public abstract string Description { get; }
        /// <summary>
        /// The name that's shown for this spell on different locales. Falls back to DisplayName for any locales that are missing localizations.
        /// </summary>
        public virtual Dictionary<string, string> LocalizedNames => new();
        /// <summary>
        /// The description that's shown for this spell on different locales. Falls back to Description for any locales that are missing localizations.
        /// </summary>
        public virtual Dictionary<string, string> LocalizedDescriptions => new();
        public virtual Sprite Sprite => GetStandardSprite(GetType().Name + ".png", warnOnFail: true);

        /// <summary>
        /// The pilot this spell belongs to. Set to None to allow any Keeper pilot to get it.
        /// </summary>
        public virtual PilotName PilotUnique => PilotName.None;
        public abstract int Cooldown { get; }

        /// <summary>
        /// The type of preview shown when the spell is highlighted in the shop/reward screens.
        /// </summary>
        public virtual ContextPreviewType ContextPreviewType => ContextPreviewType.None;

        /// <summary>
        /// The definition panels that show up in the more info screen of this spell.
        /// </summary>
        public virtual Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new Il2CppCollections.HashSet<MoreInfoWordName>();
        /// <summary>
        /// The cards that show up in the more info screen of this spell.
        /// </summary>
        public virtual Il2CppCollections.HashSet<CardName> MoreInfoCards => new Il2CppCollections.HashSet<CardName>();
        /// <summary>
        /// The items that show up in the more info screen of this spell.
        /// </summary>
        public virtual Il2CppCollections.HashSet<ItemName> MoreInfoItems => new Il2CppCollections.HashSet<ItemName>();
        /// <summary>
        /// The enemies that show up in the more info screen of this spell.
        /// </summary>
        public virtual Il2CppCollections.HashSet<EnemyName> MoreInfoEnemies => new Il2CppCollections.HashSet<EnemyName>();

        /// <summary>
        /// <para>The actions that occur when this spell is initially obtained.</para>
        /// <para>This does not use the task engine, and as such it's acceptable for actions to take effect immediately when this function is called.</para>
        /// </summary>
        public virtual void OnObtain(PlayerDataSO playerData)
        {

        }

        /// <summary>
        /// Returns the tasks to perform when this spell is initialized at the beginning of the encounter OR spawned mid-way through an encounter.
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetSpawnTaskList(OnCreateIDValue artifactID)
        {
            return new();
        }

        /// <summary>
        /// Returns the tasks to perform when this spell is cast.
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetTaskList(OnCreateIDValue artifactID)
        {
            return new();
        }

        /// <summary>
        /// <para>Returns the condition that must be fulfilled for the spell to be castable.</para>
        /// <para>Note: This condition is automatically combined with the condition that the spell is off-cooldown.</para>
        /// </summary>
        public virtual ACondition GetCastCondition(OnCreateIDValue artifactID)
        {
            return new AlwaysTrueCondition();
        }

        /// <summary>
        /// Returns the selections that the player makes when casting this spell.
        /// </summary>
        public virtual Il2CppCollections.List<Selection> GetSpellSelections(OnCreateIDValue artifactID)
        {
            return new();
        }

        /// <summary>
        /// <para>Returns the full task list associated with this spell.</para>
        /// <para>DO NOT OVERRIDE THIS FUNCTION UNLESS YOU KNOW WHAT YOU'RE DOING. The function you should normally override to change the task list is GetTaskList().</para>
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetFullTaskList(OnCreateIDValue artifactID)
        {
            var taskList = GetTaskList(artifactID);
            taskList.Add(new SpellChargeTask(artifactID, Operation.Replace, Cooldown));
            var selections = GetSpellSelections(artifactID);
            if(selections != null && selections.Count > 0)
            {
                taskList = new List<ATask>()
                {
                    new SpellSelectionTask(artifactID, selections, taskList, PreviewTaskList: GetTaskList(artifactID))
                }.ToILCPP();
            }
            return new List<ATask>()
            {
                new ConditionalTask(GetCastCondition(artifactID), taskList)
            }.ToILCPP();
        }

        /// <summary>
        /// <para>Returns any effects that might occur while this spell is in play.</para>
        /// </summary>
        public virtual Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue artifactID)
        {
            return new();
        }

        /// <summary>
        /// The spell's rarity. Any rarity other than "Common" will cause the spell to be unable to be picked by any effect that creates a random spell (including rewards)
        /// </summary>
        public virtual Rarity Rarity => Rarity.Common;

        bool IHasArtifactID.CanBeDuplicated => false;
        ClassName IHasArtifactID.Class => ClassName.Mystic;
        bool IHasArtifactID.IsSpell => true;

        Il2CppCollections.List<ATask> IHasArtifactID.GetTaskList(OnCreateIDValue artifactID) => GetFullTaskList(artifactID);

        public static implicit operator AArtifactModelDefinition(AModSpell modSpell)
        {
            ArtifactName name = modSpell.ArtifactName;
            if (name == ModContentManager.INVALIDARTIFACTID)
            {
                throw new InvalidOperationException($"Attempted to use un-registered spell {modSpell.GetType()}.");
            }
            return new ModArtifactModelDef(modSpell, name);
        }
    }
}
