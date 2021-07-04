using Game.Scripts.SkakBoard.Management;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts.SkakBoard.Entities
{
    public class BoardEntity : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private SortingGroup _sortingGroup;
        private bool _hasSortingGroup;

        public Vector2Int? boardPosition;

        public Board board;
        public bool IsPlaced => id >= 0;
        public int id = -1;

        private void Awake()
        {
            _hasSortingGroup = TryGetComponent(out _sortingGroup);
            if (!_hasSortingGroup)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        private void Update()
        {
            if (!IsPlaced) return;
            
            int sorting = board.sorting.Entity(transform.position);
            if (_hasSortingGroup)
            {
                _sortingGroup.sortingOrder = sorting;
            }
            else
            {
                _spriteRenderer.sortingOrder = sorting;
            }
        }

        public void Place(Board newBoard, int newId)
        {
            board = newBoard;
            id = newId;
        }

        public void RemoveFromBoard()
        {
            board = null;
            id = -1;
        }
    }
}