using System;
using BepInEx.Configuration;
using ServerSync;

namespace TwinTrinkets
{
    internal enum PhysicalResistanceTier
    {
        Normal,
        SlightlyResistant,
        Resistant,
        VeryResistant,
        Immune
    }

    internal static class ModConfig
    {
        private static readonly ConfigSync ConfigSync =
            new(Plugin.PluginGuid)
            {
                DisplayName = Plugin.PluginName,
                CurrentVersion = Plugin.PluginVersion,
                MinimumRequiredVersion = Plugin.PluginVersion,
                ModRequired = true
            };

        internal static ConfigEntry<bool> LockConfiguration = null!;

        internal static ConfigEntry<float> HeartHealthRegen = null!;
        internal static ConfigEntry<float> BronzeStaminaRegen = null!;
        internal static ConfigEntry<float> IronArmor = null!;
        internal static ConfigEntry<float> NimbleMovementSpeed = null!;
        internal static ConfigEntry<float> FinsSwimSpeed = null!;
        internal static ConfigEntry<float> FinsSwimStaminaUsage = null!;
        internal static ConfigEntry<float> WolfBowSkill = null!;
        internal static ConfigEntry<float> WolfSpearSkill = null!;
        internal static ConfigEntry<float> WolfPierceDamage = null!;
        internal static ConfigEntry<PhysicalResistanceTier> CrystalPhysicalResistance = null!;
        internal static ConfigEntry<float> BraceletsClubSkill = null!;
        internal static ConfigEntry<float> BraceletsBluntDamage = null!;
        internal static ConfigEntry<float> EvasionBlockStaminaUsage = null!;
        internal static ConfigEntry<float> EvasionParryBonus = null!;
        internal static ConfigEntry<float> EvasionDodgeSkill = null!;
        internal static ConfigEntry<float> PulsatingEitrRegen = null!;
        internal static ConfigEntry<float> ResoundingSlashDamage = null!;
        internal static ConfigEntry<float> JormundlingElementalMagicSkill = null!;
        internal static ConfigEntry<float> JormundlingBloodMagicSkill = null!;
        internal static ConfigEntry<float> BrimstoneBluntDamage = null!;
        internal static ConfigEntry<float> BrimstoneSlashDamage = null!;
        internal static ConfigEntry<float> BrimstonePierceDamage = null!;
        internal static ConfigEntry<float> NeckstabberHealthRegen = null!;
        internal static ConfigEntry<float> NeckstabberArmor = null!;
        internal static ConfigEntry<float> NeckstabberRunStaminaUsage = null!;
        internal static ConfigEntry<float> NeckstabberAttackStaminaUsage = null!;
        internal static ConfigEntry<float> WitchHealthRegen = null!;
        internal static ConfigEntry<float> WitchStaminaRegen = null!;
        internal static ConfigEntry<float> WitchEitrRegen = null!;

