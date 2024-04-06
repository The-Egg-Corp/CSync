using HarmonyLib;
using UnityEngine.UI;

namespace CSync.Patches.LethalCompany;

[HarmonyPatch(typeof(Button))]
internal class LethalConfigPatch {
    [HarmonyPostfix]
    [HarmonyPatch(nameof(Button.OnPointerClick))]
    static void ApplyClicked(Button __instance) {
        Plugin.Logger.LogDebug("Apply clicked! Attempting to resync..");
        
        var isLC = __instance.transform.parent.name == "BottomBar";
        if (!isLC) return;

        var isApply = __instance.transform.name == "ApplyButton";
        if (!isApply) return;

        // If we are the host, make all clients resync.
        //if (NetworkManager.Singleton.IsHost) {
            
        //}
    }
}