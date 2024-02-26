using BepInEx.Configuration;
using BepInEx;
using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using System.Linq;

namespace CSync.Lib;

/// <summary>
/// Helper class enabling the user to easily setup CSync.<br></br>
/// Handles config registration, instance syncing and caching of BepInEx files.<br></br>
/// <br></br>
/// Reference this class in a static manner, do <b>NOT</b> create any instances of it!
/// </summary>
public class ConfigManager {
    internal static Dictionary<string, ConfigFile> FileCache = [];
    internal static Dictionary<string, object> Instances = [];

    internal static ConfigFile GetConfigFile(string fileName) {
        bool exists = FileCache.TryGetValue(fileName, out ConfigFile cfg);
        if (!exists) {
            string absPath = Path.Combine(Paths.ConfigPath, fileName);

            cfg = new(absPath, false);
            FileCache.Add(fileName, cfg);
        }

        return cfg;
    }

    public static void Register<T>(T config) where T : SyncedConfig<T> {
        string guid = config.GUID;

        if (config == null) {
            Plugin.Logger.LogError($"An error occurred registering config: {guid}\nConfig instance cannot be null!");
        }

        if (Instances.ContainsKey(guid)) {
            Plugin.Logger.LogWarning($"Attempted to register config `{guid}` after it has already been registered!");
            return;
        }

        config.InitInstance(config);
        Instances.Add(guid, config);
    }

    public static void Unregister(string modGuid) {
        Instances.Remove(modGuid);
    }

    internal static void SyncInstances() => Instances.Values.OfType<SyncedConfig<object>>().Do(i => i.SetupSync());
    internal static void RevertSyncedInstances() => Instances.Values.OfType<SyncedConfig<object>>().Do(i => i.RevertSync());
}