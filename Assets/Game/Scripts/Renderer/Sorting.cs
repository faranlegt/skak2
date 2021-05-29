using Game.Scripts.SkakBoard.Management;
using UnityEngine;

namespace Game.Scripts.Renderer
{
    /// <summary>
    /// Sorting in skak is based on board lines (y coord)
    /// </summary>
    public class Sorting
    {
        private readonly Board _board;
        
        private int _baseLineSortingStep = -32;
        private int _entityStep = 16;
        

        public Sorting(Board board) {
            _board = board;
        }
        
        private float BaseLine(float line) => line * _baseLineSortingStep;

        public int Square(int line) => (int)BaseLine(line);

        public int Entity(Vector3 position) => (int)(BaseLine(_board.ToBoard(position).y - 0.5f) + _entityStep);
    }
}