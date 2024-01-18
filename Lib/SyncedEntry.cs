using BepInEx.Configuration;
using System;

namespace CSync.Lib;

[Serializable]
public class SyncedEntry<T> {
    public T Value { 
        get => Entry.Value; 
        private set => Value = value;
    }

    [NonSerialized]
    readonly ConfigEntry<T> Entry;

    SyncedEntry() {}

    SyncedEntry(ConfigEntry<T> entry) {
        Entry = entry;
    }

    public void Apply() {
        Entry.Value = Value;
        Entry.ConfigFile.Save();
    }
}