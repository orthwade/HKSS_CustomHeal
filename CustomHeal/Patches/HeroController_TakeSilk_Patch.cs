using HarmonyLib;
using UnityEngine;
using System.Diagnostics;

namespace CustomHeal.Patches
{
    [HarmonyPatch(typeof(HeroController))]
    [HarmonyPatch("TakeSilk")]
    [HarmonyPatch(new[] { typeof(int), typeof(SilkSpool.SilkTakeSource) })]
    internal static class HeroController_TakeSilk_Patch
    {
        private static bool Prefix(ref int amount, SilkSpool.SilkTakeSource source)
        {
            // var trace = new StackTrace(true);
            // PluginLogger.LogInfo($"HeroController.TakeSilk called with amount={amount}, source={source}\nStack trace:\n{trace}");
            return true;
        }
    }

    [HarmonyPatch(typeof(HutongGames.PlayMaker.Actions.TakeSilk), MethodType.Constructor)]
    class TakeSilkCtorPatch
    {
        static void Postfix(HutongGames.PlayMaker.Actions.TakeSilk __instance)
        {
            // var trace = new StackTrace(true);
            // PluginLogger.LogInfo($"TakeSilk constructor called.\nStack trace:\n{trace}");
        }
    }
}
