namespace SVModHelper
{
    [HarmonyPatch(typeof(CardModel), "SetCardDefinition")]
    internal class CardPropertyFixer
    {
        public static void Postfix(CardModel __instance)
        {
            if(ModContentManager.activeCardMods.TryGetValue(__instance.CardName, out var mod))
            {
                mod.ApplyTo(__instance);
            }
        }
    }

    [HarmonyPatch(typeof(ArtifactFactory), nameof(ArtifactFactory.CreateArtifactModel))]
    internal class ArtifactPropertyFixer
    {
        public static void Postfix(ArtifactModel __result)
        {
            if (ModContentManager.activeArtifactMods.TryGetValue(__result.ArtifactName, out var mod))
            {
                mod.ApplyTo(__result);
            }
        }
    }

    [HarmonyPatch(typeof(ComponentFactory), nameof(ComponentFactory.CreateComponent))]
    internal class ComponentPropertyFixer
    {
        public static void Postfix(AComponent __result)
        {
            if (ModContentManager.activeComponentMods.TryGetValue(__result.ComponentName, out var mod))
            {
                mod.ApplyTo(__result);
            }
        }
    }

    [HarmonyPatch(typeof(EntityFactory), nameof(EntityFactory.CreateItemModel))]
    internal class ItemPropertyFixer
    {
        public static void Postfix(ItemEntityModel __result)
        {
            if(ModContentManager.activeItemMods.TryGetValue(__result.ItemName, out var mod))
            {
                mod.ApplyTo(__result);
            }
        }
    }
}
