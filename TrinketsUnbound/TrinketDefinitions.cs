using System;
using System.Collections.Generic;
using UnityEngine;

namespace TrinketsUnbound
{
    internal sealed class TrinketDefinition
    {
        internal string ItemName { get; }
        internal string StatusEffectName { get; }
        internal string Description { get; }
        internal Func<string> ItemTooltip { get; }
        internal Action<SE_Stats> Configure { get; }

        internal TrinketDefinition(
            string itemName,
            string statusEffectName,
            string description,
            Func<string> itemTooltip,
            Action<SE_Stats> configure)
        {
            ItemName = itemName;
            StatusEffectName = statusEffectName;
            Description = description;
            ItemTooltip = itemTooltip;
            Configure = configure;
        }
    }

    internal static class TrinketDefinitions
    {
        private static readonly Dictionary<string, TrinketDefinition>
            Definitions = new()
            {
                // Heart of the Forest
                ["$item_trinketbronzehealth"] =
                    new TrinketDefinition(
                        "$item_trinketbronzehealth",
                        "TrinketBronzeHealth",
                        "Increased health regeneration.",
                        () => PercentLine("Health regeneration", ModConfig.HeartHealthRegen.Value),
                        se =>
                        {
                            ClearInstantEffects(se);
                            se.m_healthRegenMultiplier = Multiplier(ModConfig.HeartHealthRegen.Value);
                        }),

                // Bronze Pendant
                ["$item_trinketbronzestamina"] =
                    new TrinketDefinition(
                        "$item_trinketbronzestamina",
                        "TrinketBronzeStamina",
                        "Increased stamina regeneration.",
                        () => PercentLine("Stamina regeneration", ModConfig.BronzeStaminaRegen.Value),
                        se =>
                        {
                            ClearInstantEffects(se);
                            se.m_staminaRegenMultiplier = Multiplier(ModConfig.BronzeStaminaRegen.Value);
                        }),

                // Iron Brooch
                ["$item_trinketironhealth"] =
                    new TrinketDefinition(
                        "$item_trinketironhealth",
                        "TrinketIronHealth",
                        "Increased armour.",
                        () => ValueLine("Armour", ModConfig.IronArmor.Value),
                        se =>
                        {
                            ClearInstantEffects(se);
                            se.m_addArmor = ModConfig.IronArmor.Value;
                        }),

                // Nimble Anklet
                ["$item_trinketironstamina"] =
                    new TrinketDefinition(
                        "$item_trinketironstamina",
                        "TrinketIronStamina",
                        "Increased movement speed.",
                        () => PercentLine("Movement speed", ModConfig.NimbleMovementSpeed.Value),
                        se =>
                        {
                            ClearInstantEffects(se);
                            se.m_speedModifier = Percent(ModConfig.NimbleMovementSpeed.Value);
                        }),

                // Fins of Destiny
                ["$item_trinketchitinswim"] =
                    new TrinketDefinition(
                        "$item_trinketchitinswim",
                        "TrinketChitinSwim",
                        "Reduced swim stamina cost and increased swim speed.",
                        () => PercentLine("Swim speed", ModConfig.FinsSwimSpeed.Value) + "\n" +
                              PercentLine("Swim stamina usage", ModConfig.FinsSwimStaminaUsage.Value),
                        se =>
                        {
                            ClearInstantEffects(se);
                            se.m_swimSpeedModifier = Percent(ModConfig.FinsSwimSpeed.Value);
                            se.m_swimStaminaUseModifier = Percent(ModConfig.FinsSwimStaminaUsage.Value);
                        }),

                // Wolf Sight
                ["$item_trinketsilverdamage"] =
                    new TrinketDefinition(
                        "$item_trinketsilverdamage",
                        "TrinketSilverDamage",
                        "Increased bow and spear skill, and increased pierce damage.",
                        () => ValueLine("Bow skill", ModConfig.WolfBowSkill.Value) + "\n" +
                              ValueLine("Spear skill", ModConfig.WolfSpearSkill.Value) + "\n" +
                              PercentLine("Pierce damage", ModConfig.WolfPierceDamage.Value),
                        se =>
                        {
                            ClearInstantEffects(se);

                            se.m_skillLevel =
                                Skills.SkillType.Bows;
                            se.m_skillLevelModifier = ModConfig.WolfBowSkill.Value;

                            se.m_skillLevel2 =
                                Skills.SkillType.Spears;
                            se.m_skillLevelModifier2 = ModConfig.WolfSpearSkill.Value;

                            se.m_percentigeDamageModifiers.m_pierce =
                                Percent(ModConfig.WolfPierceDamage.Value);
                        }),

                // Crystal Heart
                ["$item_trinketsilverresist"] =
                    new TrinketDefinition(
                        "$item_trinketsilverresist",
                        "TrinketSilverResist",
                        "Increased resistance against blunt, slash and pierce damage.",
                        () => ResistanceLine("Blunt resistance") + "\n" +
                              ResistanceLine("Slash resistance") + "\n" +
                              ResistanceLine("Pierce resistance"),
                        se =>
                        {
                            ClearInstantEffects(se);

                            ApplyPhysicalResistance(se);
                        }),

                // Bracelets of the Brave
                ["$item_trinketblackdamagedealth"] =
                    new TrinketDefinition(
                        "$item_trinketblackdamagedealth",
                        "TrinketBlackDamageHealth",
                        "Increased club skill and blunt damage.",
                        () => ValueLine("Club skill", ModConfig.BraceletsClubSkill.Value) + "\n" +
                              PercentLine("Blunt damage", ModConfig.BraceletsBluntDamage.Value),
                        se =>
                        {
                            ClearInstantEffects(se);

                            se.m_skillLevel =
                                Skills.SkillType.Clubs;
                            se.m_skillLevelModifier = ModConfig.BraceletsClubSkill.Value;

                            se.m_percentigeDamageModifiers.m_blunt =
                                Percent(ModConfig.BraceletsBluntDamage.Value);
                        }),

                // Evasion Mantle
                ["$item_trinketblackdtamina"] =
                    new TrinketDefinition(
                        "$item_trinketblackdtamina",
                        "TrinketBlackStamina",
                        "Increased dodge skill, improved parrying, and reduced block stamina cost.",
                        () => PercentLine("Block stamina usage", ModConfig.EvasionBlockStaminaUsage.Value) + "\n" +
                              PercentLine("Parry bonus", ModConfig.EvasionParryBonus.Value) + "\n" +
                              ValueLine("Dodge skill", ModConfig.EvasionDodgeSkill.Value),
                        se =>
                        {
                            ClearInstantEffects(se);

                            se.m_blockStaminaUseModifier =
                                Percent(ModConfig.EvasionBlockStaminaUsage.Value);
                            se.m_timedBlockBonus =
                                Percent(ModConfig.EvasionParryBonus.Value);

                            se.m_skillLevel =
                                Skills.SkillType.Dodge;
                            se.m_skillLevelModifier = ModConfig.EvasionDodgeSkill.Value;
                        }),

                // Pulsating Earrings
                ["$item_trinketcarapaceeitr"] =
                    new TrinketDefinition(
                        "$item_trinketcarapaceeitr",
                        "TrinketCarapaceEitr",
                        "Increased eitr regeneration.",
                        () => PercentLine("Eitr regeneration", ModConfig.PulsatingEitrRegen.Value),
                        se =>
                        {
                            ClearInstantEffects(se);
                            se.m_eitrRegenMultiplier = Multiplier(ModConfig.PulsatingEitrRegen.Value);
                        }),

                // Resounding Shackle
                ["$item_trinketscalestaminadamage"] =
                    new TrinketDefinition(
                        "$item_trinketscalestaminadamage",
                        "TrinketScaleStaminaDamage",
                        "Increased slash damage.",
                        () => PercentLine("Slash damage", ModConfig.ResoundingSlashDamage.Value),
                        se =>
                        {
                            ClearInstantEffects(se);

                            se.m_percentigeDamageModifiers.m_slash =
                                Percent(ModConfig.ResoundingSlashDamage.Value);
                        }),

                // Jörmundling
                ["$item_trinketflametaleitr"] =
                    new TrinketDefinition(
                        "$item_trinketflametaleitr",
                        "TrinketFlametalEitr",
                        "Increased elemental and blood magic skill.",
                        () => ValueLine("Elemental magic skill", ModConfig.JormundlingElementalMagicSkill.Value) + "\n" +
                              ValueLine("Blood magic skill", ModConfig.JormundlingBloodMagicSkill.Value),
                        se =>
                        {
                            ClearInstantEffects(se);

                            se.m_skillLevel =
                                Skills.SkillType.ElementalMagic;
                            se.m_skillLevelModifier = ModConfig.JormundlingElementalMagicSkill.Value;

                            se.m_skillLevel2 =
                                Skills.SkillType.BloodMagic;
                            se.m_skillLevelModifier2 = ModConfig.JormundlingBloodMagicSkill.Value;
                        }),

                // Brimstone
                ["$item_trinketflametalstaminahealth"] =
                    new TrinketDefinition(
                        "$item_trinketflametalstaminahealth",
                        "TrinketFlametalStaminaHealth",
                        "Increased blunt, slash and pierce damage.",
                        () => PercentLine("Blunt damage", ModConfig.BrimstoneBluntDamage.Value) + "\n" +
                              PercentLine("Slash damage", ModConfig.BrimstoneSlashDamage.Value) + "\n" +
                              PercentLine("Pierce damage", ModConfig.BrimstonePierceDamage.Value),
                        se =>
                        {
                            ClearInstantEffects(se);

                            se.m_percentigeDamageModifiers.m_blunt =
                                Percent(ModConfig.BrimstoneBluntDamage.Value);
                            se.m_percentigeDamageModifiers.m_slash =
                                Percent(ModConfig.BrimstoneSlashDamage.Value);
                            se.m_percentigeDamageModifiers.m_pierce =
                                Percent(ModConfig.BrimstonePierceDamage.Value);
                        }),

                // Neckstabber
                ["$item_trinketbloodgoldhealth"] =
                    new TrinketDefinition(
                        "$item_trinketbloodgoldhealth",
                        "TrinketBloodGoldHealth",
                        "Health regenerates faster, armour is increased, and running and attacking cost less stamina.",
                        () => PercentLine("Health regeneration", ModConfig.NeckstabberHealthRegen.Value) + "\n" +
                              ValueLine("Armour", ModConfig.NeckstabberArmor.Value) + "\n" +
                              PercentLine("Run stamina usage", ModConfig.NeckstabberRunStaminaUsage.Value) + "\n" +
                              PercentLine("Attack stamina usage", ModConfig.NeckstabberAttackStaminaUsage.Value),
                        se =>
                        {
                            ClearInstantEffects(se);

                            // Remove the inherited vanilla run-drain modifier
                            // so we don't apply the reduction twice.
                            se.m_runStaminaDrainModifier = 0f;

                            se.m_runStaminaUseModifier = Percent(ModConfig.NeckstabberRunStaminaUsage.Value);
                            se.m_attackStaminaUseModifier = Percent(ModConfig.NeckstabberAttackStaminaUsage.Value);
                            se.m_addArmor = ModConfig.NeckstabberArmor.Value;
                            se.m_healthRegenMultiplier = Multiplier(ModConfig.NeckstabberHealthRegen.Value);
                        }),

                // Witch Crown
                ["$item_trinketbloodgoldstamina"] =
                    new TrinketDefinition(
                        "$item_trinketbloodgoldstamina",
                        "TrinketBloodGoldStamina",
                        "Health, stamina and eitr regenerate faster.",
                        () => PercentLine("Health regeneration", ModConfig.WitchHealthRegen.Value) + "\n" +
                              PercentLine("Stamina regeneration", ModConfig.WitchStaminaRegen.Value) + "\n" +
                              PercentLine("Eitr regeneration", ModConfig.WitchEitrRegen.Value),
                        se =>
                        {
                            ClearInstantEffects(se);

                            se.m_healthRegenMultiplier = Multiplier(ModConfig.WitchHealthRegen.Value);
                            se.m_staminaRegenMultiplier = Multiplier(ModConfig.WitchStaminaRegen.Value);
                            se.m_eitrRegenMultiplier = Multiplier(ModConfig.WitchEitrRegen.Value);
                        })
            };

