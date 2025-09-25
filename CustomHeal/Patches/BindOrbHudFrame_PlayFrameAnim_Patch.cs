// using UnityEngine;
// using HarmonyLib;
// using HutongGames.PlayMaker;
//         using System.Diagnostics;

// namespace CustomHeal.Patches
// {
//     [HarmonyPatch(typeof(BindOrbHudFrame))]
//     [HarmonyPatch("PlayFrameAnim")]
//     public static class BindOrbHudFrame_PlayFrameAnim_Patch
//     {

//          private static bool Prefix(string animName, int frame)
//         {
//             PluginLogger.LogInfo($"BindOrbHudFrame.PlayFrameAnim called with animName={animName}, frame={frame}");
//             // var trace = new StackTrace(true);
//             //         PluginLogger.LogInfo($"Stack trace for BindOrbHudFrame.PlayFrameAnim:\n{trace}");
//             return true;
//         }
//     }
// }
