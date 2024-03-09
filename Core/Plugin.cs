using BepInEx;
using BepInEx.Logging;

using HarmonyLib;
using System;

namespace CSync;

/// <summary>
/// The main entry point of this library.<br></br>
/// Holds a reference to the logger and applies patches on load.
/// </summary>
[BepInPlugin(GUID, NAME, VERSION)]
public class Plugin : BaseUnityPlugin {
    internal static new ManualLogSource Logger { get; private set; }
    
    const string GUID = MyPluginInfo.PLUGIN_GUID;
    const string NAME = MyPluginInfo.PLUGIN_NAME;
    const string VERSION = MyPluginInfo.PLUGIN_VERSION;

    Harmony Patcher;

    private void Awake() {
        Logger = base.Logger;

        try {
            Patcher = new(GUID);
            Patcher.PatchAll();

            Logger.LogInfo("CSync successfully applied patches.");
        } catch(Exception e) {
            Logger.LogError(e);
        }
    }
}