using System.IO;
using System;
using System.Runtime.Serialization;

namespace CSync.Util;

[Serializable]
public class ByteSerializer<T> {
    [NonSerialized]
    static readonly DataContractSerializer Serializer = new(typeof(T));

    // Ensures the size of an integer is correct for the current system.
    public static int IntSize => sizeof(int);

    public static byte[] SerializeToBytes(T val) {
        using MemoryStream stream = new();

        try {
            Serializer.WriteObject(stream, val);
            return stream.ToArray();
        }
        catch (Exception e) {
            CSync.Logger.LogError($"Error serializing instance: {e}");
            return null;
        }
    }

    public static T DeserializeFromBytes(byte[] data) {
        using MemoryStream stream = new(data);

        try {
            return (T) Serializer.ReadObject(stream);
        } catch (Exception e) {
            CSync.Logger.LogError($"Error deserializing instance: {e}");
            return default;
        }
    }
}