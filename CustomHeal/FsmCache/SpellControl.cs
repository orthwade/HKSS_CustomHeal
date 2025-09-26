using HutongGames.PlayMaker;
using UnityEngine;
using CustomHeal;
using System.Linq;
using HutongGames.PlayMaker.Actions;

namespace owd
{
    namespace FsmCache
    {
        public static class SpellControl
        {
            private static PlayMakerFSM? spellControlFsm;
            private static FsmState[]? states;

            private static FsmState? statePause;
            private static FsmState? stateBindGround;
            private static GetConstantsValue? actionGetConstantsValueBindSilkCost;
            private static GetConstantsValue? actionGetConstantsValueBindSilkCostWitch;


            private static TimeLimitSetV2? actionTimeLimitSetV2;

            private static Tk2dPlayAnimation? actionTk2dPlayAnimation;
            public static void Store(HeroController hero)
            {
                if (hero == null)
                {
                    PluginLogger.LogWarning("HeroController is null, cannot cache spellControl FSM");
                    return;
                }
                if (hero.spellControl == null)
                {
                    PluginLogger.LogWarning("hero.spellControl is null, cannot cache spellControl FSM");
                    return;
                }
                spellControlFsm = hero.spellControl;
                PluginLogger.LogInfo("Cached spellControl FSM");

                states = spellControlFsm.FsmStates;
                PluginLogger.LogInfo($"spellControl FSM has {states.Length} states");

                foreach (var state in states)
                {
                    if (state.Name != "Pause")
                        continue;

                    statePause = state;

                    PluginLogger.LogInfo("Cached Pause state in spellControl FSM");

                    foreach (var action in state.Actions.OfType<GetConstantsValue>())
                    {
                        // Only care about these variables
                        if (action.variableName.IsNone) continue;
                        if (action.storeValue.IsNone) continue;

                        string varName = action.variableName.Value;

                        if (varName != "BIND_SILK_COST" && varName != "BIND_SILK_COST_WITCH")
                            continue;

                        if (varName == "BIND_SILK_COST")
                        {
                            actionGetConstantsValueBindSilkCost = action;
                            PluginLogger.LogInfo("Cached GetConstantsValue action for BIND_SILK_COST");
                        }
                        else if (varName == "BIND_SILK_COST_WITCH")
                        {
                            actionGetConstantsValueBindSilkCostWitch = action;
                            PluginLogger.LogInfo("Cached GetConstantsValue action for BIND_SILK_COST_WITCH");
                        }
                    }
                    foreach (var action in state.Actions.OfType<TimeLimitSetV2>())
                    {
                        if (action.TimeDelay.Name == "Bind Time")
                        {
                            actionTimeLimitSetV2 = action;
                            PluginLogger.LogInfo("Cached TimeLimitSetV2 action for Bind Time");
                        }
                    }
                }
                
                foreach (var state in states)
                {
                    if (state.Name != "Bind Ground")
                        continue;

                    stateBindGround = state;

                    PluginLogger.LogInfo("Cached Bind Ground state in spellControl FSM");

                    foreach (var action in state.Actions.OfType<TimeLimitSetV2>())
                    {
                        PluginLogger.LogInfo($"Found TimeLimitSetV2 action with TimeDelay variable named '{action.TimeDelay.Name}'");
                        if (action.TimeDelay.Name == "Bind Time" && action.StoreValue.Name == "Bind Time Limit")
                        {
                            actionTimeLimitSetV2 = action;
                            PluginLogger.LogInfo("Cached TimeLimitSetV2 action for Bind Time");
                            break;
                        }
                    }
                    foreach (var action in state.Actions.OfType<Tk2dPlayAnimation>())
                    {
                        try
                        {
                            // Log detailed info about the action using reflection-safe helper
                            LogActionDetails(action);

                            if (action.clipName != null && action.clipName.Name == "Bind Silk Anim")
                            {
                                actionTk2dPlayAnimation = action;
                                PluginLogger.LogInfo("Cached Tk2dPlayAnimation action for Bind Silk Anim");
                                break;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            PluginLogger.LogWarning($"Exception while inspecting Tk2dPlayAnimation action: {ex}");
                        }
                        
                    }
                }
            
            // Helper: safe reflection-based logger for action objects
            static void LogActionDetails(object obj)
            {
                if (obj == null)
                {
                    PluginLogger.LogInfo("Action is null");
                    return;
                }

                var t = obj.GetType();
                PluginLogger.LogInfo($"Inspecting action of type '{t.FullName}'");

                // Log public properties
                try
                {
                    foreach (var prop in t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                    {
                        object? val = null;
                        try
                        {
                            val = prop.GetValue(obj);
                        }
                        catch (System.Exception e)
                        {
                            PluginLogger.LogInfo($"  Property {prop.Name}: <exception getting value: {e.GetType().Name}>");
                            continue;
                        }

                        if (val == null)
                        {
                            PluginLogger.LogInfo($"  Property {prop.Name}: null");
                            continue;
                        }

                        // If it's a PlayMaker variable, try to log its name/value
                        if (val is HutongGames.PlayMaker.FsmString fsmStr)
                        {
                            PluginLogger.LogInfo($"  Property {prop.Name}: FsmString name='{fsmStr.Name}' value='{fsmStr.Value}'");
                            continue;
                        }
                        if (val is HutongGames.PlayMaker.FsmInt fsmInt)
                        {
                            PluginLogger.LogInfo($"  Property {prop.Name}: FsmInt name='{fsmInt.Name}' value='{fsmInt.Value}'");
                            continue;
                        }
                        if (val is HutongGames.PlayMaker.FsmFloat fsmFloat)
                        {
                            PluginLogger.LogInfo($"  Property {prop.Name}: FsmFloat name='{fsmFloat.Name}' value='{fsmFloat.Value}'");
                            continue;
                        }
                        if (val is HutongGames.PlayMaker.FsmBool fsmBool)
                        {
                            PluginLogger.LogInfo($"  Property {prop.Name}: FsmBool name='{fsmBool.Name}' value='{fsmBool.Value}'");
                            continue;
                        }

                        // Fall back to ToString
                        PluginLogger.LogInfo($"  Property {prop.Name}: {val}");
                    }
                }
                catch (System.Exception ex)
                {
                    PluginLogger.LogWarning($"Failed to enumerate properties for {t.FullName}: {ex}");
                }

                // Log public fields as well
                try
                {
                    foreach (var field in t.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                    {
                        object? val = null;
                        try
                        {
                            val = field.GetValue(obj);
                        }
                        catch (System.Exception e)
                        {
                            PluginLogger.LogInfo($"  Field {field.Name}: <exception getting value: {e.GetType().Name}>");
                            continue;
                        }

                        if (val == null)
                        {
                            PluginLogger.LogInfo($"  Field {field.Name}: null");
                            continue;
                        }

                        if (val is HutongGames.PlayMaker.FsmString fsmStr)
                        {
                            PluginLogger.LogInfo($"  Field {field.Name}: FsmString name='{fsmStr.Name}' value='{fsmStr.Value}'");
                            continue;
                        }
                        if (val is HutongGames.PlayMaker.FsmInt fsmInt)
                        {
                            PluginLogger.LogInfo($"  Field {field.Name}: FsmInt name='{fsmInt.Name}' value='{fsmInt.Value}'");
                            continue;
                        }
                        if (val is HutongGames.PlayMaker.FsmFloat fsmFloat)
                        {
                            PluginLogger.LogInfo($"  Field {field.Name}: FsmFloat name='{fsmFloat.Name}' value='{fsmFloat.Value}'");
                            continue;
                        }
                        if (val is HutongGames.PlayMaker.FsmBool fsmBool)
                        {
                            PluginLogger.LogInfo($"  Field {field.Name}: FsmBool name='{fsmBool.Name}' value='{fsmBool.Value}'");
                            continue;
                        }

                        PluginLogger.LogInfo($"  Field {field.Name}: {val}");
                    }
                }
                catch (System.Exception ex)
                {
                    PluginLogger.LogWarning($"Failed to enumerate fields for {t.FullName}: {ex}");
                }
            }
            }
            public static GetConstantsValue? GetActionBindSilkCost()
            {
                if (actionGetConstantsValueBindSilkCost == null)
                {
                    PluginLogger.LogWarning("GetConstantsValue action for BIND_SILK_COST is not cached");
                }
                return actionGetConstantsValueBindSilkCost;
            }
            public static GetConstantsValue? GetActionBindSilkCostWitch()
            {
                if (actionGetConstantsValueBindSilkCostWitch == null)
                {
                    PluginLogger.LogWarning("GetConstantsValue action for BIND_SILK_COST_WITCH is not cached");
                }
                return actionGetConstantsValueBindSilkCostWitch;
            }
            public static TimeLimitSetV2? GetActionTimeLimitSetV2()
            {
                if (actionTimeLimitSetV2 == null)
                {
                    PluginLogger.LogWarning("TimeLimitSetV2 action for Bind Time is not cached");
                }
                return actionTimeLimitSetV2;
            }
            public static PlayMakerFSM? GetSpellControlFsm()
            {
                if (spellControlFsm == null)
                {
                    PluginLogger.LogWarning("spellControl FSM is not cached");
                }
                return spellControlFsm;
            }
            public static Tk2dPlayAnimation? GetActionTk2dPlayAnimation()
            {
                if (actionTk2dPlayAnimation == null)
                {
                    PluginLogger.LogWarning("Tk2dPlayAnimation action for Bind Silk Anim is not cached");
                }
                return actionTk2dPlayAnimation;
            }
        }
    }
}