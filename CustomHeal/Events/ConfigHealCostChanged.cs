using BepInEx.Logging;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using System;
using UnityEngine;

namespace CustomHeal.Events
{
    internal static class ConfigHealCostChanged
    {
        // Ensure we only schedule one deferred update per frame
        private static bool updateScheduled = false;
      
        public static void RefreshHealCost()
        {
            try
            {
                ScheduleDeferredUpdate();
            }
            catch (Exception ex)
            {
                PluginLogger.LogError($"[AfterHealCostChanged] threw: {ex}");
            }
        }

        private static void ScheduleDeferredUpdate()
        {
            if (updateScheduled) return;
            updateScheduled = true;

            // Use the plugin instance to start coroutine
            try
            {
                CustomHeal.Instance.StartCoroutine(DeferredUpdate());
            }
            catch (Exception ex)
            {
                PluginLogger.LogError($"[ScheduleDeferredUpdate] StartCoroutine threw: {ex}");
                updateScheduled = false;
            }
        }

        // Wait for physics to finish then one frame so pogo logic should have run
        private static IEnumerator DeferredUpdate()
        {
            // Wait until after the next physics step (damage occurs in fixed updates),
            // then wait one frame to be extra safe.
            yield return new WaitForFixedUpdate();
            yield return null;

            try
            {
                owd.FsmCache.SpellControl.GetActionBindSilkCost().storeValue.SetValue(CustomHealConfig.GetHealCost());

                owd.FsmCache.SpellControl.GetActionBindSilkCostWitch().storeValue.SetValue(CustomHealConfig.GetHealCost());
                PluginLogger.LogInfo($"[DeferredUpdate] Updated HealCost to {CustomHealConfig.GetHealCost()} in spellControl FSM");

                SilkSpool silkSpool = SilkSpool.Instance;

                if (silkSpool == null)
                {
                    PluginLogger.LogWarning("[DeferredUpdate] SilkSpool.Instance is null, cannot refresh silk");
                    yield break;
                }
                
                silkSpool.RefreshSilk();
            }
            catch (Exception ex)
            {
                PluginLogger.LogError($"[DeferredUpdate]  threw: {ex}");
            }
            finally
            {
                updateScheduled = false;
            }
        }
    }
}
