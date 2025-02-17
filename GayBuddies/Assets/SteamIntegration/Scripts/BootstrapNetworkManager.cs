using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Managing.Scened;
using System.Linq;

public class BootstrapNetworkManager : NetworkBehaviour
{
    private static BootstrapNetworkManager instance;

    public void Awake() {
        instance = this;
    }

    public static void ChangeNetworkScene(string sceneName, string[] scenesToClose) {
        instance.CloseScenes(scenesToClose);
        SceneLoadData sld = new SceneLoadData(sceneName);
        NetworkConnection[] conns = instance.ServerManager.Clients.Values.ToArray();
        instance.SceneManager.LoadConnectionScenes(conns, sld);
    }

    [ServerRpc(RequireOwnership = false)]
    void CloseScenes(string[] scenesToClose) {
        CloseScenesObserver(scenesToClose);
    } 

    [ObserversRpc]
    void CloseScenesObserver(string[] scenesToClose) {
        foreach (string sceneName in scenesToClose) {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
        }
    }

}
