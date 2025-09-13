using System;
using System.Collections.Generic;
using System.Management.Instrumentation;
using BepInEx.Configuration;

namespace CustomHeal
{
    internal static class CustomHealConfig
    {
        private static ConfigEntry<int> HealAmount;
        private static ConfigEntry<int> HealCost;

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
                    "Amount of silk consumed per heal",
                    new AcceptableValueRange<int>(1, 999)
                )
            );
        }

        public static int GetHealAmount() => HealAmount.Value;
        public static int GetHealCost() => HealCost.Value;
    }
}
