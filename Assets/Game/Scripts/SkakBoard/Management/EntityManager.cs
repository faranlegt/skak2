using System.Collections.Generic;
using System.Linq;
using Game.Scripts.SkakBoard.Entities;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Management
{
    public class EntityManager : MonoBehaviour
    {
        private readonly HashSet<BoardEntity> _entities = new HashSet<BoardEntity>();
        
        private Board _board;
        private Squares _squares;

        public void Awake()
        {
            _board = GetComponent<Board>();
            _squares = GetComponent<Squares>();
        }

        /// <summary>
        /// Places some entity on board if square isn't occupied. Entity position will be rewritten by provided value. 
        /// </summary>
        /// <param name="pos">Board position.</param>
        /// <param name="boardEntity">Entity to place.</param>
        /// <returns>True if entity was placed. False otherwise.</returns>
        public bool Place(BoardEntity boardEntity, Vector2Int pos)
        {
            if (!_squares.IsWalkable(pos)) return false;
            if (_entities.Any(e => e.boardPosition == pos)) return false;
         
            boardEntity.boardPosition = pos;   
            _entities.Add(boardEntity);

            return true;
        }
    }
}