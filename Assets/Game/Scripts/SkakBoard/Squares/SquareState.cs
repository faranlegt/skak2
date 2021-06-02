using System;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Squares
{
    [Serializable]
    public class SquareState
    {
        public SquareType squareType;

        public DamageLevel damageLevel;

        public bool isWalkable = true;
    }
}