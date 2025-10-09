using HarmonyLib;

namespace owd.CustomHeal.Patches
{
    [HarmonyPatch(typeof(InventoryItemSpool), "UpdateDisplay")]
    internal static class InventoryItemSpool_UpdateDisplay_Patch
    {
        private static int silkRegenMaxCache;
        private const string prefix = "InventoryItemSpool_UpdateDisplay_Patch Prefix: ";
        private const string postfix = "InventoryItemSpool_UpdateDisplay_Patch Postfix: ";

        private static bool Prefix(InventoryItemSpool __instance)
        {
            if (__instance == null)
            {
                PluginLogger.LogWarning(prefix + "__instance is null");
                return true; // continue to original method
            }

            PlayerData playerData = PlayerData.instance;
            if (playerData == null)
            {
                PluginLogger.LogWarning(prefix + "PlayerData.instance is null");
                return true;
            }

            // Cache and temporarily modify
            silkRegenMaxCache = playerData.silkRegenMax;
            int resolved = Conf.ResolveSilkRegenMax(playerData.silkRegenMax);

            // Optional: protect against negative or nonsense values
            if (resolved < 0)
            {
                PluginLogger.LogWarning(prefix + "Resolved silkRegenMax is negative, clamping to 0");
                resolved = 0;
            }

            playerData.silkRegenMax = resolved;
            return true;
        }

        private static void Postfix(InventoryItemSpool __instance)
        {
            if (__instance == null)
            {
                PluginLogger.LogWarning(postfix + "__instance is null (Postfix)");
                return;
            }

            PlayerData playerData = PlayerData.instance;
            if (playerData == null)
            {
                PluginLogger.LogWarning(postfix + "PlayerData.instance is null (Postfix)");
                return;
            }

            // Restore original value safely
            playerData.silkRegenMax = silkRegenMaxCache;
        }
    }
}
