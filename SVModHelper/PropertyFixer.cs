namespace SVModHelper
{
    [HarmonyPatch(typeof(CardModel), "SetCardDefinition")]
    internal class PropertyFixer
    {
        public static void Postfix(CardModel __instance)
        {

        }
    }
}
