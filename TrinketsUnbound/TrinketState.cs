using System.Collections.Generic;

namespace TrinketsUnbound
{
    internal static class TrinketState
    {
        private static readonly Dictionary<Player, ItemDrop.ItemData>
            SecondTrinkets = new();

        internal static ItemDrop.ItemData? GetSecond(Player player)
        {
            if (player == null)
                return null;

            return SecondTrinkets.TryGetValue(
                player,
                out ItemDrop.ItemData item)
                ? item
                : null;
        }

        internal static void SetSecond(
            Player player,
            ItemDrop.ItemData? item)
        {
            if (player == null)
                return;

            if (item == null)
            {
                SecondTrinkets.Remove(player);
                return;
            }

            SecondTrinkets[player] = item;
        }

        internal static bool IsSecond(
            Player player,
            ItemDrop.ItemData item)
        {
            if (player == null || item == null)
                return false;

            return ReferenceEquals(
                GetSecond(player),
                item);
        }
    }
}