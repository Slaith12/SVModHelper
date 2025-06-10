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
            foreach(var cardVDPair in SVModHelper.moddedCardVDs)
            {
                if (cardVDPair.Value._material == null)
                    cardVDPair.Value._material = __instance.defaultMat;
                if (cardVDPair.Value._outlineSprite == null)
                    cardVDPair.Value._outlineSprite = __instance.defaultOutlineSprite;

                __instance._cardViewDataDict[cardVDPair.Key] = cardVDPair.Value;
            }
        }
    }
}
