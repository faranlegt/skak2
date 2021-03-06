using Game.Scripts.SkakBoard.Management;
using Game.Scripts.SkakBoard.Squares;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Generators
{
    public abstract class BoardGenerator : ScriptableObject
    {
        public int size;
        
        public abstract Square[,] Generate(Board board);
    }
}