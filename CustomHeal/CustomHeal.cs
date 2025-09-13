using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace CustomHeal
{
    internal static class PluginLog
    {
        internal static ManualLogSource Log;
    }
    

    [BepInPlugin("com.orthwade.CustomHeal", "Custom Heal", "1.0.0")]
    public class CustomHeal : BaseUnityPlugin
    {
        internal static CustomHeal Instance;
        private void Awake()
        {
            Instance = this;

            PluginLog.Log = Logger;


            CustomHealConfig.Init(Config);

            Logger.LogInfo("Custom Heal loaded!");

            var harmony = new Harmony("com.orthwade.CustomHeal");
            harmony.PatchAll();
        }
    }
}
