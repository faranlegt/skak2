using System;
using Game.Scripts.SkakBoard;
using Game.Scripts.SkakBoard.Management;
using Game.Scripts.SkakBoard.Squares;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Scripts
{
    public class SkakSpawner : MonoBehaviour
    {
        public static SkakSpawner Instance { get; private set; }

        public Square squarePrefab;

        // This awake is more prioritized than others. See `Edit > Project Settings > Script Execution Order` 
        private void Awake()
        {
            if (Instance)
            {
                Debug.LogWarning("Entity spawner instance is set already. Destroying duplicate");
                Destroy(gameObject);
                return;
            }
            
            Assert.IsNotNull(squarePrefab, "Cell prefab cannot be unset");

            Instance = this;
            Debug.Log("Entity spawner initialized");
        }

        public Square SpawnCell(Board board, int x, int y, SquareState squareState)
        {
            Square square = Instantiate(squarePrefab, board.GetPositionFor(x, y), Quaternion.identity, board.transform);
            square.state = squareState;
            
            square.SyncSprite();
            square.SetSorting(y);

            return square;
        }
    }
}