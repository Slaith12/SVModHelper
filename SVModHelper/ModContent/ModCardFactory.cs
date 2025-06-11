using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVModHelper.ModContent
{
    [HarmonyPatch(typeof(CardFactory), nameof(CardFactory.CreateCardModel))]
    internal static class ModCardFactory
    {
        public static bool Prefix(CardName cardName, ref CardModel __result, CardFactory __instance)
        {
            if(cardName >= (CardName)15000 && cardName < (CardName)(SVModHelper.moddedCards.Count+15000))
            {
                CardID cardID = new CardID(cardName, __instance._cardCount);
                __instance._cardCount++;
                ModCardModelDef cardModelDef = new ModCardModelDef(SVModHelper.moddedCards[(int)cardName-15000], cardName);
                CardModel cardModel = new CardModel(cardModelDef, cardID);

                cardModel.SetComponent(ComponentFactory.CreateComponent(ComponentName.None, cardModel));
                cardModelDef.SetOnCreateID(cardModel.ID);
                __result = cardModel;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(ContentGetter), nameof(ContentGetter.GetAllCards))]
    internal static class ModdedGetAllCards
    {
        public static void Postfix(ref Il2CppCollections.List<CardModel> __result)
        {
            CardFactory cardFactory = new CardFactory();
            for(int i = 0; i < SVModHelper.moddedCards.Count; i++)
                __result.Add(cardFactory.CreateCardModel((CardName)(i+15000)));
        }
    }
}
