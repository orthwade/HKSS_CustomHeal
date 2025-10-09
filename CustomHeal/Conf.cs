using System;
using BepInEx.Configuration;

namespace owd.CustomHeal
{
    internal static class Conf
    {
        private static ConfigEntry<int> HealAmount;
        private static ConfigEntry<int> HealCost;

        public enum HealDurationModeEnum
        {
            Vanilla,
            Custom
        }

        private static ConfigEntry<HealDurationModeEnum> HealDurationMode;


        private static ConfigEntry<int> HealDurationMs;

        public enum MultibinderModeEnum
        {
            Vanilla,
            Global,
            Custom
        }

        private static ConfigEntry<MultibinderModeEnum> MultibinderMode;
        private static ConfigEntry<int> MultibinderAmount;

        private static ConfigEntry<int> SilkRegenMax;
        private static int GetSilkRegenMax() { return SilkRegenMax.Value; }
        public static int ResolveSilkRegenMax(int original) { return Math.Min(original, GetSilkRegenMax()); }

        public static void Init(ConfigFile config)
        {
            HealAmount = config.Bind(
                "General",
                "HealAmount",
                1,
                new ConfigDescription(
                    "Amount of health restored per heal",
                    new AcceptableValueRange<int>(1, 10),
                    new ConfigurationManagerAttributes { Order = -2 }
                )
            );

            HealCost = config.Bind(
                "General",
                "HealCost",
                3,
                new ConfigDescription(
                    "Amount of silk consumed per heal",
                    new AcceptableValueRange<int>(1, 22),
                    new ConfigurationManagerAttributes { Order = -3 }
                )
            );

            // HealDurationMode = config.Bind(
            //     "General",
            //     "HealDurationMode",
            //     HealDurationModeEnum.Vanilla,
            //     new ConfigDescription(
            //         "Mode for heal duration: Vanilla uses the game's default, Custom allows setting a specific duration"
            //     )
            // );

            // HealDurationMs = config.Bind(
            //     "General",
            //     "HealDurationMs",
            //     200,
            //     new ConfigDescription(
            //         "Duration of the heal animation in milliseconds. Effective only if HealDurationMode is set to Custom",
            //         new AcceptableValueRange<int>(100, 1141)
            //     )
            // );


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

            MultibinderMode = config.Bind(
                "General",
                "MultibinderMode",
                MultibinderModeEnum.Global,
                new ConfigDescription(
                    "Mode for Heal Amount when having Multibinder equiped" +
                    "\nVanilla: Uses the game's default behaviour" +
                    "\nGlobal: Uses HealAmount(Global)" +
                    "\nCustom: Uses a custom HealAmount",
                    null,
                    new ConfigurationManagerAttributes { Order = -4 }
                )
            );
            MultibinderAmount = config.Bind(
                "General",
                "MultibinderAmount",
                1,
                new ConfigDescription(
                    "Amount of health restored per heal when having Multibinder equiped " +
                    "\n(requires MultibinderMode to be set to Custom)",
                    new AcceptableValueRange<int>(1, 10),
                    new ConfigurationManagerAttributes { Order = -5 }
                )
            );
            SilkRegenMax = config.Bind(
                "General",
                "SilkRegenMax",
                2,
                new ConfigDescription(
                    "Cap max regenerated silk chunks. If SilkRegenMax < SilkHeartsFound, regen stops.",
                    new AcceptableValueRange<int>(0, 3),
                    new ConfigurationManagerAttributes { Order = -6 }
                )
            );

            // SilkRegenMax.SettingChanged += (s, e) =>
            // {
            //     HeroController hero = HeroController.instance;
            //     if (hero == null)
            //     {
            //         PluginLogger.LogWarning("HeroController.instance is null, cannot update SilkRegenMax");
            //         return;
            //     }

            //     PlayerData playerData = hero.playerData;
            //     if (playerData == null)
            //     {
            //         PluginLogger.LogWarning("playerData is null, cannot update SilkRegenMax");
            //         return;
            //     }

            //     // playerData.HasSeenSilkHearts

            //     playerData.silkRegenMax = Math.Min(playerData.silkRegenMax, )
            // };

            // HealDurationMs.SettingChanged += (s, e) =>
            // {
            //     HeroController hero = HeroController.instance;
            //     if (hero == null)
            //     {
            //         PluginLogger.LogWarning("HeroController.instance is null, cannot update HealDuration in FSM");
            //         return;
            //     }

            //     Events.ConfigHealDurationChanged.RefreshHealDuration();
            // };
        }

        public static int GetHealCost() => HealCost.Value;

        // Return value that updates immediately
        // public static int GetHealAmount() => HealAmount.Value;

        public static HealDurationModeEnum GetHealDurationMode() => HealDurationMode.Value;
        public static int GetHealDurationMs()
        {
            return Math.Max(HealDurationMs.Value, 100); // Ensure a minimum duration of 100ms
        }
        public static int ResolveHealAmount(int vanillaAmount)
        {
            if (vanillaAmount == 3)
            {
                return HealAmount.Value;
            }

            if (vanillaAmount == 2)
            {
                if (MultibinderMode.Value == MultibinderModeEnum.Vanilla)
                {
                    return vanillaAmount;
                }
                else if (MultibinderMode.Value == MultibinderModeEnum.Global)
                {
                    return HealAmount.Value;
                }

                return MultibinderAmount.Value;
            }
            
            return vanillaAmount;
        }
    }
}
