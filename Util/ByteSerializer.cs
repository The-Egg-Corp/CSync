using System.IO;
using System;
using System.Runtime.Serialization;

namespace CSync.Util;

public class ByteSerializer<T> {
    [NonSerialized]
    static readonly DataContractSerializer Serializer = new(typeof(T));

    public static byte[] SerializeToBytes(T val) {
        using MemoryStream stream = new();

        try {
            Serializer.WriteObject(stream, val);
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
            return (T) Serializer.ReadObject(stream);
        } catch (Exception e) {
            Plugin.Logger.LogError($"Error deserializing instance: {e}");
            return default;
        }
    }
}