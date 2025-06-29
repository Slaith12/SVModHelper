using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVModHelper.ModContent
{
    public interface IHasArtifactID
    {
        public ArtifactName ArtifactName => ModContentManager.GetModArtifactName(GetType());

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
