using BepInEx;
using HarmonyLib;

namespace owd.CustomHeal
{
    [BepInPlugin("com.orthwade.CustomHeal", "Custom Heal", "1.1.0")]
    public class CustomHeal : BaseUnityPlugin
    {
        internal static CustomHeal Instance;
        private void Awake()
        {
            Instance = this;

            PluginLogger.Init(Config);

            Conf.Init(Config);

            PluginLogger.LogInfo("Custom Heal loaded!");

            var harmony = new Harmony("com.orthwade.CustomHeal");
            harmony.PatchAll();

            // FSMScanner.Scan();
        }
    }
}
