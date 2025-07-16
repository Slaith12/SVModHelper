using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SVModHelper.ModContent
{
    public abstract class AModArtifact : AModContent, IHasArtifactID
    {
		/// <summary>
		/// If assigned, this artifact will be registered with this ArtifactName instead of the default one. Must be greater or equal to 15000.
		/// This is useful when setting up AModPilot.
		/// </summary>
		public virtual ArtifactName ArtifactNameOverride => ModContentManager.INVALIDARTIFACTID;

        public ArtifactName ArtifactName => ModContentManager.GetModArtifactName(GetType());

        public abstract string DisplayName { get; }
        public abstract string Description { get; }
        public virtual Sprite Sprite => GetStandardSprite(GetType().Name + ".png");

        /// <summary>
        /// The mech class that's able to get this card. Set to Neutral to allow any class to get it. Set to UniquePack to restrict it to a pack.
        /// </summary>
        public abstract ClassName Class { get; }
        /// <summary>
        /// The pilot this card belongs to. Set to None to allow any pilot to get it.
        /// </summary>
        public virtual PilotName PilotUnique => PilotName.None;

        public abstract Rarity Rarity { get; }

        /// <summary>
        /// <para>Can this artifact be duplicated by certain events?</para>
        /// <para>By default, this only affects the "Two Copies of a Common Artifact" special reward.</para>
        /// </summary>
        public virtual bool CanBeDuplicated => false;
        /// <summary>
        /// <para>Can this artifact appear as a temporary modifier for an encounter?</para>
        /// <para>It is recommended to set the rarity to "Modifier" if this is true.</para>
        /// </summary>
        public virtual bool IsEncounterModifier => false;
        /// <summary>
        /// <para>Can this artifact appear as a permanent modifier from a special encounter/strange offer?</para>
        /// <para>It is recommended to set the rarity to "Modifier" if this is true.</para>
        /// </summary>
        public virtual bool IsCurseModifier => false;

        /// <summary>
        /// The type of preview shown when the card is highlighted in the shop/reward screens.
        /// </summary>
        public virtual ContextPreviewType ContextPreviewType => ContextPreviewType.None;

        /// <summary>
        /// The definition panels that show up in the more info screen of this card.
        /// </summary>
        public virtual Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new Il2CppCollections.HashSet<MoreInfoWordName>();
        /// <summary>
        /// The cards that show up in the more info screen of this card.
        /// </summary>
        public virtual Il2CppCollections.HashSet<CardName> MoreInfoCards => new Il2CppCollections.HashSet<CardName>();
        /// <summary>
        /// The items that show up in the more info screen of this card.
        /// </summary>
        public virtual Il2CppCollections.HashSet<ItemName> MoreInfoItems => new Il2CppCollections.HashSet<ItemName>();
        /// <summary>
        /// The enemies that show up in the more info screen of this card.
        /// </summary>
        public virtual Il2CppCollections.HashSet<EnemyName> MoreInfoEnemies => new Il2CppCollections.HashSet<EnemyName>();

        /// <summary>
        /// <para>The actions when this artifact is initially obtained.</para>
        /// <para>This does not use the task engine, and as such it's acceptable for actions to take effect immediately when this function is called.</para>
        /// </summary>
        public virtual void OnObtain(PlayerDataSO playerData)
        {

        }

        /// <summary>
        /// Returns the tasks to perform when this artifact is initialized at the beginning of the encounter.
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetSpawnTaskList(OnCreateIDValue artifactID)
        {
            return new();
        }

        /// <summary>
        /// Returns the tasks to perform when this artifact is processed.
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetTaskList(OnCreateIDValue artifactID)
        {
            return new();
        }

        /// <summary>
        /// <para>Returns any effects that might occur while this artifact is in play.</para>
        /// <para>It's recommended that every effect's task list is set to <code>ProcessArtifactTask(artifactID)</code> so that the game can properly indicate that this artifact is being triggered.</para>
        /// </summary>
        public virtual Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue artifactID)
        {
            return new();
        }

        int IHasArtifactID.Cooldown => 1;
        bool IHasArtifactID.IsSpell => false;
    }
}
