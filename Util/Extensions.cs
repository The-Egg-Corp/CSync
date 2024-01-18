using BepInEx.Configuration;
using CSync.Lib;
using Unity.Netcode;

namespace CSync.Util;

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

    public static T BindPrimitive<T>(this ConfigFile cfg, string section, string key, T defaultVal, string desc) {
        return cfg.BindEntry(section, key, defaultVal, desc).Value;
    }

    public static ConfigEntry<T> BindEntry<T>(this ConfigFile cfg, string section, string key, T defaultVal, string desc) {
        return cfg.Bind(section, key, defaultVal, desc);
    }

    public static SyncedEntry<T> ToSyncedEntry<T>(this ConfigEntry<T> entry) {
        return new SyncedEntry<T>(entry);
    }
}