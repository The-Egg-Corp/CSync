using CSync.Util;
using System;
using System.IO;
using System.Runtime.Serialization;
using Unity.Netcode;

namespace CSync.Lib;

[Serializable]
public class SyncedInstance<T> : ByteSerializer<T> {
    public static bool IsClient => NetworkManager.Singleton.IsClient;
    public static bool IsHost => NetworkManager.Singleton.IsHost;

    [NonSerialized] 
    protected static int IntSize = 4;

    protected static T Default { get; private set; }
    protected static T Instance { get; private set; }

    protected static bool Synced;
    
    protected void InitInstance(T instance) {
        Default = instance;
        Instance = instance;
        
        // Ensures the size of an integer is correct for the current system.
        IntSize = sizeof(int);
    }
    
    protected static void SyncInstance(byte[] data) {
        Instance = Deserialize(data);
        Synced = true;
    }

    protected static void RevertSync() {
        Instance = Default;
        Synced = false;
    }
}