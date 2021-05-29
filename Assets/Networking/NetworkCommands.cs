using System.Text;
using MLAPI;
using Popcron.Console;
using UnityEngine;

namespace Networking
{
    [Category("Network")]
    public static class NetworkCommands
    {
        private static NetworkManager NetworkManager => NetworkManager.Singleton;

        [Command("net host")]
        public static void Host()
        {
            NetworkManager.StartHost();
        }

        [Command("net connect")]
        public static void Connect(string ip)
        {
            NetworkManager.StartClient();
        }

        [Alias("s")]
        [Command("status")]
        public static string Status()
        {
            if (!NetworkManager.IsClient && !NetworkManager.IsServer)
            {
                return "network is not started";
            } 
            
            var text = new StringBuilder();
            string mode = NetworkManager.IsHost 
                ? "Host" 
                : NetworkManager.IsServer ? "Server" : "Client";
            
            Value("Mode", mode);
            Value("Transport", NetworkManager.NetworkConfig.NetworkTransport.GetType().Name);


            void Value(string n, object value)
            {
                text.AppendLine($"\t\t{n}: {value}");
            }

            return text.ToString();
        }
    }
}