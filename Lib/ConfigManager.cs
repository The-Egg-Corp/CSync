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
/// </summary>
public class ConfigManager {
    internal static Dictionary<string, ConfigFile> FileCache = [];
    internal static Dictionary<string, ISynchronizable> Instances = [];

    internal static ConfigFile GetConfigFile(string fileName) {
        bool exists = FileCache.TryGetValue(fileName, out ConfigFile cfg);
        if (!exists) {
            string absPath = Path.Combine(Paths.ConfigPath, fileName);

            cfg = new(absPath, false);
            FileCache.Add(fileName, cfg);
        }

        return cfg;
    }

    public static void Register<T>(T config) where T : SyncedConfig<T>, ISynchronizable {
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

    internal static void SyncInstances() => Instances.Values.Do(i => i.SetupSync());
    internal static void RevertSyncedInstances() => Instances.Values.Do(i => i.RevertSync());
}

public interface ISynchronizable {
    public void SetupSync();
    public void RevertSync();
}