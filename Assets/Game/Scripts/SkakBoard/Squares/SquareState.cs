using System;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Squares
{
    [Serializable]
    public class SquareState
    {
        public SquareType squareType;

        public DamageLevel damageLevel;
        
        [Range(0, 7)]
        public int animationFrame = 0;

        public bool isWalkable = true;
    }
}