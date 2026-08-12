using SVModHelper.ModContent;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SVModHelper
{
    [HarmonyPatch(typeof(CardViewDataSO), nameof(CardViewDataSO.InitDictionary))]
    internal static class CardImageFixer
    {
        public static void Postfix(CardViewDataSO __instance)
        {
            foreach((CardName card, CardViewDescriptor descriptor) in ModContentManager.moddedCardVDs)
            {
                CardViewData viewData = SpriteHelper.GetCardViewData(descriptor);
                if (viewData == null)
                    continue;

                viewData._cardName = card;
                if (viewData._material == null)
                    viewData._material = __instance.defaultMat;
                if (viewData._outlineSprite == null)
                    viewData._outlineSprite = __instance.defaultOutlineSprite;

                __instance._cardViewDataDict[card] = viewData;
            }
        }
    }

    [HarmonyPatch(typeof(ArtifactSpritesSO), nameof(ArtifactSpritesSO.InitDictionary))]
    internal static class ArtifactImageFixer
    {
        public static void Postfix(ArtifactSpritesSO __instance)
        {
            foreach ((ArtifactName artifact, SpriteDescriptor descriptor) in ModContentManager.moddedArtifactVDs)
            {
                __instance._dict[artifact] = SpriteHelper.GetSprite(descriptor);
            }
        }
    }

    [HarmonyPatch(typeof(ComponentSpritesSO), nameof(ComponentSpritesSO.InitDictionary))]
    internal static class ComponentImageFixer
    {
        public static void Postfix(ComponentSpritesSO __instance)
        {
            foreach ((ComponentName component, SpriteDescriptor descriptor) in ModContentManager.moddedComponentVDs)
            {

                __instance._dict[component] = SpriteHelper.GetSprite(descriptor);
            }
        }
    }

    [HarmonyPatch(typeof(ItemPackSpritesSO), nameof(ItemPackSpritesSO.InitDictionary))]
    internal static class PackImageFixer
    {
        public static void Postfix(ItemPackSpritesSO __instance)
        {
            foreach ((ItemPackName pack, SpriteDescriptor descriptor) in ModContentManager.moddedPackVDs)
            {
                __instance._dict[pack] = SpriteHelper.GetSprite(descriptor);
            }
        }
    }

    //Items and Enemies use addressables for their view data assets, which makes this a lot harder.
    [HarmonyPatch(typeof(ItemViewDataListSO), nameof(ItemViewDataListSO.GetData))]
    internal static class ItemImageFixer
    {
        public static bool Prefix(ItemName entry, ref AsyncOperationHandle<AEntityViewDataSO> __result, ItemViewDataListSO __instance)
        {
            //The sprites in the cached VDs get deleted sometime after initialization, so we need to recreate the VDs every time.
            //This also means VDs added by ItemModifications are ignored. Even if we checked the mods again here, the VD would probably be deleted already.
            if (!ModContentManager.moddedItemVDs.TryGetValue(entry, out ItemViewDescriptor descriptor))
                return true;
            
            ItemViewDataSO viewData = SpriteHelper.GetItemData(descriptor);
            AsyncOperationBase<AEntityViewDataSO> op = new EntityViewDataInjector(viewData);
            __result = new AsyncOperationHandle<AEntityViewDataSO>(op);
            return false;
        }
    }
}
