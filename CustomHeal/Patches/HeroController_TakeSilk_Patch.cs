using HarmonyLib;

namespace CustomHeal.Patches
{
    [HarmonyPatch(typeof(HeroController))]
    [HarmonyPatch("TakeSilk")]
    [HarmonyPatch(new[] { typeof(int), typeof(SilkSpool.SilkTakeSource) })]
    internal static class HeroController_TakeSilk_Patch
    {
        private static void Prefix(ref int amount, SilkSpool.SilkTakeSource source)
        {
            amount = CustomHealConfig.GetHealCost();
            UnityEngine.Debug.Log($"[CustomHeal] Heal cost set to {amount}, source = {source}");
        }

        private static void Postfix(int amount, SilkSpool.SilkTakeSource source)
        {
            UnityEngine.Debug.Log($"[CustomHeal] Silk spent: {amount}, source = {source}");
        }
    }
}
