using System;
using BepInEx.Configuration;

namespace CustomHeal
{
    internal static class CustomHealConfig
    {
        private static ConfigEntry<int> HealAmount;
        private static ConfigEntry<int> HealCost;

        // Cached value for in-game use (applies only on restart)
        private static int CachedHealCost;

        public static void Init(ConfigFile config)
        {
            HealAmount = config.Bind(
                "General",
                "HealAmount",
                1,
                new ConfigDescription(
                    "Amount of health restored per heal",
                    new AcceptableValueRange<int>(1, 999)
                )
            );

            HealCost = config.Bind(
                "General",
                "HealCost",
                3,
                new ConfigDescription(
                    "Amount of silk consumed per heal (requires game restart)",
                    new AcceptableValueRange<int>(1, 25)
                )
            );

            // Cache startup value
            CachedHealCost = HealCost.Value;

            // Optional: log a warning when user changes the value
            HealCost.SettingChanged += (s, e) =>
            {
                PluginLogger.LogInfo($"HealCost changed in ConfigManager. " +
                "New value will apply after restart (currently using {CachedHealCost}).");
            };
        }

        // Return current value in-game (restart-only)
        public static int GetHealCost() => CachedHealCost;

        // Return value that updates immediately
        public static int GetHealAmount() => HealAmount.Value;
    }
}
