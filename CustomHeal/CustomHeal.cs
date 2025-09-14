using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace CustomHeal
{
    [BepInPlugin("com.orthwade.CustomHeal", "Custom Heal", "1.0.0")]
    public class CustomHeal : BaseUnityPlugin
    {
        internal static CustomHeal Instance;
        private void Awake()
        {
            Instance = this;

            CustomHealLogger.Init();

            CustomHealConfig.Init(Config);

            CustomHealLogger.LogInfo("Custom Heal loaded!");

            var harmony = new Harmony("com.orthwade.CustomHeal");
            harmony.PatchAll();
        }
    }
}
