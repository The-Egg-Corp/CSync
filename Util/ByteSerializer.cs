using System;
using System.IO;
using System.Runtime.Serialization;

namespace CSync.Util;

/// <summary>
/// Responsible for serializing to and from bytes via a <see cref="MemoryStream"/>.<br></br>
/// Uses <see cref="DataContractSerializer"/> as a fast and safer alternative to BinaryFormatter.
/// </summary>
[Serializable]
public class ByteSerializer<T> {
    [NonSerialized]
    static readonly DataContractSerializer Serializer = new(typeof(T));

    // Ensures the size of an integer is correct for the current system.
    public static int IntSize => sizeof(int);

    public static byte[] SerializeToBytes(T val) {
        using MemoryStream stream = new();

        Serializer.WriteObject(stream, val);
        return stream.ToArray();
    }

    public static T DeserializeFromBytes(byte[] data) {
        using MemoryStream stream = new(data);

        try {
            return (T) Serializer.ReadObject(stream);
        } catch (Exception) {
            return default;
        }
    }
}