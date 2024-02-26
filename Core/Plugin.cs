using BepInEx;
using BepInEx.Logging;

using CSync.Core;

namespace CSync;

/// <summary>
/// The plugin and main entry point of this library, responsible for holding the logger.<br></br>
/// Helps with internal logging and caching of config files through static references.<br></br>
/// <br></br>
/// Do <b>NOT</b> use this class directly or create any instances of it!
/// </summary>
[BepInPlugin(Metadata.GUID, Metadata.NAME, Metadata.VERSION)]
public class Plugin : BaseUnityPlugin {
    internal static new ManualLogSource Logger { get; private set; }

    private void Awake() {
        Logger = base.Logger;
    }
}