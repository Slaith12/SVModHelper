using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SVModHelper;
using SVModHelper.ModContent;
using Il2Cpp;
using MelonLoader;

namespace DemoMod
{
    internal class BlitzDriveTask : AModTask
    {
        public BlitzDriveTask(AValue targetComp)
        {
            args.Add(ArgKey.Value, targetComp);
            Melon<Core>.Logger.Msg("Blitz Drive Task initialized");
        }

        public override IEnumerator Execute(ModTaskInstance taskInstance)
        {
            Melon<Core>.Logger.Msg("Beginning Blitz Drive execution.");
            ComponentName targetComponent = taskInstance.GetArg<ComponentName>(ArgKey.Value);

            Melon<Core>.Logger.Msg("Target Component: " + targetComponent);

            List<CardModel> cards = taskInstance.EncounterModel.CardPlayModel.GetPile(Pile.Draw).ToMono()
                .Select(cardID => taskInstance.EncounterModel.GetModelItem<CardModel>(cardID.ToID()))
                .Where(card => card.Component.ComponentName == targetComponent).ToList();

            Melon<Core>.Logger.Msg("Found " + cards.Count + " matching cards.");

            if(taskInstance.IsPreviewModeView)
            {
                Melon<Core>.Logger.Msg("Performing preview tasks.");
                if (cards.Count > 0)
                    taskInstance.CardPlayView.DrawPileView.SetKnownDrawPreview(cards.ToILCPPEnumerable().PickRandom(taskInstance.EncounterModel.Random));
                taskInstance.CardPlayView.DrawPileView.Preview_Draw(cards.Count);
                Melon<Core>.Logger.Msg("Done with preview tasks.");
                yield break;
            }

            Melon<Core>.Logger.Msg("Performing main tasks.");
            
            foreach(CardModel card in cards)
            {
                //it's possible for the later card to have been drawn by earlier cards, so skip those if that happens
                if (taskInstance.CardPlayModel.GetPileContainingCard(card.CardID) != Pile.Draw)
                    continue;

                Melon<Core>.Logger.Msg("Processing card.");

                //make the target card free
                ACondition freeStartCondition = new EqualsCondition(new TargetValue(), card.CardID.BoxIl2CppObject());
                Melon<Core>.Logger.Msg("Start condition initialized.");
                //the target card will no longer be free after it's played
                List<Il2CppSystem.ValueTuple<Trigger, ACondition>> freeEndCondition = new() { new(Trigger.PostTask, new IsTypeCondition<PlayCardTask>(new RunningTaskValue())) };
                Melon<Core>.Logger.Msg("End condition initialized.");
                PlayCardModifierModel freeModifier = new PlayCardModifierModel(freeStartCondition, ArgKey.IsFree, Operation.Replace, true, freeEndCondition.ToILCPP(), true);
                Melon<Core>.Logger.Msg("Modifier initialized.");
                yield return taskInstance.TaskEngine.ProcessTask(new AddPlayCardModTask(freeModifier)).Cast<Il2CppSystem.Object>();
                Melon<Core>.Logger.Msg("Modifier applied.");

                //draw the card and play it
                yield return taskInstance.TaskEngine.ProcessTask(new MoveCardTask(card.CardID.BoxIl2CppObject(), Pile.Hand, speed: new())).Cast<Il2CppSystem.Object>();
                Melon<Core>.Logger.Msg("Card Moved.");
                //"isSkipSelection" means if the card has no selection, play the card instantly without waiting for the player to confirm.
                //It does not actually skip any selections if the card has any.
                yield return taskInstance.TaskEngine.ProcessTask(new PlayCardWithSelectionTask(card.CardID.BoxIl2CppObject(), false, isSkipSelection: true)).Cast<Il2CppSystem.Object>();
                Melon<Core>.Logger.Msg("Card Played.");
            }
        }
    }
}
