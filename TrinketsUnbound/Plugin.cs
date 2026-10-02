using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace TrinketsUnbound
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency(
        "Azumatt.AzuExtendedPlayerInventory",
        BepInDependency.DependencyFlags.SoftDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "jello.trinketsunbound";
        public const string PluginName = "Trinkets Unbound";
        public const string PluginVersion = "0.2.0";

        internal static ManualLogSource Log = null!;
        private Harmony? _harmony;
        private void Awake()
        {
            Log = Logger;

            ModConfig.Initialize(Config);
            
            AzuEpiIntegration.Initialize();

            _harmony = new Harmony(PluginGuid);
            _harmony.PatchAll(typeof(Plugin).Assembly);

            Log.LogInfo(
                "Trinkets Unbound loaded.");
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
        }
    }
}