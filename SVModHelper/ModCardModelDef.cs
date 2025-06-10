using Il2CppInterop.Runtime.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVModHelper
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    internal class ModCardModelDef : ACardModelDefinition
    {
        public ModCard cardDef;
        public CardName cardDefID;

        public ModCardModelDef(IntPtr ptr) : base(ptr) { }
        public ModCardModelDef(ModCard definition, CardName id) : this(ClassInjector.DerivedConstructorPointer<ModCardModelDef>())
        {
            ClassInjector.DerivedConstructorBody(this);
            OnCreateIDValue = new OnCreateIDValue();
            cardDef = definition;
            cardDefID = id;
        }

        public override CardName CardName => cardDefID;
        public override Il2CppCollections.HashSet<CardTrait> Traits => cardDef.Traits;
        public override Il2CppCollections.HashSet<CardTrait> HiddenTraits => cardDef.HiddenTraits;
        public override Rarity Rarity => cardDef.Rarity;
        public override ClassName Class => cardDef.Class;
        public override PilotName PilotUnique => cardDef.PilotUnique;

        public override Il2CppCollections.HashSet<ComponentName> AllowedComponentNames => cardDef.AllowedComponentNames;
        public override Il2CppCollections.HashSet<ComponentTrait> AllowedComponentTraits => cardDef.AllowedComponentTraits;
        public override Il2CppCollections.HashSet<ComponentName> BlockedComponentNames => cardDef.BlockedComponentNames;
        public override Il2CppCollections.HashSet<ComponentTrait> BlockedComponentTraits => cardDef.BlockedComponentTraits;

        public override int ClassBaseCost => cardDef.ClassBaseCost;
        public override bool IsPowerCellOnly => cardDef.IsPowerCellOnly;
        public override bool RequiresPlayerEntity => cardDef.RequiresPlayerEntity;

        public override ContextPreviewType ContextPreviewType => cardDef.ContextPreviewType;
        public override Il2CppCollections.HashSet<CardName> MoreInfoCards => cardDef.MoreInfoCards;
        public override Il2CppCollections.HashSet<EnemyName> MoreInfoEnemies => cardDef.MoreInfoEnemies;
        public override Il2CppCollections.HashSet<ItemName> MoreInfoItems => cardDef.MoreInfoItems;
        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => cardDef.MoreInfoWords;

        public override bool IsShowable => cardDef.IsShowable;
        public override bool IsToken => cardDef.IsToken;
        public override bool IsPurged => cardDef.IsPurged;
        public override bool IsFree => cardDef.IsFree;
        public override int RepeatAmount => cardDef.RepeatAmount;
        public override Pile Destination => cardDef.Destination;

        public override Il2CppCollections.List<TriggerEffect> TriggerEffects => cardDef.GetTriggerEffects(OnCreateIDValue);
        public override Il2CppCollections.List<ATask> OnCreateTaskList => cardDef.GetOnCreateTaskList(OnCreateIDValue);
        public override Il2CppCollections.List<SelectionTaskGroup> SelectionTaskGroups => cardDef.GetSelectionTaskGroups(OnCreateIDValue);
        public override ACondition PlayCondition => cardDef.PlayCondition;

        public override bool HasSpecialCardModel => cardDef.HasSpecialCardModel;
        public override bool HasSpecialCardViewPrefab => cardDef.HasSpecialCardViewPrefab;
    }
}
