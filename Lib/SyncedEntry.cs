using System;

namespace CSync.Lib;

[Serializable]
internal class SyncedEntry<T> {
    internal static T Value { get; private set; }

    internal SyncedEntry() { }
}
