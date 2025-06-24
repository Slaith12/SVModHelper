using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVModHelper.ModContent
{
    [HarmonyPatch(typeof(ComponentFactory), nameof(ComponentFactory.CreateComponent))]
    internal static class ModComponentFactory
    {
        public static bool Prefix(ComponentName componentName, CardModel cardModel, ref AComponent __result)
        {
            AModComponent component = ModContentManager.GetModComponentInstance(componentName);
            if (component == null)
                return true;

            ModComponentModelDef componentDef = new ModComponentModelDef(component, componentName);
            if (cardModel != null)
                componentDef.SetOnCreateID(cardModel.ID);
            __result = new AComponent(componentDef);

            return false;
        }
    }

    [HarmonyPatch(typeof(CardModel), nameof(CardModel.SetComponent))]
    internal static class OnAddComponent
    {
        public static void Postfix(CardModel __instance)
        {
            AModComponent componentDef = ModContentManager.GetModComponentInstance(__instance.Component.ComponentName);
            if (componentDef == null)
                return;

            if(componentDef.BaseCostModifier != 0)
            {
                switch(ModContentManager.GetAppropriateCostType(__instance))
                {
                    case EncounterValue.Heat:
                        __instance.Component.BaseHeatCostModifier = componentDef.BaseCostModifier;
                        break;
                    case EncounterValue.Power:
                        __instance.Component.BasePowerCostModifier = componentDef.BaseCostModifier;
                        break;
                    case EncounterValue.Mana:
                        __instance.Component.BaseManaCostModifier = componentDef.BaseCostModifier;
                        break;
                    default:
                        __instance.Component.BaseHeatCostModifier = componentDef.BaseCostModifier;
                        break;
                }
            }
            //this is essentially a subsitution for the "NewCardDefinition" property in vanilla components.
            componentDef.ModifyCardModel(__instance);
        }
    }

    [HarmonyPatch(typeof(ContentGetter), nameof(ContentGetter.GetAllComponents), [])]
    internal static class ModdedGetAllComponents
    {
        public static void Postfix(ref Il2CppCollections.List<AComponent> __result)
        {
            for (int i = 0; i < ModContentManager.moddedComponents.Count; i++)
                __result.Add(ComponentFactory.CreateComponent(i + ModContentManager.MINCOMPID));
        }
    }

    [HarmonyPatch(typeof(ContentGetter), nameof(ContentGetter.GetAllComponents), typeof(CardModel))]
    internal static class ModdedGetAllComponentsWithModel
    {
        public static void Postfix(CardModel cardModelOnCreate, ref Il2CppCollections.List<AComponent> __result)
        {
            for (int i = 0; i < ModContentManager.moddedComponents.Count; i++)
                __result.Add(ComponentFactory.CreateComponent(i + ModContentManager.MINCOMPID, cardModelOnCreate));
        }
    }
}
