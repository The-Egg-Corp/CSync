using System;
using CSync.Util;
using Unity.Netcode;

namespace CSync.Lib;

/// <summary>
/// Generic class that can be serialized to bytes.<br></br>
/// Handles syncing and reverting as well as holding references to the client-side and synchronized instances.<br></br>
/// <br></br><br></br>
/// This class should always be inherited from, never use it directly!
/// </summary>
[Serializable]
public class SyncedInstance<T> : ByteSerializer<T> where T : class {
    public static CustomMessagingManager MessageManager => NetworkManager.Singleton.CustomMessagingManager;
    public static bool IsClient => NetworkManager.Singleton.IsClient;
    public static bool IsHost => NetworkManager.Singleton.IsHost;

    /// <summary>
    /// The instance of the class used to fall back to when reverting.<br></br>
    /// All of the properties on this instance are unsynced and always the same.
    /// </summary>
    public static T Default { get; private set; }

    /// <summary>
    /// The current instance of the class.<br></br>
    /// Properties contained in this instance can be synchronized.
    /// </summary>
    public static T Instance { get; private set; }

    /// <summary>Invoked when deserialization of data has finished and <see cref="Instance"/> is assigned to.</summary>
    [field:NonSerialized] public Action<bool> SyncComplete;
    internal void OnSyncCompleted(bool success) => SyncComplete?.Invoke(success);

    /// <summary>Invoked when <see cref="Instance"/> is set back to <see cref="Default"/> and no longer synced.</summary>
    [field:NonSerialized] public Action SyncReverted;
    internal void OnSyncReverted() => SyncReverted?.Invoke();
   
    /// <summary>Assigns both the default and current instances to the inputted instance.</summary>
    public void InitInstance(T instance) {
        Default = instance;
        Instance = instance;
    }
    
    public void SyncInstance(byte[] data) {
        Instance = DeserializeFromBytes(data);
        OnSyncCompleted(Instance != default(T));
    }

    public void RevertSync() {
        Instance = Default;
        OnSyncReverted();
    }
}