using BepInEx.Configuration;

namespace CSync.Core;

internal class CSyncConfig(ConfigFile cfg) {
    public ConfigEntry<bool> ENABLE_PATCHING { get; } = cfg.Bind(Plugin.GUID, "bEnablePatching", true,
        "Whether to let CSync detect apply necessary game-independent patches.\n" +
        "It is not recommended to disable this unless all of your mods implement their own patches for syncing."
    );

    public ConfigEntry<bool> GAME_DETECTION { get; } = cfg.Bind(Plugin.GUID, "bGameDetection", true,
        "Should CSync automatically apply patches specific to the game currently running?\n" +
        "Turning this off will not affect the core functionality of CSync, in some cases, it may solve issues."
    );
}