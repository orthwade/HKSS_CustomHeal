// using HarmonyLib;

// namespace CustomHeal.Patches
// {
//     [HarmonyPatch(typeof(HeroController))]
//     [HarmonyPatch("TakeSilk")]
//     [HarmonyPatch(new[] { typeof(int), typeof(SilkSpool.SilkTakeSource) })]
//     internal static class HeroController_TakeSilk_Patch
//     {
//         private static void Prefix(ref int amount, SilkSpool.SilkTakeSource source)
//         {

//             if (source == SilkSpool.SilkTakeSource.Normal && amount == 9)
//             {
//                 amount = CustomHealConfig.GetHealCost();
//             }
//         }

//         private static void Postfix(int amount, SilkSpool.SilkTakeSource source)
//         {
//         }
//     }
// }
