# Trinkets Unbound

Trinkets Unbound reworks Valheim's trinkets into passive equipment and allows you to equip two trinkets at the same time.  Inspired by the trinkets from WoW and the fact that adrenaline 
system just felt meh to me.

No more building adrenaline to activate a trinket effect, each trinket provides its own passive bonus based off the original effects.  


With the end goal of being able to pick the two trinkets that best fit your play style without being two overpowered or tanky.  Or configure it so it does just that if you want.

## THIS IS MOD IS A WORK IN PROGRESS CURRENTLY DOES NOT SUPPORT MODDED TRINKETS

## Usage

-  Trinkets are equipped normally from your inventory.  Craft a trinket see the effects in the tooltip or compendium.

-  Your first trinket uses the normal vanilla **Trinket slot** and your second becomes **Trinket II**.

-  If both are occupied, equipping another trinket replaces Trinket II.

-  Removing Trinket I does not automatically move Trinket II into its place.

- You cannot equip two copies of the same trinket at the same time.

-  Compatible with [AzuEpi](https://valheim.hexium.gg/mods/Azumatt/AzuExtendedPlayerInventory) not tested with other equipment managers could work but that would be on you to figure out.
   AzuEPI is an optional mod again this will function with the vanilla inventory.
## Default Passive Values

Each vanilla trinket has been given a passive bonus.  The bonus is shown on the tooltip as well as compendium.  These are the default values and first pass
I am not claiming that these are not all balanced some may be over or under powered.

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

Trinkets Unbound uses ServerSync.  Both server and clients need the mod installed

## Installation
- Install with Gale Mod Manager!

Manual installation
1. Install BepInEx.
2. Place `TrinketsUnbound.dll` in your `BepInEx/plugins` folder.
3. Start the game.

For multiplayer servers, install Trinkets Unbound on both the server and connecting clients.

## Thanks
Big thanks to Azumatt for the AzuEPI and the API that goes along with it.  Also thanks for his docuemtation, videos, and other resources that helped me alogn the way

Shoutout to a lot of the other folks at Odin+

