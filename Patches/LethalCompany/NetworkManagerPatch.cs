using CSync.Lib;
using HarmonyLib;
using Unity.Netcode;

namespace CSync.Patches.LethalCompany;

[HarmonyPatch(typeof(NetworkConnectionManager))]
internal class NetworkManagerPatch {
    static bool HasAuthority => NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsServer;
    
    [HarmonyPostfix]
    [HarmonyPatch("InvokeOnClientConnectedCallback")]
    private static void OnConnected(ulong clientId) {
        ConfigManager.SyncInstances();

        if (!HasAuthority) {
            Plugin.Logger.LogDebug($"Client connected with id: {clientId}");
        }
    }
}
