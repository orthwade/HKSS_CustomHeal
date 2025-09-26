// using UnityEngine;
// using HarmonyLib;
// using HutongGames.PlayMaker;
//         using System.Diagnostics;

// namespace CustomHeal.Patches
// {
//     [HarmonyPatch(typeof(GetConstantsValue))]
//     [HarmonyPatch("OnEnter")]
//     public static class GetConstantsValue_OnEnter_Patch
//     {
//         // Postfix runs AFTER the original OnEnter

//         public static void Postfix(GetConstantsValue __instance)
//         {
//             if (__instance.variableName != null && !__instance.variableName.IsNone)
//             {
//                 var variableName = __instance.variableName.Value;
//                 var cost = CustomHealConfig.GetHealCost();

//                 if (variableName == "BIND_SILK_COST" || variableName == "BIND_SILK_COST_WITCH")
//                 {
//                     __instance.storeValue.SetValue(cost);
//                     PluginLogger.LogInfo($"Overridden {variableName} = {cost}");

//                     // Dump a trace at startup
//                     // var trace = new StackTrace(true);
//                     // PluginLogger.LogInfo($"Stack trace for {variableName}:\n{trace}");
//                 }
//             }
//         }
//     }
// }
