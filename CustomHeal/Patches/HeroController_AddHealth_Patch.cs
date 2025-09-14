using HarmonyLib;

namespace CustomHeal.Patches
{
    [HarmonyPatch(typeof(HeroController), nameof(HeroController.AddHealth))]
    internal static class HeroController_AddHealth_Patch
    {
        // Postfix will run after AddHealth executes
        private static void Postfix(int amount)
        {
            // Example: Do something whenever healing happens
            CustomHealLogger.LogInfo($"Player healed for {amount} HP!");

            // You could add custom logic here, e.g.:
            // - Trigger a visual effect
            // - Cap healing
            // - Give bonus effects
        }

        // If you want to modify the healing amount BEFORE it happens:
        private static void Prefix(ref int amount)
        {
            // Example: Double healing
            if (amount == 3) // Only modify if healing is positive
                amount = CustomHealConfig.GetHealAmount();

            CustomHealLogger.LogInfo($"Healing modified to {amount}");
        }
    }
}
