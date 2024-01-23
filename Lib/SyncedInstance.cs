using CSync.Util;
using System;
using Unity.Netcode;

namespace CSync.Lib;

[Serializable]
public class SyncedInstance<T> : ByteSerializer<T> {
    public static CustomMessagingManager MessageManager => NetworkManager.Singleton.CustomMessagingManager;
    public static bool IsClient => NetworkManager.Singleton.IsClient;
    public static bool IsHost => NetworkManager.Singleton.IsHost;

    public static T Default { get; private set; }
    public static T Instance { get; private set; }

    public static bool Synced;
    
    public void InitInstance(T instance) {
        Default = instance;
        Instance = instance;
    }
    
    public static void SyncInstance(byte[] data) {
        Instance = DeserializeFromBytes(data);
        Synced = true;
    }

    public static void RevertSync() {
        Instance = Default;
        Synced = false;
    }
}