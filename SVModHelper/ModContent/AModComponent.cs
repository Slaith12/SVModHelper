using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVModHelper.ModContent
{
    public abstract class AModComponent
    {
        public abstract string DisplayName { get; }
        public abstract string Description { get; }

        /// <summary>
        /// The mech class this component is available to. Set to Neutral to allow any class to obtain it.
        /// </summary>
        public abstract ClassName Class { get; }

        /// <summary>
        /// The traits this component has. This determines what cards are able to get this component.
        /// </summary>
        public virtual Il2CppCollections.HashSet<ComponentTrait> ComponentTraits => new();
        /// <summary>
        /// The traits this component adds to modified cards.
        /// </summary>
        public virtual Il2CppCollections.HashSet<CardTrait> AddedTraits => new();
        /// <summary>
        /// The hidden traits this component adds to modified cards.
        /// </summary>
        public virtual Il2CppCollections.HashSet<CardTrait> AddedHiddenTraits => new();

        /// <summary>
        /// The context preview that appears when this component is shown as a reward.
        /// </summary>
        public virtual ContextPreviewType ContextPreviewType => ContextPreviewType.None;

        /// <summary>
        /// The definition panels this component adds to the more info screen of modified cards.
        /// </summary>
        public virtual Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new();
        /// <summary>
        /// The cards this component adds to the more info screen of modified cards.
        /// </summary>
        public virtual Il2CppCollections.HashSet<CardName> MoreInfoCards => new();
        /// <summary>
        /// The items this component adds to the more info screen of modified cards.
        /// </summary>
        public virtual Il2CppCollections.HashSet<ItemName> MoreInfoItems => new();
        /// <summary>
        /// The enemies this component adds to the more info screen of modified cards.
        /// </summary>
        public virtual Il2CppCollections.HashSet<EnemyName> MoreInfoEnemies => new();

        /// <summary>
        /// The amount this component changes the card's cost by.
        /// </summary>
        public virtual int BaseCostModifier => 0;

        /// <summary>
        /// Does this component make the card free?
        /// </summary>
        public virtual bool MakeFree => false;

        /// <summary>
        /// Gets any effects that can trigger outside of when the modified card is played.
        /// </summary>
        public virtual Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue cardID)
        {
            return new();
        }

        /// <summary>
        /// <para>The actions that occur when the component is applied to a card.</para>
        /// <para>This is meant to be used for when the component directly modifies the functionality of the card (similar to fuel bomb or brainiac augment)</para>
        /// <para>This does not use the task engine, and as such it's acceptable for actions to take effect immediately when this function is called.</para>
        /// </summary>
        public virtual void ModifyCardModel(CardModel cardModel)
        {

        }

        /// <summary>
        /// Gets the tasks that are executed when this component is applied to a card during an encounter (including when a card is created with this component).
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetOnCreateTaskList(OnCreateIDValue cardID)
        {
            Il2CppCollections.List<ATask> tasks = new();
            
            foreach(CardTrait trait in AddedTraits)
            {
                tasks.Add(new AddTraitTask(cardID.ToObject(), (int)trait));
            }
            foreach (CardTrait trait in AddedHiddenTraits)
            {
                tasks.Add(new AddTraitTask(cardID.ToObject(), (int)trait, true));
            }
            if(BaseCostModifier != 0)
            {
                tasks.Add(new AddPlayCardModTask(
                    new PlayCardModifierModel(
                        new EqualsCondition(new TargetValue().ToObject(), cardID),
                        ArgKey.BaseEnergyCost, Operation.Add, BaseCostModifier)));
            }
            if(MakeFree)
            {
                tasks.Add(new AddPlayCardModTask(
                    new PlayCardModifierModel(
                        new EqualsCondition(new TargetValue().ToObject(), cardID),
                        ArgKey.IsFree, Operation.Replace, true)));
            }
            return tasks;
        }

        /// <summary>
        /// <para>Gets the tasks that are executed AFTER the modified card has been played.</para>
        /// <para>These tasks are included in the preview even before any selections have been made.</para>
        /// <para>(If a card has multiple STGs, these are only previewed/executed following the final STG.)</para>
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetPreSelectionTaskList(OnCreateIDValue cardID)
        {
            return new();
        }

        /// <summary>
        /// <para>Gets the tasks that are executed AFTER the modified card has been played.</para>
        /// <para>These tasks are included in the preview only after all selections have been made.</para>
        /// <para>These tasks occur immediately after this component's PreSelectionTaskList.</para>
        /// </summary>
        public virtual Il2CppCollections.List<ATask> GetPostSelectionTaskList(OnCreateIDValue cardID)
        {
            return new();
        }

        /// <summary>
        /// If true, the component's description replaces the card's normal description.
        /// </summary>
        public virtual bool SkipNormalDescription => false;

        /// <summary>
        /// The component's rarity. Any rarity other than "Common" will cause the component to be unable to be applied by any effect that applies a random component (including rewards).
        /// </summary>
        public virtual Rarity Rarity => Rarity.Common;
    }
}
