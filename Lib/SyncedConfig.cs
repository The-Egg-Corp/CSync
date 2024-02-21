using System;
using Unity.Collections;
using Unity.Netcode;

using CSync.Util;

namespace CSync.Lib;

[Serializable]
public class SyncedConfig<T> : SyncedInstance<T> where T : SyncedConfig<T> {
    static void LogErr(string str) => CSync.Logger.LogError(str);

    [field:NonSerialized]
    public event EventHandler SyncComplete;

    string GUID;

    public void RequestSync(string modGuid) {
        if (!IsClient) return;

        using FastBufferWriter stream = new(IntSize, Allocator.Temp);

        // Method `OnRequestSync` will then get called on host.
        GUID = modGuid;
        stream.SendMessage($"{GUID}_OnRequestConfigSync");
    }

    public static void OnRequestSync(ulong clientId, FastBufferReader _) {
        if (!IsHost) return;

        CSync.Logger.LogDebug($"Config sync request received from client: {clientId}");

        byte[] array = SerializeToBytes(Instance);
        int value = array.Length;

        using FastBufferWriter stream = new(value + IntSize, Allocator.Temp);

        try {
            stream.WriteValueSafe(in value, default);
            stream.WriteBytesSafe(array);

            stream.SendMessage($"{Instance.GUID}_OnReceiveConfigSync", clientId);
        } catch(Exception e) {
            LogErr($"Error occurred syncing config with client: {clientId}\n{e}");
        }
    }

    public static void OnReceiveSync(ulong _, FastBufferReader reader) {
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
            Instance.OnSyncCompleted();
        } catch(Exception e) {
            LogErr($"Error syncing config instance!\n{e}");
        }
    }

    void OnSyncCompleted() {
        SyncComplete?.Invoke(this, EventArgs.Empty);
    }
}
