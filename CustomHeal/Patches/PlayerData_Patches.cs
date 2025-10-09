using HarmonyLib;

namespace owd.CustomHeal.Patches
{
    [HarmonyPatch(typeof(PlayerData), nameof(PlayerData.CurrentSilkRegenMax), MethodType.Getter)]
    internal static class PlayerData_CurrentSilkRegenMax_Patch
    {
        private const string postfix = "PlayerData_CurrentSilkRegenMax_Patch: ";

        static void Postfix(PlayerData __instance, ref int __result)
        {
            if (__instance == null)
            {
                PluginLogger.LogWarning(postfix + "__instance is null");
                return;
            }
            __result = Conf.ResolveSilkRegenMax(__result);
        }
    }
}
