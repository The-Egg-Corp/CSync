using BepInEx.Configuration;
using System;
using System.Runtime.Serialization;

namespace CSync.Lib;

[Serializable]
public class SyncedEntry<T> : ISerializable {
    [NonSerialized]
    public readonly ConfigEntry<T> Entry;

    public T Value {
        get => Entry.Value;
        set => Entry.Value = value;
    }

    public string ConfigFilePath { get; private set; }
    public string Key { get; private set; }
    public string Section { get; private set; }
    public string Description { get; private set; }
    public object DefaultValue { get; private set; }
    public object CurrentValue { get; private set; }

    public SyncedEntry(ConfigEntry<T> configEntry) {
        Entry = configEntry;
        Init();
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

    public void Init() {
        ConfigFilePath = Entry.ConfigFile.ConfigFilePath;
        Key = Entry.Definition.Key;
        Section = Entry.Definition.Section;
        Description = Entry.Description.Description;
        DefaultValue = Entry.DefaultValue;
        CurrentValue = Entry.Value;
    }
}
