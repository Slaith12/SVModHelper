using UnityEngine;

namespace SVModHelper.ModContent
{
    public interface IHasArtifactID
    {
        public ArtifactName ArtifactName { get; }
        public string ID { get; }
        public string DisplayName { get; }
        public string Description { get; }
        public Dictionary<string, string> LocalizedNames { get; }
        public Dictionary<string, string> LocalizedDescriptions { get; }
        public Sprite Sprite { get; }

        public bool CanBeDuplicated { get; }
        public ClassName Class { get; }
        public ContextPreviewType ContextPreviewType { get; }
        public int Cooldown { get; }
        public bool IsSpell { get; }
        public Il2CppCollections.HashSet<CardName> MoreInfoCards { get; }
        public Il2CppCollections.HashSet<EnemyName> MoreInfoEnemies { get; }
        public Il2CppCollections.HashSet<ItemName> MoreInfoItems { get; }
        public Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords { get; }
        public PilotName PilotUnique { get; }
        public Rarity Rarity { get; }

        public void OnObtain(PlayerDataSO playerData);
        public Il2CppCollections.List<ATask> GetSpawnTaskList(OnCreateIDValue artifactID);
        public Il2CppCollections.List<ATask> GetTaskList(OnCreateIDValue artifactID);
        public Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue artifactID);
    }
}
