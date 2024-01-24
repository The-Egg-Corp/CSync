using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

using CSync.Core;

namespace CSync;

[BepInPlugin(Metadata.GUID, Metadata.NAME, Metadata.VERSION)]
public class Plugin : BaseUnityPlugin {
    internal static new ManualLogSource Logger { get; private set; }
    internal static Plugin Instance { get; private set; }
    //internal static Harmony Harmony;

    internal static ConfigFile ConfigFile;

    private void Awake() {
        Instance = this;
        Logger = base.Logger;

        //try {
        //    Harmony = new(Metadata.GUID);
        //    Harmony.PatchAll();

        //    Logger.LogInfo("Plugin loaded.");
        //}
        //catch (Exception e) {
        //    Logger.LogError(e);
        //}
    }

    public void RegisterConfig(ConfigFile file) {
        ConfigFile = file;
    }
}