using System.Security.Cryptography;
using Game.Scripts.SkakBoard.Entities;
using Game.Scripts.SkakBoard.Squares;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Management
{
    [RequireComponent(typeof(Board))]
    public class BoardSpawner : MonoBehaviour
    {
        private Board _board;
        
        public Square squarePrefab;
        public Piece.Piece piecePrefab;

        public void Awake()
        {
            _board = GetComponent<Board>();
        }

        public Square SpawnCell(int x, int y, SquareState squareState)
        {
            var square = Instantiate(squarePrefab, _board.GetPositionFor(x, y), Quaternion.identity, _board.transform);
            square.state = squareState;
            
            square.SyncSprite();
            square.SetSorting(y);

            return square;
        }

        public Piece.Piece SpawnPiece(Vector2Int pos)
        {
            var piece = Instantiate(piecePrefab, _board.GetPositionFor(pos), Quaternion.identity, _board.transform);

            if (_board.entityManager.Place(piece.GetComponent<BoardEntity>(), pos))
            {
                return piece;
            }

            Destroy(piece);
            return null;

        }
    }
}