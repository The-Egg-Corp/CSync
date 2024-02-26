using BepInEx.Configuration;
using CSync.Lib;
using System.Runtime.Serialization;
using Unity.Netcode;

namespace CSync.Util;

/// <summary>
/// Contains helpful extension methods to aid with synchronization and reduce code duplication.
/// </summary>
public static class Extensions {
    public static void SendMessage(this FastBufferWriter stream, string label, ulong clientId = 0uL) {
        bool fragment = stream.Capacity > 1300;
        NetworkDelivery delivery = fragment ? NetworkDelivery.ReliableFragmentedSequenced : NetworkDelivery.Reliable;

        if (fragment) Plugin.Logger.LogDebug(
            $"Size of stream ({stream.Capacity}) was past the max buffer size.\n" +
            "Config instance will be sent in fragments to avoid overflowing the buffer."
        );

        var msgManager = NetworkManager.Singleton.CustomMessagingManager;
        msgManager.SendNamedMessage(label, clientId, stream, delivery);
    }

    public static V BindPrimitive<V>(this ConfigFile cfg, 
        string section, string key, V defaultVal, string desc
    ) {
        return cfg.Bind(section, key, defaultVal, desc).Value;
    }

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

    public static SyncedEntry<V> ToSyncedEntry<V>(this ConfigEntry<V> entry) {
        return new SyncedEntry<V>(entry);
    }

    public static T GetObject<T>(this SerializationInfo info, string key) {
        return (T) info.GetValue(key, typeof(T));
    }

    internal static ConfigEntry<V> Reconstruct<V>(this ConfigFile cfg, SerializationInfo info) {
        ConfigDefinition definition = new(info.GetString("Section"), info.GetString("Key"));
        ConfigDescription description = new(info.GetString("Description"));

        return cfg.Bind(definition, info.GetObject<V>("DefaultValue"), description);
    }
}