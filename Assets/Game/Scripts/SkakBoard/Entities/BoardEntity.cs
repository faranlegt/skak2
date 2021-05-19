using System;
using Game.Scripts.SkakBoard.Management;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts.SkakBoard.Entities
{
    public class BoardEntity : MonoBehaviour
    {
        private Lazy<Board> _boardLazy;

        private SpriteRenderer _spriteRenderer;
        private SortingGroup _sortingGroup;
        private bool _hasSortingGroup;

        public Vector2Int? boardPosition;

        public Board Board => _boardLazy.Value;

        public BoardEntity()
        {
            _boardLazy = new Lazy<Board>(GetComponentInParent<Board>);
        }

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
            int sorting = Board.Sorting.Entity(transform.position);
            if (_hasSortingGroup)
            {
                _sortingGroup.sortingOrder = sorting;
            }
            else
            {
                _spriteRenderer.sortingOrder = sorting;
            }
        }
    }
}