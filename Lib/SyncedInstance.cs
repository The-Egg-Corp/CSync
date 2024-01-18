using System;
using System.IO;
using System.Runtime.Serialization;
using Unity.Netcode;

namespace CSync.Lib;

public static class NetcodeExtensions {
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
}

[Serializable]
public class SyncedInstance<T> {
    public static bool IsClient => NetworkManager.Singleton.IsClient;
    public static bool IsHost => NetworkManager.Singleton.IsHost;

    [NonSerialized] protected static int IntSize = 4;
    [NonSerialized] static readonly DataContractSerializer serializer = new(typeof(T));

    internal static T Default { get; private set; }
    internal static T Instance { get; private set; }

    internal static bool Synced;
    
    protected void InitInstance(T instance) {
        Default = instance;
        Instance = instance;
        
        // Ensures the size of an integer is correct for the current system.
        IntSize = sizeof(int);
    }
    
    internal static void SyncInstance(byte[] data) {
        Instance = DeserializeFromBytes(data);
        Synced = true;
    }

    internal static void RevertSync() {
        Instance = Default;
        Synced = false;
    }

    public static byte[] SerializeToBytes(T val) {
        using MemoryStream stream = new();

        try {
            serializer.WriteObject(stream, val);
            return stream.ToArray();
        }
        catch (Exception e) {
            Plugin.Logger.LogError($"Error serializing instance: {e}");
            return null;
        }
    }

    public static T DeserializeFromBytes(byte[] data) {
        using MemoryStream stream = new(data);

        try {
            return (T) serializer.ReadObject(stream);
        } catch (Exception e) {
            Plugin.Logger.LogError($"Error deserializing instance: {e}");
            return default;
        }
    }
}