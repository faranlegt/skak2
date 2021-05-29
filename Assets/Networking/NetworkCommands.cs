using System.Text;
using MLAPI;
using Popcron.Console;
using UnityEngine;

namespace Networking
{
    [Category("Network")]
    public class NetworkCommands : MonoBehaviour
    {
        private NetworkManager _networkManager;

        private void Awake()
        {
            _networkManager = GetComponent<NetworkManager>();
            Parser.Register(this, "net");
        }

        [Alias("h")]
        [Command("host")]
        public void Host()
        {
            _networkManager.StartHost();
        }

        [Command("connect")]
        public void Connect(string ip)
        {
            _networkManager.StartClient();
        }

        [Alias("s")]
        [Command("status")]
        public string Status()
        {
            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                return "network is not started";
            } 
            
            var text = new StringBuilder();
            string mode = NetworkManager.Singleton.IsHost 
                ? "Host" 
                : NetworkManager.Singleton.IsServer ? "Server" : "Client";
            
            Value("Mode", mode);
            Value("Transport", _networkManager.NetworkConfig.NetworkTransport.GetType().Name);


            void Value(string n, object value)
            {
                text.AppendLine($"\t\t{n}: {value}");
            }

            return text.ToString();
        }
    }
}