        internal static bool TryGet(
            ItemDrop.ItemData? item,
            out TrinketDefinition definition)
        {
            definition = null!;

            if (item == null ||
                item.m_shared == null ||
                !item.m_equipped)
            {
                return false;
            }

            return Definitions.TryGetValue(
                item.m_shared.m_name,
                out definition!);
        }

        internal static bool TryGetItem(
            ItemDrop.ItemData? item,
            out TrinketDefinition definition)
        {
            definition = null!;

            if (item == null ||
                item.m_shared == null)
            {
                return false;
            }

            return Definitions.TryGetValue(
                item.m_shared.m_name,
                out definition!);
        }

        internal static void DisableVanillaAdrenaline(
            ObjectDB objectDB)
        {
            if (objectDB == null ||
                objectDB.m_items == null)
            {
                return;
            }

            foreach (GameObject prefab in objectDB.m_items)
            {
                if (prefab == null)
                    continue;

                ItemDrop itemDrop =
                    prefab.GetComponent<ItemDrop>();

                if (itemDrop == null ||
                    itemDrop.m_itemData == null ||
                    itemDrop.m_itemData.m_shared == null)
                {
                    continue;
                }

                ItemDrop.ItemData item =
                    itemDrop.m_itemData;

                if (item.m_shared.m_itemType !=
                    ItemDrop.ItemData.ItemType.Trinket)
                {
                    continue;
                }

                item.m_shared.m_maxAdrenaline = 0f;
                item.m_shared.m_fullAdrenalineSE = null;
            }
        }

