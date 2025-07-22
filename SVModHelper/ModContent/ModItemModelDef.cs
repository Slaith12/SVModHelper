using Il2CppInterop.Runtime.Injection;

namespace SVModHelper.ModContent
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    internal class ModItemModelDef : AItemEntityDefinition
    {
        public AModItem item;
        public ItemName id;

        public ModItemModelDef(IntPtr ptr) : base(ptr) { }
        public ModItemModelDef(AModItem item, ItemName id) : this(ClassInjector.DerivedConstructorPointer<ModItemModelDef>())
        {
            ClassInjector.DerivedConstructorBody(this);
            this.item = item;
            this.id = id;
            OnCreateIDValue = new();
        }

        public override ItemName ItemName => id;
        public override int DoomLevel => item.DoomLevel;
        public override bool HasRegeneratingShield => item.HasRegeneratingShield;
        public override bool HasSpecialPrefab => item.HasSpecialPrefab;
        public override int Health => item.Health;
        public override Il2CppCollections.HashSet<EntityTrait> HiddenTraits => item.HiddenTraits;
        public override bool IsInvulnerable => item.IsInvulnerable;
        public override bool IsNotMoveSelectable => item.IsNotMoveSelectable;
        public override bool IsNotShieldable => item.IsNotShieldable;
        public override bool IsNotStatusEffectSelectable => item.IsNotStatusEffectSelectable;
        public override bool IsNotSwapSelectable => item.IsNotSwapSelectable;
        public override bool IsShielded => item.IsShielded;
        public override bool IsStatic => item.IsStatic;
        public override Il2CppCollections.HashSet<CardName> MoreInfoCards => item.MoreInfoCards;
        public override Il2CppCollections.HashSet<EnemyName> MoreInfoEnemies => item.MoreInfoEnemies;
        public override Il2CppCollections.HashSet<ItemName> MoreInfoItems => item.MoreInfoItems;
        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => item.MoreInfoWords;
        public override int StarbucksReward => item.StarbucksReward;
        public override Il2CppCollections.HashSet<EntityTrait> Traits => item.Traits;

        public override Il2CppCollections.List<ATask> OnUseTaskList => item.GetOnUseTaskList(OnCreateIDValue);
        public override Il2CppCollections.List<ATask> PreSpawnTaskList => item.GetPreSpawnTaskList(OnCreateIDValue);
        public override Il2CppCollections.List<ATask> PreviewTaskList => item.GetPreviewTaskList(OnCreateIDValue);
        public override Il2CppCollections.List<ATask> SpawnTaskList => item.GetSpawnTaskList(OnCreateIDValue);
        public override Il2CppCollections.List<ATask> StartTurnActionTaskList => item.GetStartTurnActionTaskList(OnCreateIDValue);
        public override Il2CppCollections.List<TriggerEffect> TriggerEffects => item.GetTriggerEffects(OnCreateIDValue);
    }
}
