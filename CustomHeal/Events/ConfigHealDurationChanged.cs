using System.Collections;
using System;
using UnityEngine;

namespace owd.CustomHeal.Events
{
    internal static class ConfigHealDurationChanged
    {
        // Ensure we only schedule one deferred update per frame
        private static bool updateScheduled = false;
      
        public static void RefreshHealDuration()
        {
            return; // TEMP DISABLE
            
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
                FsmCache.SpellControl.GetActionTimeLimitSetV2().TimeDelay.Value = 0.001f;
                FsmCache.SpellControl.GetActionTimeLimitSetV2().StoreValue.Value = 0.001f;
                if(FsmCache.SpellControl.GetActionTk2dPlayAnimation() != null && 
                   FsmCache.SpellControl.GetActionTk2dPlayAnimation().clipName != null &&
                   FsmCache.SpellControl.GetActionTk2dPlayAnimation().Active &&
                   !FsmCache.SpellControl.GetActionTk2dPlayAnimation().Finished)
                {
                    FsmCache.SpellControl.GetActionTk2dPlayAnimation().Finish();
                }


                PluginLogger.LogInfo($"[DeferredUpdate] Updated HealDuration to {Conf.GetHealDurationMs()} ms in spellControl FSM");

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
