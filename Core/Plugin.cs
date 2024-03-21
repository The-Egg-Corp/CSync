using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;

using CSync.Patches.LethalCompany;
using CSync.Core;

namespace CSync;

/// <summary>
/// The main entry point of this library.<br></br>
/// Holds a reference to the logger and applies patches on load.
/// </summary>
[BepInPlugin(GUID, NAME, VERSION)]
public class Plugin : BaseUnityPlugin {
    internal static new ManualLogSource Logger { get; private set; }
    
    public const string GUID = $"io.github.{MyPluginInfo.PLUGIN_NAME}";
    public const string NAME = MyPluginInfo.PLUGIN_NAME;
    public const string VERSION = MyPluginInfo.PLUGIN_VERSION;

    readonly Harmony Patcher = new(GUID);
    internal static new CSyncConfig Config { get; private set; }

    private void Awake() {
        Logger = base.Logger;
        Config = new(base.Config);

        if (!Config.ENABLE_PATCHING.Value) {
            return;
        }

        var game = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", "");
        if (game == "Lethal Company" || game == "LethalCompany") {
            Logger.LogInfo("Applying Lethal Company patches.");

            try {
                Patcher.PatchAll(typeof(JoinPatch_LC));
                Patcher.PatchAll(typeof(LeavePatch_LC));
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }
}