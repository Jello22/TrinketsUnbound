# Twin Trinkets

Twin Trinkets reworks Valheim's trinkets into always-active passive equipment and allows you to equip two trinkets at the same time.

Instead of building adrenaline to activate a trinket effect, each trinket provides its own passive bonus for as long as it is equipped.

## Features

- Equip up to two trinkets at once
- Replaces trinket adrenaline effects with passive bonuses
- Each trinket has its own unique passive effect
- Passive bonuses are shown directly on item tooltips
- Prevents equipping two copies of the same trinket
- All passive values are configurable
- ServerSync support for multiplayer
- Optional AzuExtendedPlayerInventory support
- Works without an equipment inventory mod

## How It Works

Trinkets are equipped normally from your inventory.

Your first trinket becomes **Trinket I** and your second becomes **Trinket II**.

If both are occupied, equipping another trinket replaces Trinket II.

Removing Trinket I does not automatically move Trinket II into its place.

You cannot equip two copies of the same trinket at the same time.

## Passive Trinkets

Each vanilla trinket has been given a passive bonus.

Examples include:

- Increased health, stamina, or eitr regeneration
- Increased armour
- Increased movement or swim speed
- Reduced stamina costs
- Increased weapon or magic skills
- Increased physical damage
- Improved blocking and parrying
- Physical damage resistance

The exact effect and values are shown on each trinket's tooltip.

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

## Requirements

- BepInEx 5

AzuExtendedPlayerInventory is **not required**.

## Installation

1. Install BepInEx.
2. Place `TwinTrinkets.dll` in your `BepInEx/plugins` folder.
3. Start the game.

For multiplayer servers, install Twin Trinkets on both the server and connecting clients.