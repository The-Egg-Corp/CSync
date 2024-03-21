using CSync.Lib;
using HarmonyLib;

namespace CSync.Patches.LethalCompany;

[HarmonyPatch(typeof(GameNetworkManager))]
internal class LeavePatch_LC {
    [HarmonyPostfix]
    [HarmonyPatch("StartDisconnect")]
    private static void RevertOnDisconnect() {
        ConfigManager.RevertSyncedInstances();
    }
}