using System;
using BepInEx.Configuration;
using GenericVariableExtension;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;

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
                if(hero.spellControl == null)
                {
                    PluginLogger.LogWarning("hero.spellControl is null, cannot update HealCost in FSM");
                    return;
                }
                var before = hero.spellControl.GetVariable<int>("Silk Cost"); 
                // Update the FSM variable directly
                var states = hero.spellControl.FsmStates;
                foreach (var state in states)
                {
                    if(state.Name == "Pause")
                    {
                        foreach (var action in state.Actions)
                        {
                            if (action is GetConstantsValue getConstAction)
                            {
                                if (getConstAction.variableName.IsNone) continue;
                                if (getConstAction.variableName.Value != "BIND_SILK_COST" &&
                                    getConstAction.variableName.Value != "BIND_SILK_COST_WITCH") continue;
                                if (getConstAction.storeValue.IsNone) continue;

                                // Safely read the boxed value and convert to int
                                var valObj = getConstAction.storeValue.GetValue();
                                int currentValue = valObj is int ? (int)valObj : Convert.ToInt32(valObj);

                                if (currentValue == GetHealCost()) continue; // already set

                                getConstAction.storeValue.SetValue(GetHealCost());
                                SilkSpool spool = SilkSpool.Instance;
                                if (spool != null)
                                {
                                    Events.ConfigHealCostChanged.AfterHealCostChanged();
                                    PluginLogger.LogInfo($"Updated HealCost in FSM from {before} to {GetHealCost()}");
                                }
                                else
                                {
                                    PluginLogger.LogWarning("SilkSpool.Instance is null, cannot update HealCost in FSM");
                                }
                                break;
                            }
                        }
                    }
                }
            };
        }

        // Return current value in-game (restart-only)
        public static int GetHealCost() => HealCost.Value;

        // Return value that updates immediately
        public static int GetHealAmount() => HealAmount.Value;
    }
}
