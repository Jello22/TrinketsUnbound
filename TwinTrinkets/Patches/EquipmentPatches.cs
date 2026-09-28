using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace TwinTrinkets

{
    internal static class EquipmentPatches
    {
        /*
         * Sentinel ItemType used only while vanilla EquipItem is deciding
         * which equipment ownership field should receive the item.
         *
         * The actual ItemData remains ItemType.Trinket.
         */
        private static readonly ItemDrop.ItemData.ItemType CustomAssignmentType =
            (ItemDrop.ItemData.ItemType)767;

        [ThreadStatic]
        private static EquipState? _currentEquip;

        private sealed class EquipState
        {
            internal EquipState? Previous;
            internal Player? Player;
            internal ItemDrop.ItemData? Item;

            internal bool SelectionResolved;
            internal bool IsSecondTrinket;
        }
        
        /*
         * ============================================================
         * Humanoid.UnequipAllItems
         * ============================================================
         *
         * Vanilla only knows about its own equipment ownership fields.
         * Trinket II is owned by TrinketState, so explicitly feed it
         * through the normal UnequipItem lifecycle whenever vanilla
         * strips all equipment.
         */

        [HarmonyPatch(
            typeof(Humanoid),
            nameof(Humanoid.UnequipAllItems))]
        private static class UnequipAllItemsPatch
        {
            private static void Prefix(
                Humanoid __instance)
            {
                if (__instance is not Player player)
                    return;

                ItemDrop.ItemData? second =
                    TrinketState.GetSecond(
                        player);

                if (second == null)
                    return;

                Plugin.Log.LogInfo(
                    $"UNEQUIP ALL | Trinket II: " +
                    $"{GetName(second)}");

                player.UnequipItem(
                    second,
                    false);
            }
        }

        /*
         * ============================================================
         * Humanoid.EquipItem
         * ============================================================
         */

        [HarmonyPatch(
            typeof(Humanoid),
            nameof(Humanoid.EquipItem))]
        private static class EquipItemPatch
        {
            private static bool Prefix(
                Humanoid __instance,
                ItemDrop.ItemData item,
                out EquipState __state,
                ref bool __result)
            {
                __state = new EquipState
                {
                    Previous = _currentEquip,
                    Player = __instance as Player,
                    Item = item
                };

                _currentEquip = __state;

                Player? player = __state.Player;

                if (player != null &&
                    item != null &&
                    item.m_shared.m_itemType ==
                    ItemDrop.ItemData.ItemType.Trinket)
                {
                    /*
                     * Resolve the destination once for this EquipItem call.
                     *
                     * Standalone rule:
                     *
                     * Trinket I empty    -> Trinket I
                     * Trinket I occupied -> Trinket II
                     *
                     * If Trinket II is already occupied, it will be
                     * replaced after vanilla finishes the equip operation.
                     */
                    ResolveSelection(
                        __state,
                        player);

                    ItemDrop.ItemData? other =
                        __state.IsSecondTrinket
                            ? player.m_trinketItem
                            : TrinketState.GetSecond(player);

                    /*
                     * Do not allow the same trinket prefab to occupy
                     * Trinket I and Trinket II simultaneously.
                     */
                    if (IsSameTrinket(
                            item,
                            other))
                    {
                        Plugin.Log.LogInfo(
                            $"DUPLICATE BLOCKED | " +
                            $"{GetName(item)} | " +
                            $"destination=" +
                            $"{(__state.IsSecondTrinket ? "Trinket II" : "Trinket I")}");

                        __result = false;

                        _currentEquip =
                            __state.Previous;

                        return false;
                    }
                }

                return true;
            }

            /*
             * Resolve which logical trinket position this EquipItem call
             * should use.
             *
             * Trinket I remains vanilla's m_trinketItem.
             * Trinket II is owned by TrinketState.
             */
            private static void ResolveSelection(
                EquipState state,
                Player player)
            {
                if (state.SelectionResolved)
                    return;

                state.SelectionResolved = true;

                /*
                 * If AzuEPI is present and the item is physically in either of its
                 * trinket equipment cells, honor that explicit destination.
                 *
                 * Otherwise retain the proven standalone rule:
                 *
                 * Trinket I empty    -> Trinket I
                 * Trinket I occupied -> Trinket II
                 */
                if (state.Item != null &&
                    AzuEpiIntegration.TryResolveDestination(
                        player,
                        state.Item,
                        out bool isSecondTrinket))
                {
                    state.IsSecondTrinket =
                        isSecondTrinket;

                    return;
                }

                state.IsSecondTrinket =
                    player.m_trinketItem != null;
            }


            /*
             * Called ONLY for m_itemType reads belonging to vanilla's
             * equipment assignment dispatch.
             *
             * Eligibility checks continue seeing the item's real
             * ItemType.Trinket value.
             */
            private static ItemDrop.ItemData.ItemType GetAssignmentType(
                ItemDrop.ItemData.ItemType originalType,
                Humanoid humanoid,
                ItemDrop.ItemData item)
            {
                EquipState? state =
                    _currentEquip;

                if (state == null ||
                    !ReferenceEquals(
                        state.Player,
                        humanoid) ||
                    !ReferenceEquals(
                        state.Item,
                        item) ||
                    state.Player == null)
                {
                    return originalType;
                }

                if (originalType !=
                    ItemDrop.ItemData.ItemType.Trinket)
                {
                    return originalType;
                }

                ResolveSelection(
                    state,
                    state.Player);

                return state.IsSecondTrinket
                    ? CustomAssignmentType
                    : originalType;
            }

            /*
             * Preserve vanilla's eligibility checks and alter only the
             * ItemType reads used by the equipment assignment dispatch.
             *
             * This is the same hardened boundary used by our previously
             * tested ownership implementation.
             */
            private static IEnumerable<CodeInstruction> Transpiler(
                IEnumerable<CodeInstruction> instructions)
            {
                List<CodeInstruction> code =
                    instructions.ToList();

                MethodInfo editorGetter =
                    AccessTools.PropertyGetter(
                        typeof(Application),
                        nameof(Application.isEditor));

                FieldInfo sharedData =
                    AccessTools.Field(
                        typeof(ItemDrop.ItemData),
                        nameof(ItemDrop.ItemData.m_shared));

                FieldInfo itemType =
                    AccessTools.Field(
                        typeof(ItemDrop.ItemData.SharedData),
                        nameof(ItemDrop.ItemData.SharedData.m_itemType));

                MethodInfo assignmentType =
                    AccessTools.Method(
                        typeof(EquipItemPatch),
                        nameof(GetAssignmentType));

                /*
                 * Application.isEditor marks the end of the eligibility
                 * portion of the current Humanoid.EquipItem layout.
                 */
                int eligibilityEnd =
                    code.FindIndex(
                        instruction =>
                            instruction.Calls(
                                editorGetter));

                if (eligibilityEnd < 0 ||
                    !code
                        .Take(eligibilityEnd)
                        .Any(
                            instruction =>
                                instruction.LoadsField(
                                    itemType)))
                {
                    throw new InvalidOperationException(
                        "Twin Trinkets: unsupported " +
                        "Humanoid.EquipItem eligibility layout.");
                }

                /*
                 * Find only:
                 *
                 *     ldarg.1
                 *     ldfld ItemData.m_shared
                 *     ldfld SharedData.m_itemType
                 *
                 * after the eligibility section.
                 */
                HashSet<int> assignmentReads =
                    new HashSet<int>();

                for (int i = eligibilityEnd + 1;
                     i < code.Count;
                     ++i)
                {
                    if (i < 2)
                        continue;

                    if (code[i].LoadsField(itemType) &&
                        code[i - 1].LoadsField(sharedData) &&
                        code[i - 2].opcode ==
                        OpCodes.Ldarg_1)
                    {
                        assignmentReads.Add(i);
                    }
                }

                if (assignmentReads.Count == 0)
                {
                    throw new InvalidOperationException(
                        "Twin Trinkets: Humanoid.EquipItem " +
                        "assignment reads were not found.");
                }

                Plugin.Log.LogInfo(
                    "Twin Trinkets: EquipItem transpiler " +
                    $"patched {assignmentReads.Count} " +
                    "assignment read(s).");

                for (int i = 0;
                     i < code.Count;
                     ++i)
                {
                    yield return code[i];

                    if (!assignmentReads.Contains(i))
                        continue;

                    yield return new CodeInstruction(
                        OpCodes.Ldarg_0);

                    yield return new CodeInstruction(
                        OpCodes.Ldarg_1);

                    yield return new CodeInstruction(
                        OpCodes.Call,
                        assignmentType);
                }
            }

            private static void Postfix(
                Humanoid __instance,
                ItemDrop.ItemData item,
                bool triggerEquipEffects,
                EquipState __state,
                ref bool __result)
            {
                try
                {
                    if (__state.Player == null ||
                        item == null)
                    {
                        return;
                    }

                    Player player =
                        __state.Player;

                    if (!__result ||
                        item.m_shared.m_itemType !=
                        ItemDrop.ItemData.ItemType.Trinket)
                    {
                        return;
                    }

                    /*
                     * ------------------------------------------------
                     * Trinket II
                     * ------------------------------------------------
                     */
                    if (__state.IsSecondTrinket)
                    {
                        ItemDrop.ItemData? previous =
                            TrinketState.GetSecond(
                                player);

                        /*
                         * Replacing Trinket II should use vanilla's
                         * normal unequip lifecycle.
                         *
                         * Our UnequipItem patch clears TrinketState.
                         */
                        if (previous != null &&
                            !ReferenceEquals(
                                previous,
                                item))
                        {
                            player.UnequipItem(
                                previous,
                                triggerEquipEffects);
                        }

                        TrinketState.SetSecond(
                            player,
                            item);

                        /*
                         * Vanilla's common EquipItem path normally sets
                         * this as well. Keep it explicit because this item
                         * intentionally bypassed vanilla's Trinket
                         * assignment branch.
                         */
                        item.m_equipped = true;

                        __instance.SetupEquipment();

                        Plugin.Log.LogInfo(
                            $"OWNERSHIP | Trinket II equipped: " +
                            $"{GetName(item)}");

                        LogCurrentState(
                            player);

                        PassiveTrinketManager.Refresh(
                            player);

                        return;
                    }

                    /*
                     * ------------------------------------------------
                     * Trinket I
                     * ------------------------------------------------
                     *
                     * Vanilla owns Trinket I through m_trinketItem.
                     */
                    if (ReferenceEquals(
                            player.m_trinketItem,
                            item))
                    {
                        Plugin.Log.LogInfo(
                            $"OWNERSHIP | Trinket I equipped: " +
                            $"{GetName(item)}");

                        LogCurrentState(
                            player);

                        PassiveTrinketManager.Refresh(
                            player);
                    }
                }
                finally
                {
                    _currentEquip =
                        __state.Previous;
                }
            }

            /*
             * Restore thread-local state even if EquipItem throws.
             */
            private static Exception? Finalizer(
                Exception? __exception,
                EquipState __state)
            {
                _currentEquip =
                    __state.Previous;

                return __exception;
            }
        }
        
        
        /*
         * ============================================================
         * Humanoid.IsItemEquiped
         * ============================================================
         *
         * Vanilla knows about Trinket I through m_trinketItem.
         *
         * Teach vanilla that our Trinket II owner also counts as
         * equipped.
         */

        [HarmonyPatch(
            typeof(Humanoid),
            nameof(Humanoid.IsItemEquiped))]
        private static class IsItemEquipedPatch
        {
            private static void Postfix(
                Humanoid __instance,
                ItemDrop.ItemData item,
                ref bool __result)
            {
                if (__result)
                    return;

                if (__instance is not Player player)
                    return;

                if (TrinketState.IsSecond(
                        player,
                        item))
                {
                    __result = true;
                }
            }
        }

        /*
         * ============================================================
         * Player.GetEquipmentMaxAdrenaline
         * ============================================================
         *
         * Twin Trinkets converts trinkets from adrenaline-triggered
         * active effects into passive effects.
         */

        [HarmonyPatch(
            typeof(Player),
            nameof(Player.GetEquipmentMaxAdrenaline))]
        private static class GetEquipmentMaxAdrenalinePatch
        {
            private static bool Prefix(
                ref float __result)
            {
                __result = 0f;
                return false;
            }
        }

        /*
         * ============================================================
         * Humanoid.UnequipItem
         * ============================================================
         *
         * Vanilla handles m_equipped, effects and SetupEquipment.
         * We only need to clear our additional ownership reference.
         */

        [HarmonyPatch(
            typeof(Humanoid),
            nameof(Humanoid.UnequipItem))]
        private static class UnequipItemPatch
        {
            private static void Postfix(
                Humanoid __instance,
                ItemDrop.ItemData item)
            {
                if (__instance is not Player player ||
                    item == null)
                {
                    return;
                }

                /*
                 * Trinket II is our equipment owner.
                 */
                if (TrinketState.IsSecond(
                        player,
                        item))
                {
                    TrinketState.SetSecond(
                        player,
                        null);

                    /*
                     * Vanilla normally reaches its common unequip code
                     * because IsItemEquiped recognizes Trinket II.
                     *
                     * Keep this explicit for ownership consistency.
                     */
                    item.m_equipped = false;

                    __instance.SetupEquipment();

                    Plugin.Log.LogInfo(
                        $"OWNERSHIP | Trinket II unequipped: " +
                        $"{GetName(item)} | " +
                        $"equipped={item.m_equipped}");

                    LogCurrentState(
                        player);

                    PassiveTrinketManager.Refresh(
                        player);

                    return;
                }

                if (item.m_shared.m_itemType ==
                    ItemDrop.ItemData.ItemType.Trinket)
                {
                    Plugin.Log.LogInfo(
                        $"VANILLA UNEQUIP | " +
                        $"{GetName(item)} | " +
                        $"equipped={item.m_equipped}");

                    LogCurrentState(
                        player);

                    PassiveTrinketManager.Refresh(
                        player);
                }
            }
        }

        /*
         * ============================================================
         * Duplicate comparison
         * ============================================================
         *
         * Compare prefab identity rather than ItemData reference.
         *
         * Two separate copies of the same trinket may exist in the
         * inventory, but they may not occupy Trinket I and Trinket II
         * simultaneously.
         */

        private static bool IsSameTrinket(
            ItemDrop.ItemData? first,
            ItemDrop.ItemData? second)
        {
            if (first == null ||
                second == null)
            {
                return false;
            }

            /*
             * Reprocessing the same physical ItemData is not a duplicate.
             */
            if (ReferenceEquals(
                    first,
                    second))
            {
                return false;
            }

            if (first.m_dropPrefab == null ||
                second.m_dropPrefab == null)
            {
                return false;
            }

            return first.m_dropPrefab.name ==
                   second.m_dropPrefab.name;
        }

        /*
         * ============================================================
         * Diagnostics
         * ============================================================
         */

        private static void LogCurrentState(
            Player player)
        {
            ItemDrop.ItemData? first =
                player.m_trinketItem;

            ItemDrop.ItemData? second =
                TrinketState.GetSecond(
                    player);

            Plugin.Log.LogInfo(
                "OWNERSHIP STATE | " +
                $"Trinket I={GetName(first)} " +
                $"(equipped={first?.m_equipped ?? false}) | " +
                $"Trinket II={GetName(second)} " +
                $"(equipped={second?.m_equipped ?? false})");
        }

        private static string GetName(
            ItemDrop.ItemData? item)
        {
            return item?.m_shared?.m_name ??
                   "<empty>";
        }
    }
}
