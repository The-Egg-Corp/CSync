using BepInEx.Configuration;
using CSync.Lib;
using System.Runtime.Serialization;
using Unity.Netcode;

namespace CSync.Util;

/// <summary>
/// Contains helpful extension methods to aid with synchronization and reduce code duplication.
/// </summary>
public static class Extensions {
    /// <summary>
    /// Binds an entry to this file and returns the converted synced entry.
    /// </summary>
    /// <param name="cfg">The currently selected config file.</param>
    /// <param name="section">The category that this entry should show under.</param>
    /// <param name="key">The name/identifier of this entry.</param>
    /// <param name="defaultVal">The value assigned to this entry if not changed.</param>
    /// <param name="desc">The description indicating what this entry does.</param>
    public static SyncedEntry<V> BindSyncedEntry<V>(this ConfigFile cfg, 
        string section, string key, V defaultVal, string desc
    ) {
        return cfg.Bind(section, key, defaultVal, desc).ToSyncedEntry();
    }

    public static SyncedEntry<V> BindSyncedEntry<V>(this ConfigFile cfg, 
        string section, string key, V defaultVal, ConfigDescription desc
    ) {
        return cfg.BindSyncedEntry(section, key, defaultVal, desc.Description);
    }

    public static SyncedEntry<T> BindSyncedEntry<T>(this ConfigFile cfg,
        ConfigDefinition definition, T defaultValue, ConfigDescription desc = null
    ) {
        return cfg.BindSyncedEntry(definition.Section, definition.Key, defaultValue, desc.Description);
    }

    public static SyncedEntry<T> BindSyncedEntry<T>(this ConfigFile cfg,
        ConfigDefinition definition, T defaultValue, string desc
    ) {
        return cfg.BindSyncedEntry(definition.Section, definition.Key, defaultValue, desc);
    }

    /// <summary>
    /// Converts this entry into a serializable alternative, allowing it to be synced.
    /// </summary>
    public static SyncedEntry<V> ToSyncedEntry<V>(this ConfigEntry<V> entry) {
        return new SyncedEntry<V>(entry);
    }

    /// <summary>
    /// Helper method to grab a value from SerializationInfo and cast it to the specified type.
    /// </summary>
    public static T GetObject<T>(this SerializationInfo info, string key) {
        return (T) info.GetValue(key, typeof(T));
    }

    internal static ConfigEntry<V> Reconstruct<V>(this ConfigFile cfg, SerializationInfo info) {
        ConfigDefinition definition = new(info.GetString("Section"), info.GetString("Key"));
        ConfigDescription description = new(info.GetString("Description"));

        return cfg.Bind(definition, info.GetObject<V>("DefaultValue"), description);
    }

    internal static void SendMessage(this FastBufferWriter stream, string guid, string label, ulong clientId = 0uL) {
        bool fragment = stream.Capacity > 1300;
        NetworkDelivery delivery = fragment ? NetworkDelivery.ReliableFragmentedSequenced : NetworkDelivery.Reliable;

        if (fragment) Plugin.Logger.LogDebug(
            $"{guid} - Size of stream ({stream.Capacity}) was past the max buffer size.\n" +
            "Config instance will be sent in fragments to avoid overflowing the buffer."
        );

        var msgManager = NetworkManager.Singleton.CustomMessagingManager;
        msgManager.SendNamedMessage($"{guid}_{label}", clientId, stream, delivery);
    }
}