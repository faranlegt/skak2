using System;
using Game.Scripts.SkakBoard.Generators;
using Game.Scripts.SkakBoard.Squares;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Scripts.SkakBoard
{
    public class Board : MonoBehaviour
    {
        private Square[,] _squares;

        public int Size { get; private set; }

        public BoardGenerator boardGenerator;
        public float squareSize = 1;

        private void Start()
        {
            Assert.IsNotNull(boardGenerator, "Board generator wasn't set. Board will not be generated.");

            BuildBoard();
        }

        public void OnValidate() => SyncSquaresPositions();

        public Vector3 GetPositionFor(int x, int y) => 
            transform.position + new Vector3((x - Size / 2f + 0.5f) * squareSize, (y - Size / 2f + 0.5f) * squareSize, 0);

        private void BuildBoard()
        {
            Size = boardGenerator.size;
            _squares = boardGenerator.Generate(this);

            SyncSquaresPositions();
        }

        private void SyncSquaresPositions()
        {
            for (var x = 0; x < Size; x++)
            for (var y = 0; y < Size; y++)
            {
                _squares[x, y].transform.position = GetPositionFor(x, y);
            }
        }
    }
}