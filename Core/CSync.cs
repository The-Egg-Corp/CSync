using BepInEx;
using BepInEx.Logging;

using CSync.Core;

namespace CSync;

[BepInPlugin(Metadata.GUID, Metadata.NAME, Metadata.VERSION)]
public class CSync : BaseUnityPlugin {
    internal static new ManualLogSource Logger { get; private set; }

    private void Awake() {
        Logger = base.Logger;
    }
}