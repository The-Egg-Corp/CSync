using BepInEx.Configuration;
using System;
using System.Runtime.Serialization;

namespace CSync.Lib;

[Serializable]
public class SyncedEntry<T> : ISerializable {
    [NonSerialized] readonly string ConfigFilePath;
    [NonSerialized] readonly string Key;
    [NonSerialized] readonly string Section;
    [NonSerialized] readonly string Description;
    [NonSerialized] readonly object DefaultValue;
    [NonSerialized] readonly object CurrentValue;

    [NonSerialized] public readonly ConfigEntry<T> Entry;

    public T Value {
        get => Entry.Value;
        set => Entry.Value = value;
    }

    public SyncedEntry(ConfigEntry<T> configEntry) {
        Entry = configEntry;
        
        ConfigFilePath = Entry.ConfigFile.ConfigFilePath;
        Key = Entry.Definition.Key;
        Section = Entry.Definition.Section;
        Description = Entry.Description.Description;
        DefaultValue = Entry.DefaultValue;
        CurrentValue = Entry.Value;
    }

    // Deserialization
    SyncedEntry(SerializationInfo info, StreamingContext ctx) {
        ConfigFile cfg = new(info.GetString("ConfigFilePath"), false);
        ConfigDefinition definition = new(info.GetString("Section"), info.GetString("Key"));
        ConfigDescription description = new(info.GetString("Description"));

        T defaultVal = (T) info.GetValue("DefaultValue", typeof(T));

        // Reconstruct ConfigEntry
        Entry = cfg.Bind(definition, defaultVal, description);
        Value = (T) info.GetValue("CurrentValue", typeof(T));
    }

    // Serialization
    public void GetObjectData(SerializationInfo info, StreamingContext context) {
        info.AddValue("ConfigFilePath", ConfigFilePath);
        info.AddValue("Key", Key);
        info.AddValue("Section", Section);
        info.AddValue("Description", Description);
        info.AddValue("DefaultValue", DefaultValue);
        info.AddValue("CurrentValue", CurrentValue);
    }
}
