using HarmonyLib;
using UnityEngine;
using HutongGames.PlayMaker.Actions;
using System.Collections;

namespace owd.CustomHeal.Patches
{ 
   [HarmonyPatch(typeof(Tk2dPlayAnimation), "DoPlayAnimation")]
class Patch_DoPlayAnimation
{
    static bool Prefix(Tk2dPlayAnimation __instance)
    {
        return true; // TEMP DISABLE

        if (Conf.GetHealDurationMode() != Conf.HealDurationModeEnum.Custom)
                return true; // ✅ only modify if in Custom mode

        if (__instance == FsmCache.SpellControl.GetActionTk2dPlayAnimation()) // ✅ check specific action
        {
            CoroutineRunner.Instance.StopAllCoroutines();
            CoroutineRunner.Instance.Run(FinishAfterDelay(Conf.GetHealDurationMs() / 1000f));
            PluginLogger.LogInfo($"[DoPlayAnimation Prefix] Scheduled FinishAfterDelay for {Conf.GetHealDurationMs()} ms");
        }
        return true;
    }

    private static System.Collections.IEnumerator FinishAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (FsmCache.SpellControl.GetActionTk2dPlayAnimation() != null)
        {
            Events.ConfigHealDurationChanged.RefreshHealDuration();
            // owd.FsmCache.SpellControl.GetActionTk2dPlayAnimation().clipName.Value = "";

            // owd.FsmCache.SpellControl.GetActionTk2dPlayAnimation().Finish();
            PluginLogger.LogInfo($"[FinishAfterDelay] Finished Tk2dPlayAnimation after {delay} seconds");
        }
        else
        {
            PluginLogger.LogInfo($"[FinishAfterDelay] Tk2dPlayAnimation already finished or inactive");
        }
    }
}

 
}

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner _instance;

    public static CoroutineRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("CoroutineRunner");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<CoroutineRunner>();
            }
            return _instance;
        }
    }

    public void Run(IEnumerator routine) => StartCoroutine(routine);
}




