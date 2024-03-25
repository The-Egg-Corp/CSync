using BepInEx.Configuration;

namespace CSync.Core;

internal class CSyncConfig {
    public ConfigEntry<bool> ENABLE_PATCHING { get; private set; }
    
    public CSyncConfig(ConfigFile cfg) {
        ENABLE_PATCHING = cfg.Bind(Plugin.GUID, "bEnablePatching", true, 
            "Whether to let CSync detect the game and apply patches itself.\n" + 
            "It is not recommended to disable this unless all of your mods implement their own patches for syncing."
        );
    }
}
