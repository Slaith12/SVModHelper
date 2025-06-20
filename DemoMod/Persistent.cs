using SVModHelper;
using SVModHelper.ModContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod
{
    public class Persistent : AModComponent
    {
        public override string DisplayName => "Persistent";

        public override string Description => "If unplayed, create a copy of this card at the top of your draw pile. Purge this card when played.";

        public override ClassName Class => ClassName.Neutral;

        public override Il2CppCollections.HashSet<ComponentTrait> ComponentTraits => new HashSet<ComponentTrait>() 
        { ComponentTrait.Basic, ComponentTrait.NonPurge }.ToILCPP();

        public override Il2CppCollections.HashSet<CardTrait> AddedTraits => new HashSet<CardTrait>() 
        { CardTrait.Tactic }.ToILCPP();

        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new HashSet<MoreInfoWordName>() 
        { MoreInfoWordName.Copy, MoreInfoWordName.Purge }.ToILCPP();

        public override Il2CppCollections.List<ATask> GetOnCreateTaskList(OnCreateIDValue cardID)
        {
            Il2CppCollections.List<ATask> tasks = base.GetOnCreateTaskList(cardID);
            tasks.Add(new AddPlayCardModTask(new PlayCardModifierModel(new EqualsCondition(new TargetValue().ToObject(), cardID.ToObject()), ArgKey.IsPurged, Operation.Replace, true)));
            return tasks;
        }

        public override Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue cardID)
        {
            var startConditions = new List<Il2CppSystem.ValueTuple<Trigger, ACondition>>() { new(Trigger.PreTask, new AndCondition(
                new IsTypeCondition<EndTurnTask>(new RunningTaskValue()),
                new CardInPileCondition(cardID.ToObject(), Pile.Hand))) }.ToILCPP();
            return new List<TriggerEffect>()
            {
                new TriggerEffect(startConditions, new List<ATask>() { new DuplicateCardTask(cardID.ToObject(), Pile.Draw) }.ToILCPP())
            }.ToILCPP();
        }
    }
}
