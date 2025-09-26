using HarmonyLib;
using UnityEngine;
using HutongGames.PlayMaker.Actions;
using CustomHeal;
using System.Collections;
using System.Diagnostics;

namespace CustomHeal.Patches
{ 
   [HarmonyPatch(typeof(Tk2dPlayAnimation), "DoPlayAnimation")]
class Patch_DoPlayAnimation
{
    static bool Prefix(Tk2dPlayAnimation __instance)
    {
        if (__instance == owd.FsmCache.SpellControl.GetActionTk2dPlayAnimation()) // ✅ check specific action
        {
            CoroutineRunner.Instance.Run(FinishAfterDelay(CustomHealConfig.GetHealDurationMs() / 1000f));
            PluginLogger.LogInfo($"[DoPlayAnimation Prefix] Scheduled FinishAfterDelay for {CustomHealConfig.GetHealDurationMs()} ms");
        }
        return true;
    }

    private static System.Collections.IEnumerator FinishAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (owd.FsmCache.SpellControl.GetActionTk2dPlayAnimation() != null)
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

    [HarmonyPatch(typeof(HutongGames.PlayMaker.FsmStateAction), nameof(HutongGames.PlayMaker.FsmStateAction.Finish))]
    class Patch_Finish
    {
        static bool Prefix(HutongGames.PlayMaker.FsmStateAction __instance)
        {
            if (__instance is HutongGames.PlayMaker.Actions.Tk2dPlayAnimation tk2dAnim &&
                __instance == owd.FsmCache.SpellControl.GetActionTk2dPlayAnimation())
            {
                CoroutineRunner.Instance.StopAllCoroutines();
                PluginLogger.LogInfo("[Finish Prefix] Stopped all coroutines to prevent multiple finishes");
            }
            return true;
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




