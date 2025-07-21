using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SVModHelper.ModContent
{
    [HarmonyPatch(typeof(EntityFactory), nameof(EntityFactory.CreateItemModel))]
    internal class ModItemFactory
    {
        public static bool Prefix(ItemName itemName, ref ItemEntityModel __result, EntityFactory __instance)
        {
            AModItem modItem = ModContentManager.GetModItemInstance(itemName);
            if (modItem == null)
                return true;
            EntityID id = new ItemID(itemName, __instance._entityCount).BoxIl2CppObject().Cast<EntityID>();
            __instance._entityCount++;
            ModItemModelDef modelDef = new ModItemModelDef(modItem, itemName);
            ItemEntityModel entityModel = new ItemEntityModel(modelDef, id);
            modelDef.SetOnCreateID(entityModel.ID);
            __result = entityModel;
            return false;
        }
    }
}
