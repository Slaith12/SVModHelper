using Il2CppInterop.Runtime.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVModHelper.ModContent
{
    [MelonLoader.RegisterTypeInIl2Cpp]
    internal class ModComponentModelDef : AComponentDefinition
    {
        public AModComponent component;
        public ComponentName id;

        public ModComponentModelDef(IntPtr ptr) : base(ptr) { }
        public ModComponentModelDef(AModComponent comp, ComponentName id) : this(ClassInjector.DerivedConstructorPointer<ModComponentModelDef>())
        {
            ClassInjector.DerivedConstructorBody(this);
            component = comp;
            this.id = id;
            OnCreateIDValue = new();
        }


        public override ComponentName ComponentName => id;
        public override ClassName Class => component.Class;
        public override Il2CppCollections.HashSet<CardTrait> AddedTraits => component.AddedTraits;
        public override Il2CppCollections.HashSet<CardTrait> AddedHiddenTraits => component.AddedHiddenTraits;
        public override Il2CppCollections.HashSet<ComponentTrait> ComponentTraits => component.ComponentTraits;
        public override ContextPreviewType ContextPreviewType => component.ContextPreviewType;
        public override bool MakeFree => component.MakeFree;
        public override Il2CppCollections.HashSet<CardName> MoreInfoCards => component.MoreInfoCards;
        public override Il2CppCollections.HashSet<EnemyName> MoreInfoEnemies => component.MoreInfoEnemies;
        public override Il2CppCollections.HashSet<ItemName> MoreInfoItems => component.MoreInfoItems;
        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => component.MoreInfoWords;
        public override Il2CppCollections.List<ATask> OnCreateTaskList => component.GetOnCreateTaskList(OnCreateIDValue);
        public override Il2CppCollections.List<ATask> PreSelectionTaskList => component.GetPreSelectionTaskList(OnCreateIDValue);
        public override Il2CppCollections.List<ATask> PostSelectionTaskList => component.GetPostSelectionTaskList(OnCreateIDValue);
        public override Il2CppCollections.List<TriggerEffect> TriggerEffects => component.GetTriggerEffects(OnCreateIDValue);
        public override Rarity Rarity => component.Rarity;
        public override bool SkipNormalDescription => component.SkipNormalDescription;

        //these are all handled when the component is applied to the card
        public override int BaseHeatCostModifier => 0;
        public override int BaseManaCostModifier => 0;
        public override int BasePowerCostModifier => 0;
        public override ACardModelDefinition NewCardDefinition => null;
    }
}
