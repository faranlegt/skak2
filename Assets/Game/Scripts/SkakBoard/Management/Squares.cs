using System;
using Game.Scripts.SkakBoard.Generators;
using Game.Scripts.SkakBoard.Squares;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Management
{
    public class Squares : MonoBehaviour
    {
        private Board _board;
        
        private Square[,] _squares;

        private void Awake()
        {
            _board = GetComponent<Board>();
        }

        public void SyncSquaresPositions()
        {
            for (var x = 0; x < _board.Size; x++)
            for (var y = 0; y < _board.Size; y++)
            {
                _squares[x, y].transform.position = _board.GetPositionFor(x, y);
            }
        }

        public void Generate(BoardGenerator boardGenerator)
        {
            _squares = boardGenerator.Generate(_board);
        }

        /// <summary>
        /// Checks is square can be walked on.
        /// </summary>
        /// <param name="pos">Position of the square.</param>
        /// <returns>False if there is no square or it's not walkable. True otherwise.</returns>
        public bool IsWalkable(Vector2Int pos)
        {
            if (!Exists(pos)) return false;

            return _squares[pos.x, pos.y].state.isWalkable;
        }

        public float GetHeight(Vector2Int pos) => GetHeight(pos.x, pos.y);

        public float GetHeight(int x, int y)
        {
            if (!Exists(x, y)) return 0;

            return _squares[x, y].state.height;
        }

        public bool Exists(Vector2Int pos) => Exists(pos.x, pos.y);
        
        public bool Exists(int x, int y)
        {
            if (x < 0 || x >= _board.Size || y < 0 || y >= _board.Size) return false;
            if (_squares is null || !_squares[x, y]) return false;

            return true;
        }
    }
}