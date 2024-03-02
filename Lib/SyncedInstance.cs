using CSync.Util;
using System;
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

    /// <summary>
    /// Invoked when deserialization of data has finished and <see cref="Instance"/> is assigned to.
    /// </summary>
    [field:NonSerialized] public event EventHandler SyncComplete;
    void OnSyncCompleted() => SyncComplete?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// Invoked when <see cref="Instance"/> is set back to <see cref="Default"/> and no longer synced.
    /// </summary>
    [field:NonSerialized] public event EventHandler SyncReverted;
    void OnSyncReverted() => SyncReverted?.Invoke(this, EventArgs.Empty);
    
    public static bool Synced;

    public void InitInstance(T instance) {
        Default = instance;
        Instance = instance;
    }
    
    public void SyncInstance(byte[] data) {
        Instance = DeserializeFromBytes(data);
        Synced = Instance != default(T);

        OnSyncCompleted();
    }

    public void RevertSync() {
        Instance = Default;
        Synced = false;

        OnSyncReverted();
    }
}