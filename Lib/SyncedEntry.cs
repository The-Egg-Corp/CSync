using System;
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
    [NonSerialized] string ConfigFilePath;
    [NonSerialized] string Key;
    [NonSerialized] string Section;
    [NonSerialized] string Desc;
    [NonSerialized] object DefaultValue;
    [NonSerialized] object CurrentValue;

    [NonSerialized] public readonly ConfigEntry<V> Entry;

    public V Value {
        get => Entry.Value;
        set => Entry.Value = value;
    }

    // Exposes the event from the underlying ConfigEntry
    public event EventHandler SettingChanged {
        add { Entry.SettingChanged += value; }
        remove { Entry.SettingChanged -= value; }
    }

    public SyncedEntry(ConfigEntry<V> cfgEntry) {
        Entry = cfgEntry;
        Init();
    }

    // Deserialization
    SyncedEntry(SerializationInfo info, StreamingContext ctx) {
        // Reconstruct or get cached file
        ConfigFile cfg = CSync.GetConfigFile(info.GetString("ConfigFilePath"));

        // Reconstruct entry and reassign its value.
        Entry = cfg.Reconstruct<V>(info);
        Value = info.GetObject<V>("CurrentValue");

        Init();
    }

    // Serialization
    public void GetObjectData(SerializationInfo info, StreamingContext context) {
        info.AddValue("ConfigFilePath", ConfigFilePath);
        info.AddValue("Key", Key);
        info.AddValue("Section", Section);
        info.AddValue("Description", Desc);
        info.AddValue("DefaultValue", DefaultValue);
        info.AddValue("CurrentValue", CurrentValue);
    }

    public void Init() {
        ConfigFilePath = Entry.ConfigFile.ConfigFilePath;
        Key = Entry.Definition.Key;
        Section = Entry.Definition.Section;
        Desc = Entry.Description.Description;
        DefaultValue = Entry.DefaultValue;
        CurrentValue = Entry.Value;
    }
}
