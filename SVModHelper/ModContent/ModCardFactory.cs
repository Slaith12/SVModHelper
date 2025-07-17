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
            if(cardName >= (CardName)ModContentManager.MINCARDID)
            {
                CardID cardID = new CardID(cardName, __instance._cardCount);
                __instance._cardCount++;

                var type = ModContentManager.moddedCardDict.FirstOrDefault(pair => pair.Value == cardName).Key;

                if (type == null)
				{
					MelonLogger.Error($"CardName {cardName} not found in modded card dictionary.");
					__result = null;
					return false;
				}

				AModCard modCard = Activator.CreateInstance(type, true) as AModCard;

				ModCardModelDef cardModelDef = new ModCardModelDef(modCard, cardName);
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
            foreach(CardName cardName in ModContentManager.moddedCardDict.Values)
                __result.Add(cardFactory.CreateCardModel(cardName));
        }
    }
}
