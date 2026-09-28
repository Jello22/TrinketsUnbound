# Twin Trinkets

Twin Trinkets reworks Valheim's trinkets into always-active passive equipment and allows you to equip two trinkets at the same time.

No more building adrenaline to activate a trinket effect, each trinket provides its own passive bonus for as long as it is equipped.

## Features

- Equip up to two trinkets at once
- Replaces trinket adrenaline effects with passive bonuses
- Each trinket has its own unique passive effect
- Prevents equipping two copies of the same trinket
- All passive values are configurable
- ServerSync support for multiplayer
- Compatible with AzuEPI 
- Works without an equipment inventory mod

## How It Works

Trinkets are equipped normally from your inventory.

Your first trinket becomes **Trinket I** and your second becomes **Trinket II**.

If both are occupied, equipping another trinket replaces Trinket II.

Removing Trinket I does not automatically move Trinket II into its place.

You cannot equip two copies of the same trinket at the same time.

## Passive Trinkets

Each vanilla trinket has been given a passive bonus.  The bonus is shown on the tooltip as well as compenduim.

| Trinket | Passive Effect |
|---|---|
| Heart of the Forest | +15% Health Regeneration |
| Bronze Pendant | +15% Stamina Regeneration |
| Iron Brooch | +10 Armour |
| Nimble Anklet | +15% Movement Speed |
| Fins of Destiny | +25% Swim Speed, -40% Swim Stamina Usage |
| Wolf Sight | +10 Bow Skill, +10 Spear Skill, +6% Pierce Damage |
| Crystal Heart | Slightly Resistant to Blunt, Slash, and Pierce |
| Bracelets of the Brave | +10 Club Skill, +7% Blunt Damage |
| Evasion Mantle | -25% Block Stamina Usage, +25% Parry Bonus, +10 Dodge Skill |
| Pulsating Earrings | +15% Eitr Regeneration |
| Resounding Shackle | +7% Slash Damage |
| Jörmundling | +10 Elemental Magic, +10 Blood Magic |
| Brimstone | +7% Blunt, Slash, and Pierce Damage |
| Neckstabber | -10% Run Stamina Usage, -10% Attack Stamina Usage, +10 Armour, +10% Health Regeneration |
| Witch Crown | +10% Health, Stamina, and Eitr Regeneration |

## Configuration

All passive values can be adjusted through the configuration file or BepInEx Configuration Manager.

Percentage settings use normal percentage values. For example, setting a value to `15` means **15%**.

Crystal Heart's physical resistance can also be configured using Valheim's native resistance levels.

## Multiplayer

Twin Trinkets uses ServerSync.

By default, the server controls the gameplay configuration so everyone uses the same trinket values.

Server administrators can change the passive values to suit their server.

## AzuExtendedPlayerInventory

AzuExtendedPlayerInventory is **optional**.

When AzuEPI is installed, Twin Trinkets adds a dedicated **Trinket II** equipment slot alongside AzuEPI's normal trinket slot.

Without AzuEPI, Twin Trinkets works normally using Valheim's standard inventory and equipment controls.

## Installation
- Install with Gale Mod Manager!

Manual installation
1. Install BepInEx.
2. Place `TwinTrinkets.dll` in your `BepInEx/plugins` folder.
3. Start the game.

For multiplayer servers, install Twin Trinkets on both the server and connecting clients.