        private static float Percent(float value) => value / 100f;

        private static float Multiplier(float value) => 1f + Percent(value);

        private static string Format(float value) => value.ToString("0.##");

        private static string Signed(float value) =>
            value > 0f ? "+" + Format(value) : Format(value);

        private static string PercentLine(string label, float value) =>
            $"{label}: <color=orange>{Signed(value)}%</color>";

        private static string ValueLine(string label, float value) =>
            $"{label}: <color=orange>{Signed(value)}</color>";

        private static string ResistanceLine(string label) =>
            $"{label}: <color=orange>{ResistanceDisplayName(ModConfig.CrystalPhysicalResistance.Value)}</color>";

        private static string ResistanceDisplayName(PhysicalResistanceTier tier)
        {
            return tier switch
            {
                PhysicalResistanceTier.Normal => "Normal",
                PhysicalResistanceTier.SlightlyResistant => "Slightly resistant",
                PhysicalResistanceTier.Resistant => "Resistant",
                PhysicalResistanceTier.VeryResistant => "Very resistant",
                PhysicalResistanceTier.Immune => "Immune",
                _ => "Slightly resistant"
            };
        }

        private static HitData.DamageModifier ResistanceModifier(PhysicalResistanceTier tier)
        {
            return tier switch
            {
                PhysicalResistanceTier.Normal => HitData.DamageModifier.Normal,
                PhysicalResistanceTier.SlightlyResistant => HitData.DamageModifier.SlightlyResistant,
                PhysicalResistanceTier.Resistant => HitData.DamageModifier.Resistant,
                PhysicalResistanceTier.VeryResistant => HitData.DamageModifier.VeryResistant,
                PhysicalResistanceTier.Immune => HitData.DamageModifier.Immune,
                _ => HitData.DamageModifier.SlightlyResistant
            };
        }

        private static void ApplyPhysicalResistance(SE_Stats se)
        {
            HitData.DamageModifier modifier =
                ResistanceModifier(ModConfig.CrystalPhysicalResistance.Value);

            for (int i = 0; i < se.m_mods.Count; i++)
            {
                HitData.DamageModPair pair = se.m_mods[i];

                if (pair.m_type != HitData.DamageType.Blunt &&
                    pair.m_type != HitData.DamageType.Slash &&
                    pair.m_type != HitData.DamageType.Pierce)
                {
                    continue;
                }

                pair.m_modifier = modifier;
                se.m_mods[i] = pair;
            }
        }

        private static void ClearInstantEffects(
            SE_Stats se)
        {
            se.m_healthUpFront = 0f;
            se.m_staminaUpFront = 0f;
            se.m_eitrUpFront = 0f;
            se.m_adrenalineUpFront = 0f;
        }
    }
}