using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;

using CSync.Core;

using System.Collections.Generic;

namespace CSync;

/// <summary>
/// The plugin and main entry point of this library.<br></br>
/// Helps with internal logging and caching of config files through static references.<br></br>
/// <br></br>
/// Do <b>NOT</b> use this class directly or create any instances of it!
/// </summary>
[BepInPlugin(Metadata.GUID, Metadata.NAME, Metadata.VERSION)]
public class CSync : BaseUnityPlugin {
    internal static new ManualLogSource Logger { get; private set; }
    internal static Dictionary<string, ConfigFile> FileCache = [];

    private void Awake() {
        Logger = base.Logger;
    }

    internal static ConfigFile GetConfigFile(string path) {
        bool exists = FileCache.TryGetValue(path, out ConfigFile cfg);
        if (!exists) {
            cfg = new(path, false);
            FileCache.Add(path, cfg);
        }

        return cfg;
    }
}