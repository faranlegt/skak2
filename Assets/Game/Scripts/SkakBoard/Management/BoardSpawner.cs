using System;
using System.Security.Cryptography;
using Game.Scripts.SkakBoard.Entities;
using Game.Scripts.SkakBoard.Squares;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.SkakBoard.Management
{
    [RequireComponent(typeof(Board))]
    public class BoardSpawner : MonoBehaviour
    {
        private Board _board;

        private Transform _entitiesParent;
        private Transform _squaresParent;
        
        public Square squarePrefab;
        public Piece.Piece piecePrefab;

        public void Awake()
        {
            _board = GetComponent<Board>();
            
            _entitiesParent = CreateChildContainer("Entities");
            _squaresParent = CreateChildContainer("Squares");
        }

        private Transform CreateChildContainer(string name)
        {
            var go = new GameObject();
            go.transform.parent = transform;
            go.name = name;

            return go.transform;
        }
        

        public Square SpawnSquare(int x, int y, SquareState squareState)
        {
            var square = Instantiate(squarePrefab, _board.GetPositionFor(x, y), Quaternion.identity, _squaresParent);
            square.state = squareState;
            
            square.SyncSprite();
            square.SetSorting(y);
            square.name = $"Square ({x}, {y})";

            return square;
        }

        public Piece.Piece SpawnPiece(Vector2Int pos)
        {
            var piece = Instantiate(piecePrefab, _board.GetPositionFor(pos), Quaternion.identity, _entitiesParent);

            if (_board.entityManager.Place(piece.GetComponent<BoardEntity>(), pos))
            {
                return piece;
            }

            Destroy(piece);
            return null;

        }
    }
}