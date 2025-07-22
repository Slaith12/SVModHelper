using SVModHelper;
using SVModHelper.ModContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMod
{
    public class LandmineItem : AModItem
    {
        public override string DisplayName => "Landmine";

        public override string Description => "Detonates when struck or when an entity moves into an adjacent tile.";

        public override Il2CppCollections.HashSet<EntityTrait> Traits => new HashSet<EntityTrait>() { EntityTrait.Bomb }.ToILCPP();
        public override Il2CppCollections.HashSet<EntityTrait> HiddenTraits => new HashSet<EntityTrait>() { EntityTrait.PlayerBomb }.ToILCPP();

        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new HashSet<MoreInfoWordName>()
        {
            MoreInfoWordName.Bomb
        }.ToILCPP();

        public override Il2CppCollections.List<ATask> GetPreSpawnTaskList(OnCreateIDValue itemID)
        {
            Il2CppCollections.List<ATask> taskList = new();
            taskList.Add(new AddToPlayerBombListTask(itemID));
            return taskList;
        }

        public override Il2CppCollections.List<ATask> GetPreviewTaskList(OnCreateIDValue itemID)
        {
            Il2CppCollections.List<ATask> taskList = new();
            taskList.Add(new ShowBombNumberTask());
            taskList.Add(new DestroyEntityTask(itemID));
            return taskList;
        }

        public override Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue itemID)
        {
            Il2CppCollections.List<TriggerEffect> triggerEffects = new();

            //Detonate when destroyed
            Il2CppCollections.List<ATask> detonateTaskList = new();
            detonateTaskList.Add(new RemoveFromPlayerBombListTask(itemID));
            detonateTaskList.Add(new DetonationTask(new TaskArgValue(new RunningTaskValue<DestroyEntityTask>(), ArgKey.Coord)));

            triggerEffects.Add(new TriggerEffect(TriggerEffectHelpers.DestroyIDTriggerConditions(itemID), detonateTaskList, true));

            //Destroy (detonate) when an entity moves to an adjacent tile
            Il2CppCollections.List<ATask> destroyTaskList = new();
            destroyTaskList.Add(new DestroyEntityTask(itemID));

            Il2CppCollections.List<Il2CppSystem.ValueTuple<Trigger, ACondition>> destroyConditions = new();
            destroyConditions.Add(new(Trigger.PostTask, new AndCondition(
                new IsTypeCondition<MoveEntityTask>(new RunningTaskValue()),
                new NotCondition(new EqualsCondition(new TaskArgValue(new RunningTaskValue(), ArgKey.EntityID), itemID)),
                new DistanceCondition(new EntityCoordValue(itemID), new TaskArgValue(new RunningTaskValue(), ArgKey.Coord), 1, 1))));

            triggerEffects.Add(new TriggerEffect(destroyConditions, TriggerEffectHelpers.DestroyOrDissipateIDTriggerConditions(itemID), destroyTaskList, true));

            //Remove from bomb list when dissipated
            Il2CppCollections.List<ATask> dissipateTaskList = new();
            dissipateTaskList.Add(new RemoveFromPlayerBombListTask(itemID));

            triggerEffects.Add(new TriggerEffect(TriggerEffectHelpers.DissipateIDTriggerConditions(itemID), dissipateTaskList, true));

            return triggerEffects;
        }
    }
}
