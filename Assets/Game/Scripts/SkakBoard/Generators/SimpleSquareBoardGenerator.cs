using Game.Scripts.SkakBoard.Management;
using Game.Scripts.SkakBoard.Squares;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Generators
{
    [CreateAssetMenu(menuName = "Skak/Board Generators/Simple square")]
    public class SimpleSquareBoardGenerator : BoardGenerator
    {
        public SquareType leftCornerSquare = SquareType.Black; 
        
        public override Square[,] Generate(Board board)
        {
            var cells = new Square[size, size];
            
            for (var i = 0; i < size; i++)
            for (var j = 0; j < size; j++)
            {
                SquareType squareType = (i + j % 2 + (int) leftCornerSquare) % 2 == 0
                    ? SquareType.Black
                    : SquareType.White;

                var cellState = new SquareState
                {
                    squareType = squareType
                };

                cells[i, j] = board.spawner.SpawnSquare(i, j, cellState);
            }

            return cells;
        }
    }
}