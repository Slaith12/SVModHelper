using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SVModHelper
{
    [HarmonyPatch(typeof(CardViewDataSO), nameof(CardViewDataSO.InitDictionary))]
    internal static class CardImageFixer
    {
        public static void Postfix(CardViewDataSO __instance)
        {
            foreach(var vdPair in ModContentManager.moddedCardVDs)
            {
                if (vdPair.Value._material == null)
                    vdPair.Value._material = __instance.defaultMat;
                if (vdPair.Value._outlineSprite == null)
                    vdPair.Value._outlineSprite = __instance.defaultOutlineSprite;

                __instance._cardViewDataDict[vdPair.Key] = vdPair.Value;
            }
        }
    }

    [HarmonyPatch(typeof(ArtifactSpritesSO), nameof(ArtifactSpritesSO.InitDictionary))]
    internal static class ArtifactImageFixer
    {
        public static void Postfix(ArtifactSpritesSO __instance)
        {
            foreach (var vdPair in ModContentManager.moddedArtifactVDs)
            {
                __instance._dict[vdPair.Key] = vdPair.Value;
            }
        }
    }

    [HarmonyPatch(typeof(ComponentSpritesSO), nameof(ComponentSpritesSO.InitDictionary))]
    internal static class ComponentImageFixer
    {
        public static void Postfix(ComponentSpritesSO __instance)
        {
            foreach (var vdPair in ModContentManager.moddedComponentVDs)
            {
                __instance._dict[vdPair.Key] = vdPair.Value;
            }
        }
    }

    [HarmonyPatch(typeof(ItemPackSpritesSO), nameof(ItemPackSpritesSO.InitDictionary))]
    internal static class PackImageFixer
    {
        public static void Postfix(ItemPackSpritesSO __instance)
        {
            foreach (var vdPair in ModContentManager.moddedPackVDs)
            {
                __instance._dict[vdPair.Key] = vdPair.Value;
            }
        }
    }

    //Items and Enemies use addressables for their view data assets, which makes this a lot harder.
    [HarmonyPatch(typeof(ItemViewDataListSO), nameof(ItemViewDataListSO.GetData))]
    internal static class ItemImageFixer
    {
        public static bool Prefix(ItemName entry, ref AsyncOperationHandle<AEntityViewDataSO> __result, ItemViewDataListSO __instance)
        {
            if (!ModContentManager.moddedItemVDs.TryGetValue(entry, out var viewData))
                return true;
            __result = __instance.GetData(ItemName.HealPackage);
            return false;
        }
    }
}
