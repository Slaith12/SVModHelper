using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVModHelper;
using SVModHelper.ModContent;

namespace DemoMod
{
    public class BlitzDrive : AModArtifact
    {
        public override string DisplayName => "Blitz Drive";

        public override string Description => 
            "When you play a card with a component, draw and play every card with the same component for free.";

        public override ClassName Class => ClassName.UniquePack;

        public override Rarity Rarity => Rarity.Legendary;

        public override ContextPreviewType ContextPreviewType => ContextPreviewType.Upgraded;
        public override Il2CppCollections.HashSet<MoreInfoWordName> MoreInfoWords => new HashSet<MoreInfoWordName>()
        {
            MoreInfoWordName.Component,
            MoreInfoWordName.Free
        }.ToILCPP();

        public override Il2CppCollections.List<ATask> GetTaskList(OnCreateIDValue artifactID)
        {
            return new List<ATask>()
            {
                new BlitzDriveTask(new CardComponentNameValue(new TaskArgValue(new RunningTaskValue<PlayCardTask>(), ArgKey.CardID))).Convert()
            }.ToILCPP();
        }

        public override Il2CppCollections.List<TriggerEffect> GetTriggerEffects(OnCreateIDValue artifactID)
        {
            List<Il2CppSystem.ValueTuple<Trigger, ACondition>> triggerConditions = new()
            {
                new (Trigger.PostTask, new AndCondition(
                    new IsTypeCondition<PlayCardEndTask>(new RunningTaskValue()),
                    new HasComponentCondition(new TaskArgValue(new RunningTaskValue<PlayCardEndTask>(), ArgKey.CardID))/*,
                    new NotCondition(new IsChildOfTaskTypeCondition<BlitzDriveTask>())*/))
                // Modded task types are not valid targets for type conditions!
                // Custom conditions will be needed for this
            };

            List<ATask> triggerTasks = new()
            {
                new ProcessArtifactTask(artifactID)
            };

            return new List<TriggerEffect>()
            {
                new TriggerEffect(triggerConditions.ToILCPP(), triggerTasks.ToILCPP())
            }.ToILCPP();
        }
    }
}
