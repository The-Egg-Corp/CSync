using System;
using Unity.Collections;
using Unity.Netcode;

using CSync.Util;

namespace CSync.Lib;

[Serializable]
public class SyncedConfig<T>(string guid) : SyncedInstance<T> where T : class {
    static void LogErr(string str) => Plugin.Logger.LogError(str);

    public readonly string GUID = guid;

    [field:NonSerialized]
    public event EventHandler SyncComplete;

    void OnSyncCompleted() {
        SyncComplete?.Invoke(this, EventArgs.Empty);
    }

    public void SetupSync() {
        if (IsHost) {
            MessageManager.RegisterNamedMessageHandler($"{GUID}_OnRequestConfigSync", OnRequestSync);
            Synced = true;
            return;
        }

        Synced = false;
        MessageManager.RegisterNamedMessageHandler($"{GUID}_OnReceiveConfigSync", OnReceiveSync);
        RequestSync();
    }

    public void RequestSync() {
        if (!IsClient) return;

        using FastBufferWriter stream = new(IntSize, Allocator.Temp);

        // Method `OnRequestSync` will then get called on the host.
        stream.SendMessage($"{GUID}_OnRequestConfigSync");
    }

    public void OnRequestSync(ulong clientId, FastBufferReader _) {
        if (!IsHost) return;

        Plugin.Logger.LogDebug($"Config sync request received from client: {clientId}");

        byte[] array = SerializeToBytes(Instance);
        int value = array.Length;

        using FastBufferWriter stream = new(value + IntSize, Allocator.Temp);

        try {
            stream.WriteValueSafe(in value, default);
            stream.WriteBytesSafe(array);

            stream.SendMessage($"{GUID}_OnReceiveConfigSync", clientId);
        } catch(Exception e) {
            LogErr($"Error occurred syncing config with client: {clientId}\n{e}");
        }
    }

    public void OnReceiveSync(ulong _, FastBufferReader reader) {
        if (!reader.TryBeginRead(IntSize)) {
            LogErr("Config sync error: Could not begin reading buffer.");
            return;
        }

        reader.ReadValueSafe(out int val, default);
        if (!reader.TryBeginRead(val)) {
            LogErr("Config sync error: Host could not sync.");
            return;
        }

        byte[] data = new byte[val];
        reader.ReadBytesSafe(ref data, val);

        try {
            SyncInstance(data);
            OnSyncCompleted();
        } catch(Exception e) {
            LogErr($"Error syncing config instance!\n{e}");
        }
    }
}
