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
    [NonSerialized] string ConfigFileName;

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
        string fileName = info.GetString("ConfigFileName");
        ConfigFile cfg = CSync.GetConfigFile(fileName);

        // Reconstruct entry and reassign its value.
        Entry = cfg.Reconstruct<V>(info);
        Value = info.GetObject<V>("CurrentValue");

        //CSync.Logger.LogDebug($"Expected config file path: {cfg.ConfigFilePath}");
        //CSync.Logger.LogDebug($"Deserialized key: `{info.GetString("Key")}`. Value: {Value}");

        Init();
    }

    // Serialization
    public void GetObjectData(SerializationInfo info, StreamingContext context) {
        info.AddValue("ConfigFileName", ConfigFileName);
        info.AddValue("Key", Key);
        info.AddValue("Section", Section);
        info.AddValue("Description", Desc);
        info.AddValue("DefaultValue", DefaultValue);
        info.AddValue("CurrentValue", CurrentValue);

        //CSync.Logger.LogDebug($"Serialized key: `{Key}`. Value: {CurrentValue}");
    }

    public void Init() {
        // Absolute path to the config file on the current system.
        ConfigFileName = Path.GetFileName(Entry.ConfigFile.ConfigFilePath);

        // Metadata for this entry's ConfigFile.
        Key = Entry.Definition.Key;
        Section = Entry.Definition.Section;
        Desc = Entry.Description.Description;

        // The values assigned to this entry.
        DefaultValue = Entry.DefaultValue;
        CurrentValue = Entry.Value;
    }
}
