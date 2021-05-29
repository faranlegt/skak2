using System;
using Game.Scripts.Models;
using UnityEngine;

namespace Game.Scripts.Renderer
{
    public delegate void OnAnimationEnd();
    
    [RequireComponent(typeof(SpriteRenderer))]
    public class LineAnimator : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private bool _revalidate;
        private float _frameTime;
        private int _oldFrame;
        private SpritesLine _oldSprites;
        
        public SpritesLine sprites;
        public int animationFrame = 0;
        public bool loop;
        public bool animate;
        public float frameLength = 1f;

        public event OnAnimationEnd OnAnimationEnd;
        
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            
            SyncSprite();
        }

        private void Start()
        {
            _revalidate = true;
        }

        private void OnValidate()
        {
            _revalidate = true;
        }

        private void LateUpdate()
        {
            _revalidate |= _oldFrame != animationFrame || _oldSprites != sprites;
            
            if (_revalidate)
            {
                SyncSprite();
            }

            if (animate)
            {
                _frameTime += Time.deltaTime;
                if (_frameTime >= frameLength)
                {
                    NextFrame();
                }
            }

            _oldFrame = animationFrame;
            _oldSprites = sprites;
        }

        private void NextFrame()
        {
            _frameTime = 0;
            if (animationFrame == sprites.sprites.Length - 1)
            {
                if (loop)
                {
                    animationFrame = 0;
                }
                else
                {
                    animate = false;
                    OnAnimationEnd?.Invoke();
                }
            }
            else
            {
                animationFrame++;
            }

            SyncSprite();
        }

        public void SyncSprite()
        {
            _renderer.sprite = sprites.sprites[animationFrame];
            _revalidate = false;
        }
    }
}