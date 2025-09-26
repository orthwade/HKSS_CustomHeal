using BepInEx.Logging;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using System;
using UnityEngine;
using GenericVariableExtension;

namespace CustomHeal.Events
{
    internal static class ConfigHealDurationChanged
    {
        // Ensure we only schedule one deferred update per frame
        private static bool updateScheduled = false;
      
        public static void RefreshHealDuration()
        {
            try
            {
                ScheduleDeferredUpdate();
            }
            catch (Exception ex)
            {
                PluginLogger.LogError($"[AfterHealDurationChanged] threw: {ex}");
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
                // float newDurationSeconds = CustomHealConfig.GetHealDurationMs() / 1000f;
                owd.FsmCache.SpellControl.GetActionTimeLimitSetV2().TimeDelay.Value = 0.001f;
                owd.FsmCache.SpellControl.GetActionTimeLimitSetV2().StoreValue.Value = 0.001f;
                if(owd.FsmCache.SpellControl.GetActionTk2dPlayAnimation() != null && 
                   owd.FsmCache.SpellControl.GetActionTk2dPlayAnimation().clipName != null &&
                   owd.FsmCache.SpellControl.GetActionTk2dPlayAnimation().Active &&
                   !owd.FsmCache.SpellControl.GetActionTk2dPlayAnimation().Finished)
                {
                    owd.FsmCache.SpellControl.GetActionTk2dPlayAnimation().Finish();
                }


                PluginLogger.LogInfo($"[DeferredUpdate] Updated HealDuration to {CustomHealConfig.GetHealDurationMs()} ms in spellControl FSM");

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
