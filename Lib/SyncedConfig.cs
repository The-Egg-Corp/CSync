using System;
using Unity.Collections;
using Unity.Netcode;

using CSync.Util;

namespace CSync.Lib;

/// <summary>
/// Wrapper class allowing the config class (type parameter) to be synchronized.<br></br>
/// Stores the mod's unique identifier and handles registering and sending of named messages.
/// </summary>
[Serializable]
public class SyncedConfig<T>(string guid) : SyncedInstance<T>, ISynchronizable where T : class {
    static void LogErr(string str) => Plugin.Logger.LogError(str);

    /// <summary>
    /// The mod name or abbreviation. After being given to the constructor, it cannot be changed.
    /// </summary>
    public readonly string GUID = guid;

    void ISynchronizable.SetupSync() {
        if (IsHost) {
            MessageManager.RegisterNamedMessageHandler($"{GUID}_OnRequestConfigSync", OnRequestSync);
            return;
        }

        MessageManager.RegisterNamedMessageHandler($"{GUID}_OnReceiveConfigSync", OnReceiveSync);
        RequestSync();
    }

    void RequestSync() {
        if (!IsClient) return;

        using FastBufferWriter stream = new(IntSize, Allocator.Temp);

        // Method `OnRequestSync` will then get called on the host.
        stream.SendMessage(GUID, "OnRequestConfigSync");
    }

    internal void OnRequestSync(ulong clientId, FastBufferReader _) {
        if (!IsHost) return;

        Plugin.Logger.LogDebug($"Config sync request received from client: {clientId}");

        byte[] array = SerializeToBytes(Instance);
        int value = array.Length;

        using FastBufferWriter stream = new(value + IntSize, Allocator.Temp);

        try {
            stream.WriteValueSafe(in value, default);
            stream.WriteBytesSafe(array);

            stream.SendMessage(GUID, "OnReceiveConfigSync", clientId);
        } catch(Exception e) {
            LogErr($"Error occurred syncing config with client: {clientId}\n{e}");
        }
    }

    internal void OnReceiveSync(ulong _, FastBufferReader reader) {
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
        } catch(Exception e) {
            LogErr($"Error syncing config instance!\n{e}");
        }
    }
}
