using System;
using Game.Scripts.SkakBoard.Entities;
using Popcron.Console;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Management
{
    [Category("Board")]
    public class BoardCommands : MonoBehaviour
    {
        private Board _board;

        private void Awake()
        {
            _board = GetComponent<Board>();
        }

        private void OnEnable()
        {
            Parser.Register(this, "board");
        }

        private void OnDisable()
        {
            Parser.Unregister(this);
        }

        [Alias("p")]
        [Command("spawn_piece")]
        public string PlacePiece(int x, int y, int color)
        {
            var spawned = _board.spawner.SpawnPiece(new Vector2Int(x, y));
            if (!spawned)
            {
                return "<b>Piece wasn't spawned</b>";
            }

            spawned.color = color;
            
            return spawned.GetComponent<BoardEntity>().id.ToString();
        }
    }
}