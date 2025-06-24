using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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
}
