using System;
using Game.Scripts.SkakBoard.Management;
using MLAPI;
using UnityEngine;

namespace Networking
{
    [RequireComponent(typeof(NetworkManager))]
    public class SessionManagement : MonoBehaviour
    {
        private NetworkManager _networkManager;

        public Board boardPrefab;

        private void Awake()
        {
            _networkManager = GetComponent<NetworkManager>();
        }

        private void Start()
        {
            _networkManager.OnServerStarted += SetupSession;
        }

        private void SetupSession()
        {
            var board = Instantiate(boardPrefab, Vector3.zero, Quaternion.identity);
            board.name = "Board";
            board.GetComponent<NetworkObject>().Spawn();
        }
    }
}