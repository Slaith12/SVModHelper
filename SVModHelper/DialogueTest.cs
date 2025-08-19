using Il2CppPixelCrushers.DialogueSystem;
using UnityEngine;
using MelonLoader;
using System.Reflection;

namespace SVModHelper
{
    [HarmonyPatch]
    internal class DialogueTest
    {
        public static IEnumerable<MethodBase> TargetMethods()
        {
            yield return AccessTools.Method(typeof(DialogueSystemController), nameof(DialogueSystemController.StartConversation), [typeof(string), typeof(Transform), typeof(Transform), typeof(int), typeof(IDialogueUI)]);
            yield return AccessTools.Method(typeof(DialogueSystemController), nameof(DialogueSystemController.RestoreOriginalUI));
            yield return AccessTools.Method(typeof(DialogueSystemController), nameof(DialogueSystemController.SetConversationUI));
            yield return AccessTools.Method(typeof(DialogueSystemController), nameof(DialogueSystemController.GetNewSequencer));
            yield return AccessTools.Method(typeof(ConversationView), nameof(ConversationView.Initialize));
            yield return AccessTools.Method(typeof(ConversationView), nameof(ConversationView.SetPCPortrait));
            yield return AccessTools.Method(typeof(ConversationModel), nameof(ConversationModel.GetPCName));
            yield return AccessTools.Method(typeof(ConversationModel), nameof(ConversationModel.GetPCSprite));
            yield return AccessTools.Method(typeof(ConversationController), nameof(ConversationController.Initialize), [typeof(ConversationModel), typeof(ConversationView), typeof(bool), typeof(bool), typeof(ConversationController.EndConversationDelegate)]);
            yield return AccessTools.Method(typeof(DialogueDatabase), nameof(DialogueDatabase.GetConversation), [typeof(string)]);
            yield return AccessTools.Method(typeof(DialogueDatabase), nameof(DialogueDatabase.GetConversation), [typeof(int)]);
            yield return AccessTools.Method(typeof(ConversationModel), nameof(ConversationModel.SetParticipants));
            yield return AccessTools.Method(typeof(ConversationModel), nameof(ConversationModel.GetState), [typeof(DialogueEntry), typeof(bool), typeof(bool), typeof(bool)]);
            yield return AccessTools.Method(typeof(ConversationModel), nameof(ConversationModel.SetDialogTable));
            yield return AccessTools.Method(typeof(ConversationModel), nameof(ConversationModel.GetCharacterInfo), [typeof(int)]);
            yield return AccessTools.Method(typeof(ConversationModel), nameof(ConversationModel.ExecuteEntry));
            yield return AccessTools.Method(typeof(ConversationModel), nameof(ConversationModel.CheckSequenceField));
            yield return AccessTools.Method(typeof(DialogueLua), nameof(DialogueLua.MarkDialogueEntry));
            yield return AccessTools.Method(typeof(DialogueLua), nameof(DialogueLua.SetParticipants));
            yield return AccessTools.Method(typeof(ConversationLogger), nameof(ConversationLogger.OnPrepareConversationLine));
        }

        public static void Prefix(MethodBase __originalMethod)
        {
            Melon<Core>.Logger.Msg("Executing method " + __originalMethod.Name);
        }

        public static void Postfix(MethodBase __originalMethod)
        {
            Melon<Core>.Logger.Msg("Done executing method " + __originalMethod.Name);
        }
    }
}
