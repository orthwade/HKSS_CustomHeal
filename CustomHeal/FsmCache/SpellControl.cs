using HutongGames.PlayMaker;
using UnityEngine;
using CustomHeal;
using System.Linq;

namespace owd
{
    namespace FsmCache
    {
        public static class SpellControl
        {
            private static PlayMakerFSM spellControlFsm;
            private static FsmState[] states;

            private static FsmState statePause;
            private static GetConstantsValue? actionGetConstantsValueBindSilkCost;
            private static GetConstantsValue? actionGetConstantsValueBindSilkCostWitch;
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
                }
            }
            public static GetConstantsValue GetActionBindSilkCost()
            {
                if (actionGetConstantsValueBindSilkCost == null)
                {
                    PluginLogger.LogWarning("GetConstantsValue action for BIND_SILK_COST is not cached");
                }
                return actionGetConstantsValueBindSilkCost;
            }
            public static GetConstantsValue GetActionBindSilkCostWitch()
            {
                if (actionGetConstantsValueBindSilkCostWitch == null)
                {
                    PluginLogger.LogWarning("GetConstantsValue action for BIND_SILK_COST_WITCH is not cached");
                }
                return actionGetConstantsValueBindSilkCostWitch;
            }
        }
    }
}