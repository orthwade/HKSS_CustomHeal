using HarmonyLib;
using UnityEngine;

namespace CustomHeal.Patches
{
    [HarmonyPatch(typeof(SilkSpool), "BindCost", MethodType.Getter)]
    internal static class SilkSpool_BindCost_Patch
    {
        [HarmonyPrefix]
        private static bool Prefix(ref float __result)
        {
            if (PlayerData.instance.IsAnyCursed)
            {
                __result = float.MaxValue;
                PluginLogger.LogInfo("Player is cursed → BindCost set to MaxValue");
                return false; // skip original getter
            }

            float cost = CustomHealConfig.GetHealCost();
            __result = cost;
            PluginLogger.LogInfo($"Overriding BindCost → {cost}");
            return false; // skip original getter
        }
    }
}
