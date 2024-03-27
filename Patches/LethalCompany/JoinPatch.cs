using CSync.Lib;
using GameNetcodeStuff;
using HarmonyLib;

namespace CSync.Patches.LethalCompany;

[HarmonyPatch(typeof(PlayerControllerB))]
internal class JoinPatch_LC {
    [HarmonyPostfix]
    [HarmonyPatch("ConnectClientToPlayerObject")]
    static void SyncOnJoin() {
        ConfigManager.SyncInstances();
    }
}