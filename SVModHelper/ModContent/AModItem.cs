namespace SVModHelper.ModContent
{
    public abstract class AModItem : AModContent
    {
        public ItemName ItemName => ModContentManager.GetModItemName(GetType());

        public abstract string DisplayName { get; }
        public abstract string Description { get; }

        public virtual ItemViewDataSO ItemViewData => new ItemViewDataSO() 
        { 
            Sprite = GetStandardSprite(GetType().Name + ".png", warnOnFail: false) ?? GetDefaultEntitySprite(),
            Shadow = GetDefaultShadowSprite()
        };

        /// <summary>
        /// The definition panels that show up in the more info screen of this item.
        /// </summary>
        public virtual Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords { get; } = new();
        /// <summary>
        /// The cards that show up in the more info screen of this item.
        /// </summary>
        public virtual Il2CppCollections.HashSet<CardName> MoreInfoCards { get; } = new();
        /// <summary>
        /// The items that show up in the more info screen of this item.
        /// </summary>
        public virtual Il2CppCollections.HashSet<ItemName> MoreInfoItems { get; } = new();
        /// <summary>
        /// The enemies that show up in the more info screen of this item.
        /// </summary>
        public virtual Il2CppCollections.HashSet<EnemyName> MoreInfoEnemies { get; } = new();

        /// <summary>
        /// The entity traits this item has that show up on the info display.
        /// </summary>
        public virtual Il2CppCollections.HashSet<EntityTrait> Traits { get; } = new();
        /// <summary>
        /// The entity traits this item has that don't show up on the info display.
        /// </summary>
        public virtual Il2CppCollections.HashSet<EntityTrait> HiddenTraits { get; } = new();

        /// <summary>
        /// The number of hits it takes for the entity to be destroyed.
        /// </summary>
        public virtual int Health { get; } = 1;
        /// <summary>
        /// The number of stars awarded when the entity is destroyed (This value is multiplied by 100)
        /// </summary>
        public virtual int StarbucksReward { get; }
        /// <summary>
        /// If true, the entity spawns shielded.
        /// </summary>
        public virtual bool IsShielded { get; } = false;
        /// <summary>
        /// If true, the entity spawns invulnerable.
        /// </summary>
        public virtual bool IsInvulnerable { get; }
        /// <summary>
        /// If true, the entity can not be moved by any effects.
        /// </summary>
        public virtual bool IsStatic { get; }
        /// <summary>
        /// If true, the entity can not be shielded.
        /// </summary>
        public virtual bool IsNotShieldable { get; }
        /// <summary>
        /// If true, the entity can not be selected by any movement effects.
        /// </summary>
        public virtual bool IsNotMoveSelectable { get; }
        /// <summary>
        /// If true, the entity can not be selected by any swap effects.
        /// </summary>
        public virtual bool IsNotSwapSelectable { get; }
        /// <summary>
        /// If true, the entity can not be selected by any effects that apply a status effect.
        /// </summary>
        public virtual bool IsNotStatusEffectSelectable { get; }

        public virtual bool HasSpecialPrefab => false;

        /// <summary>
        /// <para>If true, the entity will be marked as having a regenerating shield.</para>
        /// <para>NOTE: this does not actually give the entity a regenerating shield, that still has to be applied in <code>GetStartTurnActionTaskList()</code></para>
        /// </summary>
        public virtual bool HasRegeneratingShield { get; }

        /// <summary>
        /// The amount of doom applied when this item channels doom.
        /// </summary>
        public virtual int DoomLevel => 0;

        /// <summary>
        /// Returns the tasks that are previewed when this item is hovered.
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetPreviewTaskList(OnCreateIDValue itemID)
        {
            return new();
        }

        /// <summary>
        /// <para>Returns the trigger effects that are added when this entity is spawned.</para>
        /// <para>NOTE: the trigger effects are not automatically removed when the item is destroyed. You need to set the trigger end conditions so they end once the item is gone.</para>
        /// </summary>
        public virtual Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue itemID)
        {
            return new();
        }

        /// <summary>
        /// <para>Returns the tasks that should be executed when the item is activated.</para>
        /// <para>NOTE: the tasks can be executed even if the item is not charged. Include a condition on the task to make sure the tasks should be executable.</para>
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetOnUseTaskList(OnCreateIDValue itemID)
        {
            return new();
        }

        /// <summary>
        /// Retunrs the tasks that should be executed right before the item is spawned.
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetPreSpawnTaskList(OnCreateIDValue itemID)
        {
            return new();
        }

        /// <summary>
        /// Returns the tasks that should be executed right after the item is spawned.
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetSpawnTaskList(OnCreateIDValue itemID)
        {
            return new();
        }

        /// <summary>
        /// Returns the tasks that should be executed at the start of the player's turn while the item is on the board.
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetStartTurnActionTaskList(OnCreateIDValue itemID)
        {
            return new();
        }
    }
}
