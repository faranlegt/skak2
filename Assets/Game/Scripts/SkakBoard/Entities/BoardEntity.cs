using System;
using Game.Scripts.SkakBoard.Management;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Entities
{
    public class BoardEntity : MonoBehaviour
    {
        private Lazy<Board> _boardLazy;
        
        public Vector2Int? boardPosition;

        public Board Board => _boardLazy.Value;

        public BoardEntity()
        {
            _boardLazy = new Lazy<Board>(GetComponentInParent<Board>);
        }
    }
}