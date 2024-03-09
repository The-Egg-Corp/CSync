using System;
using System.IO;
using System.Runtime.Serialization;
using BepInEx.Configuration;
using CSync.Util;

namespace CSync.Lib;

/// <summary>
/// Wrapper class around a BepInEx <see cref="ConfigEntry{T}"/>.<br>
/// Can serialize and deserialize itself to avoid runtime errors when syncing configs.</br>
/// </summary>
[Serializable]
public class SyncedEntry<V> : ISerializable {
    [NonSerialized] public readonly ConfigEntry<V> Entry;

    string ConfigFileName => Path.GetFileName(Entry.ConfigFile.ConfigFilePath);
    public string Key => Entry.Definition.Key;
    public string Section => Entry.Definition.Section;
    public string Description => Entry.Description.Description;
    public object DefaultValue => Entry.DefaultValue;

    /// <summary>
    /// The value associated with this entry. Can be omitted in some cases.
    /// </summary>
    public V Value {
        get => Entry.Value;
        set => Entry.Value = value;
    }

    public static implicit operator V(SyncedEntry<V> e) => e.Value;

    /// <summary>
    /// Invoked whenever <see cref="Value"/> changes.
    /// </summary>
    public event EventHandler SettingChanged {
        add => Entry.SettingChanged += value;
        remove => Entry.SettingChanged -= value;
    }

    public SyncedEntry(ConfigEntry<V> cfgEntry) {
        Entry = cfgEntry;
    }

    // Deserialization
    SyncedEntry(SerializationInfo info, StreamingContext ctx) {
        // Reconstruct or get cached file
        string fileName = info.GetString("ConfigFileName");
        ConfigFile cfg = ConfigManager.GetConfigFile(fileName);

        // Reconstruct entry and reassign its value.
        Entry = cfg.Reconstruct<V>(info);
        Value = info.GetObject<V>("CurrentValue");
    }

    // Serialization
    public void GetObjectData(SerializationInfo info, StreamingContext context) {
        info.AddValue("ConfigFileName", ConfigFileName);
        info.AddValue("Key", Key);
        info.AddValue("Section", Section);
        info.AddValue("Description", Description);
        info.AddValue("DefaultValue", DefaultValue);
        info.AddValue("CurrentValue", Value);
    }

    public override string ToString() {
        return $"Key: {Key}\nDefault Value: {DefaultValue}\nCurrent Value: {Value}";
    }
}
