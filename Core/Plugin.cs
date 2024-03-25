using System;
using System.Diagnostics;
using BepInEx;
using BepInEx.Logging;
using CSync.Core;
using CSync.Patches.LethalCompany;
using HarmonyLib;

namespace CSync;

/// <summary>
/// The main entry point of this library.<br></br>
/// Holds a reference to the logger and applies patches on load.
/// </summary>
[BepInPlugin(GUID, NAME, VERSION)]
public class Plugin : BaseUnityPlugin {
    public const string GUID = $"io.github.{MyPluginInfo.PLUGIN_NAME}";
    public const string NAME = MyPluginInfo.PLUGIN_NAME;
    public const string VERSION = MyPluginInfo.PLUGIN_VERSION;

    internal static new ManualLogSource Logger { get; private set; }
    internal static new CSyncConfig Config { get; private set; }

    Harmony Patcher;

    private void Awake() {
        Logger = base.Logger;
        Config = new(base.Config);

        if (!Config.ENABLE_PATCHING.Value) {
            return;
        }

        Patcher = new(GUID);

        using var process = Process.GetCurrentProcess();
        var game = process.MainModule.ModuleName.Replace(".exe", "");

        if (game == "Lethal Company" || game == "LethalCompany") {
            Logger.LogInfo("Applying Lethal Company patches.");

            try {
                Patcher.PatchAll(typeof(NetworkManagerPatch));
                //Patcher.PatchAll(typeof(LethalConfigPatch));
            }
            catch (Exception e) {
                Logger.LogError(e);
            }
        }
    }
}