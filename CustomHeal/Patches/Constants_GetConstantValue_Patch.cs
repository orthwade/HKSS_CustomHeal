using HarmonyLib;
using HutongGames.PlayMaker;

namespace CustomHeal.Patches
{
    [HarmonyPatch(typeof(FsmVar))]
    [HarmonyPatch(nameof(FsmVar.GetValue))]
    internal static class FsmVar_GetValue_Patch
    {
        private static bool Prefix(FsmVar __instance, ref object __result)
        {
            // If this var isn’t named, just let vanilla handle it
            if (__instance.NamedVar == null)
                return true;

            string varName = __instance.NamedVar.Name;

            switch (varName)
            {
                case nameof(Constants.BIND_SILK_COST):
                case nameof(Constants.BIND_SILK_COST_WITCH):
                    __result = CustomHealConfig.GetHealCost();
                    UnityEngine.Debug.Log($"[CustomHeal] GetValue override {varName} → {__result}");
                    return false; // skip vanilla

                case nameof(Constants.MAX_SILK_REGEN):
                    __result = CustomHealConfig.GetHealAmount();
                    UnityEngine.Debug.Log($"[CustomHeal] GetValue override {varName} → {__result}");
                    return false; // skip vanilla
            }

            // Not our constant → run original method
            return true;
        }
    }
}
