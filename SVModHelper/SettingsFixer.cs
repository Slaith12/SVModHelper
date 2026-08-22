using UnityEngine;

namespace SVModHelper
{
    [HarmonyPatch(typeof(SandboxSceneController), nameof(SandboxSceneController.SetupPacks))]
    internal static class PackSettingsFixer
    {
        public static void Prefix(SandboxSceneController __instance)
        {
            OptionToggle baseToggle = __instance.PackToggleList[0];
            Transform parent = baseToggle.transform.parent;
            int baseSiblingIndex = baseToggle.transform.GetSiblingIndex();
            //add missing buttons
            for(int i = __instance.PackToggleList.Count; i < ItemPackData.TrueClassicPackData.Count; i++)
            {
                OptionToggle newToggle = UnityEngine.Object.Instantiate(baseToggle, parent);
                newToggle.transform.SetSiblingIndex(baseSiblingIndex + i);
                __instance.PackToggleList.Add(newToggle);
            }
            //remove unneeded buttons
            while(ItemPackData.TrueClassicPackData.Count < __instance.PackToggleList.Count)
            {
                int index = __instance.PackToggleList.Count - 1;
                UnityEngine.Object.Destroy(__instance.PackToggleList[index]);
                __instance.PackToggleList.RemoveAt(index);
            }
        }
    }
}
