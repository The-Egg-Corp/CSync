using System;
using CSync.Util;
using Unity.Collections;
using Unity.Netcode;
using static Unity.Netcode.CustomMessagingManager;

namespace CSync.Lib;

/// <summary>
/// Wrapper class allowing the config class (type parameter) to be synchronized.<br></br>
/// Stores the mod's unique identifier and handles registering and sending of named messages.
/// </summary>
[Serializable]
public class SyncedConfig<T>(string guid) : SyncedInstance<T>, ISynchronizable where T : class {
    static void LogErr(string str) => Plugin.Logger.LogError(str);
    static void LogDebug(string str) => Plugin.Logger.LogDebug(str);

    void RegisterMessage(string name, HandleNamedMessageDelegate callback) => 
        MessageManager.RegisterNamedMessageHandler($"{GUID}_{name}", callback); 

    /// <summary>
    /// The unique name or abbreviation of the implementing mod.<br></br>
    /// After passing this to the constructor, it will never change.
    /// </summary>
    public readonly string GUID = guid;

    /// <summary>Invoked on the host when a client requests to sync.</summary>
    [field:NonSerialized] public event EventHandler SyncRequested;

    /// <summary>Invoked on the client when they receive the host config.</summary>
    [field:NonSerialized] public event EventHandler SyncReceived;

    internal void OnSyncRequested() => SyncRequested?.Invoke(this, EventArgs.Empty);
    internal void OnSyncReceived() => SyncReceived?.Invoke(this, EventArgs.Empty);

    internal bool MessagesRegistered = false;

    internal SyncedEntry<bool> SYNC_TO_CLIENTS { get; private set; }

    /// <summary>
    /// Allow the host to control whether clients can use their own config.
    /// This MUST be called after binding the entry parameter.
    /// </summary>
    /// <param name="hostSyncControlOption">The entry for the host to use in your config file.</param>
    protected void EnableHostSyncControl(SyncedEntry<bool> hostSyncControlOption) {
        SYNC_TO_CLIENTS = hostSyncControlOption;

        hostSyncControlOption.SettingChanged += (object sender, EventArgs e) => {
            SYNC_TO_CLIENTS = hostSyncControlOption;
        };
    }

    public void Resync() {
        if (!MessagesRegistered) return;
        RequestSync();
    }

    void ISynchronizable.RegisterMessages() {
        if (IsHost) {
            RegisterMessage("OnRequestConfigSync", OnRequestSync);
            return;
        }

        RegisterMessage("OnHostDisabledSyncing", OnHostDisabledSyncing);
        RegisterMessage("OnReceiveConfigSync", OnReceiveSync);

        MessagesRegistered = true;

        RequestSync();
    }

    void ISynchronizable.RequestSync() => RequestSync();

    void RequestSync() {
        if (!IsClient) return;

        using FastBufferWriter stream = new(IntSize, Allocator.Temp);

        // No ID specified, `OnRequestSync` will be called on the host.
        stream.SendMessage(GUID, "OnRequestConfigSync");
    }

    // Invoked on host
    internal void OnRequestSync(ulong clientId, FastBufferReader _) {
        if (!IsHost) return;
        OnSyncRequested();

        if (SYNC_TO_CLIENTS != null && SYNC_TO_CLIENTS == false) {
            using FastBufferWriter s = new(IntSize, Allocator.Temp);
            s.SendMessage(GUID, "OnHostDisabledSyncing", clientId);

            LogDebug($"{GUID} - The host (you) has syncing disabled! Informing other clients..");
            return;
        }

        LogDebug($"{GUID} - Config sync request received from client: {clientId}");

        byte[] array = SerializeToBytes(Instance);
        int value = array.Length;

        using FastBufferWriter stream = new(value + IntSize, Allocator.Temp);

        try {
            stream.WriteValueSafe(in value, default);
            stream.WriteBytesSafe(array);

            stream.SendMessage(GUID, "OnReceiveConfigSync", clientId);
        } catch(Exception e) {
            LogErr($"{GUID} - Error occurred syncing config with client: {clientId}\n{e}");
        }
    }

    // Invoked on client
    internal void OnReceiveSync(ulong _, FastBufferReader reader) {
        OnSyncReceived();

        if (!reader.TryBeginRead(IntSize)) {
            LogErr($"{GUID} - Config sync error: Could not begin reading buffer.");
            return;
        }

        reader.ReadValueSafe(out int val, default);
        if (!reader.TryBeginRead(val)) {
            LogErr($"{GUID} - Config sync error: Host could not sync.");
            return;
        }

        byte[] data = new byte[val];
        reader.ReadBytesSafe(ref data, val);

        try {
            SyncInstance(data);
        } catch(Exception e) {
            LogErr($"Error syncing config instance!\n{e}");
        }
    }

    internal void OnHostDisabledSyncing(ulong _, FastBufferReader reader) {
        Synced = false;
        OnSyncCompleted();

        LogDebug($"{GUID} - The host has disabled syncing. Invoking the SyncComplete event..");
    }
}
