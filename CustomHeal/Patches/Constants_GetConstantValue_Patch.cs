using UnityEngine;
using HarmonyLib;
using HutongGames.PlayMaker;

namespace CustomHeal.Patches
{
    [HarmonyPatch(typeof(GetConstantsValue))]
    [HarmonyPatch("OnEnter")]
    public static class GetConstantsValue_OnEnter_Patch
    {
        // Postfix runs AFTER the original OnEnter
        public static void Postfix(GetConstantsValue __instance)
        {
            // Example: override a specific variable
            if (__instance.variableName != null && !__instance.variableName.IsNone)
            {
                var variableName = __instance.variableName.Value;
                switch (variableName)
                {
                    case "BIND_SILK_COST":
                    case "BIND_SILK_COST_WITCH":
                        var cost = CustomHealConfig.GetHealCost();
                        __instance.storeValue.SetValue(cost);
                        PluginLogger.LogInfo($"Overridden {variableName} = {cost}");
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
