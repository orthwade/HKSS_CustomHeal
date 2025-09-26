using System;
using BepInEx.Configuration;
using GenericVariableExtension;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using System.Linq;

namespace CustomHeal
{
    internal static class CustomHealConfig
    {
        private static ConfigEntry<int> HealAmount;
        private static ConfigEntry<int> HealCost;

        // Cached value for in-game use (applies only on restart)
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


            // Optional: log a warning when user changes the value
            HealCost.SettingChanged += (s, e) =>
            {
                HeroController hero = HeroController.instance;
                if (hero == null)
                {
                    PluginLogger.LogWarning("HeroController.instance is null, cannot update HealCost in FSM");
                    return;
                }

                Events.ConfigHealCostChanged.RefreshHealCost();
            };
        }

        // Return current value in-game (restart-only)
        public static int GetHealCost() => HealCost.Value;

        // Return value that updates immediately
        public static int GetHealAmount() => HealAmount.Value;
    }
}
