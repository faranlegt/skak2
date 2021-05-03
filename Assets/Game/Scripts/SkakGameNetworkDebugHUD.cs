using MLAPI;
using UnityEngine;

namespace Game.Scripts
{
    public class SkakGameNetworkDebugHUD : MonoBehaviour
    {
        public void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            GUILayout.EndArea();
        }

        private static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) NetworkManager.Singleton.StartServer();
        }

        private static void StatusLabels()
        {
            string mode = NetworkManager.Singleton.IsHost 
                ? "Host" 
                : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label($"Transport: {NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name}");
            GUILayout.Label("Mode: " + mode);
        }
    }
}
