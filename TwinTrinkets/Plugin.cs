using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace TwinTrinkets
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(
        "Azumatt.AzuExtendedPlayerInventory",
        BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "jello.twintrinkets";
        public const string PluginName = "Twin Trinkets";
        public const string PluginVersion = "0.2.0";

        internal static ManualLogSource Log = null!;

        private Harmony _harmony = null!;

        private void Awake()
        {
            Log = Logger;

            ModConfig.Initialize(Config);
            
            AzuEpiIntegration.Initialize();

            _harmony = new Harmony(PluginGuid);
            _harmony.PatchAll();

            Log.LogInfo(
                "Twin Trinkets loaded.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}