        internal static void Initialize(ConfigFile config)
        {
            LockConfiguration = config.Bind(
                "General",
                "Lock Configuration",
                true,
                "If enabled, only server administrators can change Twin Trinkets gameplay settings.");
            ConfigSync.AddLockingConfigEntry(LockConfiguration);

            HeartHealthRegen = Synced(config, "Heart of the Forest", "Health Regeneration (%)", 15f, "Percentage increase to health regeneration.");
            BronzeStaminaRegen = Synced(config, "Bronze Pendant", "Stamina Regeneration (%)", 15f, "Percentage increase to stamina regeneration.");
            IronArmor = Synced(config, "Iron Brooch", "Armor", 10f, "Flat armor added while equipped.");
            NimbleMovementSpeed = Synced(config, "Nimble Anklet", "Movement Speed (%)", 15f, "Percentage increase to movement speed.");

            FinsSwimSpeed = Synced(config, "Fins of Destiny", "Swim Speed (%)", 25f, "Percentage increase to swim speed.");
            FinsSwimStaminaUsage = Synced(config, "Fins of Destiny", "Swim Stamina Usage (%)", -40f, "Percentage modifier to swim stamina usage. Negative values reduce stamina cost.");

            WolfBowSkill = Synced(config, "Wolf Sight", "Bow Skill", 10f, "Bow skill bonus.");
            WolfSpearSkill = Synced(config, "Wolf Sight", "Spear Skill", 10f, "Spear skill bonus.");
            WolfPierceDamage = Synced(config, "Wolf Sight", "Pierce Damage (%)", 6f, "Percentage increase to pierce damage.");

            CrystalPhysicalResistance = Synced(config, "Crystal Heart", "Physical Resistance", PhysicalResistanceTier.SlightlyResistant, "Valheim resistance tier applied to blunt, slash and pierce damage.");

            BraceletsClubSkill = Synced(config, "Bracelets of the Brave", "Club Skill", 10f, "Club skill bonus.");
            BraceletsBluntDamage = Synced(config, "Bracelets of the Brave", "Blunt Damage (%)", 7f, "Percentage increase to blunt damage.");

            EvasionBlockStaminaUsage = Synced(config, "Evasion Mantle", "Block Stamina Usage (%)", -25f, "Percentage modifier to block stamina usage. Negative values reduce stamina cost.");
            EvasionParryBonus = Synced(config, "Evasion Mantle", "Parry Bonus (%)", 25f, "Percentage increase to timed-block/parry bonus.");
            EvasionDodgeSkill = Synced(config, "Evasion Mantle", "Dodge Skill", 10f, "Dodge skill bonus.");

            PulsatingEitrRegen = Synced(config, "Pulsating Earrings", "Eitr Regeneration (%)", 15f, "Percentage increase to eitr regeneration.");
            ResoundingSlashDamage = Synced(config, "Resounding Shackle", "Slash Damage (%)", 7f, "Percentage increase to slash damage.");

            JormundlingElementalMagicSkill = Synced(config, "Jörmundling", "Elemental Magic Skill", 10f, "Elemental Magic skill bonus.");
            JormundlingBloodMagicSkill = Synced(config, "Jörmundling", "Blood Magic Skill", 10f, "Blood Magic skill bonus.");

            BrimstoneBluntDamage = Synced(config, "Brimstone", "Blunt Damage (%)", 7f, "Percentage increase to blunt damage.");
            BrimstoneSlashDamage = Synced(config, "Brimstone", "Slash Damage (%)", 7f, "Percentage increase to slash damage.");
            BrimstonePierceDamage = Synced(config, "Brimstone", "Pierce Damage (%)", 7f, "Percentage increase to pierce damage.");

            NeckstabberHealthRegen = Synced(config, "Neckstabber", "Health Regeneration (%)", 10f, "Percentage increase to health regeneration.");
            NeckstabberArmor = Synced(config, "Neckstabber", "Armor", 10f, "Flat armor added while equipped.");
            NeckstabberRunStaminaUsage = Synced(config, "Neckstabber", "Run Stamina Usage (%)", -10f, "Percentage modifier to running stamina usage. Negative values reduce stamina cost.");
            NeckstabberAttackStaminaUsage = Synced(config, "Neckstabber", "Attack Stamina Usage (%)", -10f, "Percentage modifier to attack stamina usage. Negative values reduce stamina cost.");

            WitchHealthRegen = Synced(config, "Witch Crown", "Health Regeneration (%)", 10f, "Percentage increase to health regeneration.");
            WitchStaminaRegen = Synced(config, "Witch Crown", "Stamina Regeneration (%)", 10f, "Percentage increase to stamina regeneration.");
            WitchEitrRegen = Synced(config, "Witch Crown", "Eitr Regeneration (%)", 10f, "Percentage increase to eitr regeneration.");
        }

        private static ConfigEntry<T> Synced<T>(ConfigFile config, string section, string key, T defaultValue, string description)
        {
            ConfigEntry<T> entry = config.Bind(section, key, defaultValue, description);
            ConfigSync.AddConfigEntry(entry).SynchronizedConfig = true;
            entry.SettingChanged += OnGameplaySettingChanged;
            return entry;
        }

        private static void OnGameplaySettingChanged(object sender, EventArgs e)
        {
            PassiveTrinketManager.RefreshLocal(force: true);
        }
    }
}
