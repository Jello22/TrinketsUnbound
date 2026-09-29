using UnityEngine;

namespace TrinketsUnbound
{
    internal static class PassiveTrinketManager
    {
        private static Player? _player;

        private static ItemDrop.ItemData? _firstItem;
        private static ItemDrop.ItemData? _secondItem;

        private static StatusEffect? _firstEffect;
        private static StatusEffect? _secondEffect;

        internal static void Refresh(Player player, bool force = false)
        {
            if (player == null)
                return;


            if (!ReferenceEquals(_player, player))
            {
                Reset();
                _player = player;
                force = true;
            }

            ItemDrop.ItemData? first =
                player.m_trinketItem;

            ItemDrop.ItemData? second =
                TrinketState.GetSecond(player);


            if (force ||
                !ReferenceEquals(first, _firstItem))
            {
                UpdateSlot(
                    player,
                    first,
                    ref _firstItem,
                    ref _firstEffect,
                    "I");
            }

            if (force ||
                !ReferenceEquals(second, _secondItem))
            {
                UpdateSlot(
                    player,
                    second,
                    ref _secondItem,
                    ref _secondEffect,
                    "II");
            }
        }

        internal static void RefreshLocal(bool force = false)
        {
            Player? player = Player.m_localPlayer;

            if (player == null)
                return;

            Refresh(player, force);
        }

        private static void UpdateSlot(
            Player player,
            ItemDrop.ItemData? newItem,
            ref ItemDrop.ItemData? trackedItem,
            ref StatusEffect? trackedEffect,
            string slotName)
        {
            SEMan seMan =
                player.GetSEMan();


            if (trackedEffect != null)
            {
                seMan.RemoveStatusEffect(
                    trackedEffect,
                    true);

                Plugin.Log.LogInfo(
                    $"PASSIVE | Trinket {slotName} removed");

                trackedEffect = null;
            }

            trackedItem = newItem;

            /*
             * Empty slot.
             */
            if (newItem == null)
                return;

            /*
             * Only supported trinkets receive a passive.
             */
            if (!TrinketDefinitions.TryGet(
                    newItem,
                    out TrinketDefinition definition))
            {
                Plugin.Log.LogWarning(
                    $"PASSIVE | Trinket {slotName} unsupported: " +
                    $"{newItem.m_shared.m_name}");

                return;
            }

            /*
             * Resolve the original vanilla StatusEffect.
             */
            if (ObjectDB.instance == null)
            {
                Plugin.Log.LogWarning(
                    $"PASSIVE | ObjectDB unavailable for " +
                    $"{definition.StatusEffectName}");

                return;
            }

            StatusEffect source =
                ObjectDB.instance.GetStatusEffect(
                    definition.StatusEffectName
                        .GetStableHashCode());

            if (source == null)
            {
                Plugin.Log.LogWarning(
                    $"PASSIVE | Vanilla SE not found: " +
                    $"{definition.StatusEffectName}");

                return;
            }

            /*
             * Every currently supported passive is based on
             * native SE_Stats.
             */
            if (source is not SE_Stats)
            {
                Plugin.Log.LogWarning(
                    $"PASSIVE | {definition.StatusEffectName} " +
                    $"is not SE_Stats");

                return;
            }


            StatusEffect clone =
                Object.Instantiate(source);

            clone.m_ttl = 0f;

            if (clone is not SE_Stats stats)
            {
                Object.Destroy(clone);
                return;
            }


            definition.Configure(stats);


            StatusEffect? applied =
                seMan.AddStatusEffect(
                    clone,
                    false,
                    0,
                    0f,
                    0);

            if (applied == null)
            {
                Plugin.Log.LogWarning(
                    $"PASSIVE | Failed to apply " +
                    $"{definition.StatusEffectName}");

                Object.Destroy(clone);
                return;
            }


            applied.m_ttl = 0f;

            trackedEffect = applied;

            Plugin.Log.LogInfo(
                $"PASSIVE | Trinket {slotName} applied: " +
                $"{definition.StatusEffectName}");
        }

        internal static void Reset()
        {

            if (_player != null)
            {
                SEMan seMan =
                    _player.GetSEMan();

                if (_firstEffect != null)
                {
                    seMan.RemoveStatusEffect(
                        _firstEffect,
                        true);
                }

                if (_secondEffect != null)
                {
                    seMan.RemoveStatusEffect(
                        _secondEffect,
                        true);
                }
            }

            _firstEffect = null;
            _secondEffect = null;

            _firstItem = null;
            _secondItem = null;

            _player = null;
        }
    }
}