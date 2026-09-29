using HarmonyLib;

namespace TrinketsUnbound
{
    internal static class TooltipPatches
    {
        [HarmonyPatch(
            typeof(ItemDrop.ItemData),
            nameof(ItemDrop.ItemData.GetTooltip),
            new[]
            {
                typeof(ItemDrop.ItemData),
                typeof(int),
                typeof(bool),
                typeof(float),
                typeof(int),
                typeof(bool)
            })]
        private static class GetTooltipPatch
        {
            private static void Postfix(
                ItemDrop.ItemData item,
                bool appending,
                ref string __result)
            {
                if (item == null ||
                    appending)
                {
                    return;
                }

                if (!TrinketDefinitions.TryGetItem(
                        item,
                        out TrinketDefinition definition))
                {
                    return;
                }

                string itemTooltip = definition.ItemTooltip();

                if (string.IsNullOrEmpty(
                        itemTooltip))
                {
                    return;
                }

                __result +=
                    "\n\n<color=yellow>Passive:</color>\n" +
                    itemTooltip;
            }
        }
    }
}