using HarmonyLib;

namespace owd.CustomHeal.Patches
{
    [HarmonyPatch(typeof(HeroController))]
    [HarmonyPatch("Start")]
    internal static class HeroController_Start
    {
        private static void Postfix(HeroController __instance)
        {
            if (__instance == null)
            {
                PluginLogger.LogWarning("HeroController instance is null in Start postfix");
                return;
            }
            FsmCache.SpellControl.Store(__instance);
            Events.ConfigHealCostChanged.RefreshHealCost();
            Events.ConfigHealDurationChanged.RefreshHealDuration();
        }
    }
}
