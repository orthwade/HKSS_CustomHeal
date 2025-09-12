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
    

    [BepInPlugin("com.orthwade.CustomHeal", "Tools Melee Recharge", "1.0.0")]
    public class CustomHeal : BaseUnityPlugin
    {
        internal static CustomHeal Instance;
        private void Awake()
        {
            Instance = this;

            PluginLog.Log = Logger;

            // ToolLibrary.Init(Config);

            // Initialize config manager
            CustomHealConfig.Init(Config);

            Logger.LogInfo("Tools Melee Recharge loaded!");

            // Apply Harmony patches
            var harmony = new Harmony("com.orthwade.CustomHeal");
            harmony.PatchAll();
        }
    }
}
