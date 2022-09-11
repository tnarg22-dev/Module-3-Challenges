using Unity.Netcode;
using UnityEngine;


// Adapted from https://docs-multiplayer.unity3d.com/netcode/current/tutorials/helloworld
//
// This adds the serverStartType property which allows you to specify how the project
// should be run when running through the Unity editor.
public class GameManager : MonoBehaviour {
    public NetworkCommandLine.StartModes serverStartType = NetworkCommandLine.StartModes.CHOOSE;
    private GameObject networkCmdlnObj;


    private void Start() {
        if (Application.isEditor) {
            networkCmdlnObj = GameObject.Find("NetworkCommandLine");
            var networkCmdln = networkCmdlnObj.GetComponent<NetworkCommandLine>();
            networkCmdln.StartAs(serverStartType);
        }
    }


    void OnGUI() {
        GUILayout.BeginArea(new Rect(10, 10, 300, 300));
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer) {
            StartButtons();
        } else {
            StatusLabels();
        }

        GUILayout.EndArea();
    }


    static void StartButtons() {
        if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
        if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
        if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
    }


    static void StatusLabels() {
        var mode = NetworkManager.Singleton.IsHost ?
            "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: " +
            NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
        GUILayout.Label("Mode: " + mode);
    }
}