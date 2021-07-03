using System;
using Game.Scripts.SkakBoard.Management;
using Game.Scripts.SkakBoard.Squares;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Generators
{
    [CreateAssetMenu(menuName = "Skak/Board Generators/Test Scene")]
    public class TestSceneBoardGenerator : BoardGenerator
    {
        //private float?[,] _heights =
        //{
        //    {
        //        null, null, null, null, null
        //    },
        //    {
        //        null, null, 1, 0.15f, null
        //    },
        //    {
        //        null, 1, 1, 1, 1
        //    },
        //    {
        //        0.15f, 1, 1, 1, null
        //    },
        //    {
        //        null, null, null, null, null
        //    },
        //};

        private float?[,] _heights;

        void GenerateHeights()
        {
            int w = size;

            _heights = new float?[w, w];

            var cX = w / 2;
            var cY = w / 2;

            var k = 0.3f;
            var nX = UnityEngine.Random.Range(0, 10000);
            var nY = UnityEngine.Random.Range(0, 10000);

            for (var y = 0; y < w; y++)
                for (var x = 0; x < w; x++)
                {
                    var noise = Mathf.PerlinNoise(nX + x * k, nY + y * k);
                    var fade = 1 - new Vector2(cX - x, cY - y).magnitude / cX;

                    _heights[x, y] = (noise * 0.7f + fade * 2) / 2 - 0.1f;

                    if (_heights[x, y] > 0.5f) _heights[x, y] = 1f;
                    if (_heights[x, y] < 0.2f) _heights[x, y] = null;
                }
        }

        public override Square[,] Generate(Board board)
        {
            GenerateHeights();

            int h = _heights.GetLength(0),
                w = _heights.GetLength(1);
            
            if (h > size || w > size)
            {
                throw new InvalidOperationException($"Size can't be less than {Mathf.Max(h, w)}");
            }
            
            var cells = new Square[size, size];
            int startX = (size - w) / 2, startY = (size - h) / 2; 

            for (var i = 0; i < w; i++)
            for (var j = 0; j < h; j++)
            {
                var height = _heights[j, i];
                if (!height.HasValue) continue;
                
                
                int x = i + startX, y = j + startY;
                SquareType squareType = (x + y % 2) % 2 == 0
                    ? SquareType.Black
                    : SquareType.White;

                var cellState = new SquareState
                {
                    squareType = squareType,
                    height = height.Value * 2.95f
                };

                cells[i, j] = board.spawner.SpawnSquare(x, y, cellState);
            }

            return cells;
        }
    }
}