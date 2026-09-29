using AzuEPI;


namespace TrinketsUnbound
{
    internal static class AzuEpiIntegration
    {
        internal const string SecondTrinketSlotName = "Trinket II";
        private const string FirstTrinketSlotName = "$azu_epi_trinket";

        private static int _firstTrinketSlotIndex = -1;
        private static int _secondTrinketSlotIndex = -1;

        internal static bool IsActive { get; private set; }

        internal static void Initialize()
        {
            if (!API.IsLoaded())
            {
                Plugin.Log.LogInfo(
                    "AzuEPI not detected - using standalone equipment mode.");
                return;
            }

            bool added = API.AddSlot(
                SecondTrinketSlotName,
                player => TrinketState.GetSecond(player),
                item =>
                    item != null &&
                    item.m_shared != null &&
                    item.m_shared.m_itemType ==
                    ItemDrop.ItemData.ItemType.Trinket);

            if (!added)
            {
                Plugin.Log.LogWarning(
                    "AzuEPI detected, but Trinket II slot registration failed. " +
                    "Using standalone equipment behavior.");
                return;
            }

            if (!API.TryGetSlotIndexByName(
                    SecondTrinketSlotName,
                    out _secondTrinketSlotIndex))
            {
                Plugin.Log.LogWarning(
                    "AzuEPI registered Trinket II, but its slot index could not be resolved. " +
                    "Using standalone equipment behavior.");
                _secondTrinketSlotIndex = -1;
                return;
            }

            // Resolve AzuEPI's built-in vanilla trinket slot by
            // original name. Failure here is not fatal; Trinket II can still
            // be explicitly recognized and all other equips fall back to the
            // standalone Trinkets Unbound rule.
            API.TryGetSlotIndexByName(
                FirstTrinketSlotName,
                out _firstTrinketSlotIndex);

            IsActive = true;

            Plugin.Log.LogInfo(
                $"AzuEPI detected - registered {SecondTrinketSlotName} " +
                $"at slot index {_secondTrinketSlotIndex}.");
        }

       
        /// If AzuEPI can prove that the item currently occupies one of the
        /// two trinket equipment cells, return the requested logical owner.
        ///
        /// true  = AzuEPI resolved the destination
        /// false = caller should use standalone Trinkets Unbound selection
        
        internal static bool TryResolveDestination(
            Player player,
            ItemDrop.ItemData item,
            out bool isSecondTrinket)
        {
            isSecondTrinket = false;

            if (!IsActive ||
                player == null ||
                item == null)
            {
                return false;
            }

            Inventory inventory = player.GetInventory();

            if (inventory == null)
                return false;

            if (!API.TryGetSlotIndexAtGridPos(
                    inventory,
                    item.m_gridPos,
                    out int slotIndex))
            {
                return false;
            }

            if (slotIndex == _secondTrinketSlotIndex)
            {
                isSecondTrinket = true;
                return true;
            }

            if (_firstTrinketSlotIndex >= 0 &&
                slotIndex == _firstTrinketSlotIndex)
            {
                isSecondTrinket = false;
                return true;
            }

            return false;
        }
    }
}
