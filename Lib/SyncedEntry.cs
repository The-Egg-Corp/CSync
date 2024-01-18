using BepInEx.Configuration;
using System;

namespace CSync.Lib;

[Serializable]
public class SyncedEntry<T>(ConfigEntry<T> configEntry) {
    public T Value { get; private set; } = configEntry.Value;

    public void Apply() {
        configEntry.Value = Value;
        configEntry.ConfigFile.Save();
    }
}