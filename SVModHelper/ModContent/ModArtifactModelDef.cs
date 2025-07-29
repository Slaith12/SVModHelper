﻿using Il2CppInterop.Runtime.Injection;

namespace SVModHelper.ModContent
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    internal class ModArtifactModelDef : AArtifactModelDefinition
    {
        public IHasArtifactID artifactDef;
        public ArtifactName artifactDefID;

        public ModArtifactModelDef(IntPtr ptr) : base(ptr) { }
        public ModArtifactModelDef(IHasArtifactID definition, ArtifactName id) : this(ClassInjector.DerivedConstructorPointer<ModArtifactModelDef>())
        {
            ClassInjector.DerivedConstructorBody(this);
            OnCreateIDValue = new OnCreateIDValue();
            artifactDef = definition;
            artifactDefID = id;
        }

        public override ArtifactName ArtifactName => artifactDefID;
        public override bool CanBeDuplicated => artifactDef.CanBeDuplicated;
        public override ClassName Class => artifactDef.Class;
        public override ContextPreviewType ContextPreviewType => artifactDef.ContextPreviewType;
        public override int Cooldown => artifactDef.Cooldown;
        public override bool IsSpell => artifactDef.IsSpell;
        public override Il2CppCollections.HashSet<CardName> MoreInfoCards => artifactDef.MoreInfoCards;
        public override Il2CppCollections.HashSet<EnemyName> MoreInfoEnemies => artifactDef.MoreInfoEnemies;
        public override Il2CppCollections.HashSet<ItemName> MoreInfoItems => artifactDef.MoreInfoItems;
        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => artifactDef.MoreInfoWords;
        public override PilotName PilotUnique => artifactDef.PilotUnique;
        public override Rarity Rarity => artifactDef.Rarity;
        public override Il2CppCollections.List<ATask> SpawnTaskList => artifactDef.GetSpawnTaskList(OnCreateIDValue);
        public override Il2CppCollections.List<ATask> TaskList => artifactDef.GetTaskList(OnCreateIDValue);
        public override Il2CppCollections.List<TriggerEffect> TriggerEffects => artifactDef.GetTriggerEffects(OnCreateIDValue);
    }
}
