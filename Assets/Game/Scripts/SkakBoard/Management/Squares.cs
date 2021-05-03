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
            if (pos.x < 0 || pos.x >= _board.Size || pos.y < 0 || pos.y >= _board.Size) return false;
            if (!_squares[pos.x, pos.y]) return false;

            return _squares[pos.x, pos.y].state.isWalkable;
        }
    }
}