using HarmonyLib;

namespace owd.CustomHeal.Patches
{
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.AddHealth))]
    internal static class HeroController_AddHealth_Patch
    {
        private static void Prefix(ref int amount)
        {
            amount = Conf.ResolveHealAmount(amount);

            PluginLogger.LogInfo($"Healing modified to {amount}");
        }
    }
}
