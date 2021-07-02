using System;
using Game.Scripts.Models;
using Game.Scripts.Renderer;
using Game.Scripts.SkakBoard.Management;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Squares
{
    [RequireComponent(typeof(LineAnimator))]
    public class Square : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private LineAnimator _animator;

        public SquareState state;
        public SpritesLine[] spritesByType;
        
        public int firstBrokenSpriteNumber = 4;
        public int brokenSpritesCount = 4;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<LineAnimator>();

            _animator.OnAnimationEnd += () => Destroy(gameObject);
        }

        private void Start()
        {
            _animator.sprites = spritesByType[(int) state.squareType];
        }

        public void SetSorting(int line) =>
            _spriteRenderer.sortingOrder = GetComponentInParent<Board>().sorting.Square(line);

        public void Brake()
        {
            state.damageLevel = DamageLevel.Broken;
            _animator.animationFrame = firstBrokenSpriteNumber;
            _animator.animate = true;
        }

        private void Update()
        {
            var pos = transform.position;
            
            if (!Mathf.Approximately(state.height, pos.z))
            {
                pos.z = state.height;
                transform.position = pos;
            }
        }
    }
}