using SVModHelper;
using SVModHelper.ModContent;
using System;
using System.Collections.Generic;

namespace DemoMod
{
    public class FollowThrough : AModCard
    {
        public override string DisplayName => "Follow Through";

        public override string Description => "<nobr><sprite=\"TextIcons\" name=\"Flow\"> <b><color=#FFBF00>Flow</b></color></nobr>.\n" +
                                              "Move 1 to 2 tiles.\n" +
                                              "When an invader is destroyed, return this card to hand from your discard pile.";

        public override Il2CppCollections.HashSet<CardTrait> Traits => new HashSet<CardTrait>() 
        {
            CardTrait.Move
        }.ToILCPP();
        public override Il2CppCollections.HashSet<CardTrait> HiddenTraits => new HashSet<CardTrait>() 
        {
            //NOTE: This allows other cards/artifacts to recognize this card as a "flow" card, but it does NOT apply the flow effect to the card
            //The flow effect must be applied in the GetTriggerEffects() function
            CardTrait.Flow
        }.ToILCPP();

        public override ClassName Class => ClassName.Melee;
        public override Rarity Rarity => Rarity.Common;
        public override int ClassBaseCost => 1;
        public override bool RequiresPlayerEntity => true;

        public override Il2CppCollections.HashSet<ComponentTrait> AllowedComponentTraits => new HashSet<ComponentTrait>()
        {
            ComponentTrait.Basic,
            ComponentTrait.Selection,
            ComponentTrait.Move,
        }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentTrait> BlockedComponentTraits => new HashSet<ComponentTrait>()
        {
            ComponentTrait.SelectionLess,
            ComponentTrait.Costed,
        }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentName> AllowedComponentNames => new HashSet<ComponentName>()
        {
            ComponentName.Jab,
            ComponentName.Tactical,
        }.ToILCPP();

        public override Il2CppCollections.HashSet<ComponentName> BlockedComponentNames => new HashSet<ComponentName>()
        {
            ComponentName.Wild,
            ComponentName.Boosted,
        }.ToILCPP();

        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new HashSet<MoreInfoWordName>() { MoreInfoWordName.Flow }.ToILCPP();

        public override Il2CppCollections.List<Selection> GetSelections(OnCreateIDValue cardID)
        {
            Il2CppCollections.List<Selection> selections = new();
            selections.Add(new Selection(new AndCondition(
                new IsTypeCondition<Coord>(new TargetValue()),   // Make sure the selection is a coordinate on the board
                new IsMoveableCoordCondition(new TargetValue()), // Check if player can move to the selected coordinate
                new DistanceCondition(new PlayerCoordValue(), new TargetValue(), 1, 2) // Check if selected coordinate is within range
                )));
            return selections;
        }

        public override Il2CppCollections.List<ATask> GetPostSelectionTaskList(OnCreateIDValue cardID)
        {
            Il2CppCollections.List<ATask> tasks = new();
            tasks.Add(new MoveEntityTask(new PlayerIDValue(), new TargetValue())); //Move player to the selected coordinate
            return tasks;
        }

        public override Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue cardID)
        {
            Il2CppCollections.List<TriggerEffect> triggerEffects = new();

            //Construct "return to hand" trigger
            Il2CppCollections.List<Il2CppSystem.ValueTuple<Trigger, ACondition>> triggerConditions = new();
            triggerConditions.Add(new(Trigger.PostTask, //Check the condtion after a task finishes
                new AndCondition(
                    new IsTypeCondition<DestroyEntityTask>(new RunningTaskValue()),                  //Is the current task a DestroyEntityTask?
                    new IsEnemyCondition(new TaskArgValue(new RunningTaskValue(), ArgKey.EntityID)), //Is the entity being destroyed an invader?
                    new CardInPileCondition(cardID, Pile.Discard)                                    //Is this card in the discard pile?
                )));

            Il2CppCollections.List<ATask> triggerTaskList = new();
            triggerTaskList.Add(new MoveCardTask(cardID, Pile.Hand, speed: new())); //Return this card to hand.

            //Register "return to hand" trigger
            triggerEffects.Add(new TriggerEffect(triggerConditions, triggerTaskList));
            //Register flow trigger
            triggerEffects.Add(TriggerEffectHelpers.GetFlowTriggerEffect(cardID));

            return triggerEffects;
        }
    }
}
