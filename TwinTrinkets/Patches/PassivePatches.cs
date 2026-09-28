using HarmonyLib;

namespace TwinTrinkets
{
    internal static class PassivePatches
    {
        [HarmonyPatch(
            typeof(ObjectDB),
            nameof(ObjectDB.CopyOtherDB))]
        private static class ObjectDBCopyOtherDBPatch
        {
            private static void Postfix(
                ObjectDB __instance)
            {
                TrinketDefinitions.DisableVanillaAdrenaline(
                    __instance);
            }
        }

        /*
         * Player.EquipInventoryItems is Valheim's equipment restoration
         * lifecycle used when inventory equipment is rebuilt after load.
         * Reconcile once after restoration rather than polling Player.Update.
         */
        [HarmonyPatch(
            typeof(Player),
            nameof(Player.EquipInventoryItems))]
        private static class EquipInventoryItemsPatch
        {
            private static void Postfix(
                Player __instance)
            {
                if (__instance != Player.m_localPlayer)
                    return;

                PassiveTrinketManager.Refresh(
                    __instance);
            }
        }
    }
}