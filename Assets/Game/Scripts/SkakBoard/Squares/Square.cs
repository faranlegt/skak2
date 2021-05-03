using Game.Scripts.Renderer;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Squares
{
    public class Square : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private float _animationTime = 0f;

        public bool revalidate;
        public SquareState state;
        public SpritesLine[] spritesByType;
        public AnimationCurve destructionAnimationCurve;
        public float destructionTime = 2f;
        public int firstBrokenSpriteNumber = 4;
        public int brokenSpritesCount = 4;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            SyncSprite();
        }

        private void Update()
        {
            if (revalidate)
            {
                SyncSprite();
                revalidate = false;
            }

            if (state.damageLevel == DamageLevel.Broken)
            {
                _animationTime += Time.deltaTime;
                float brokenProgress = destructionAnimationCurve.Evaluate(_animationTime / destructionTime);
                var brokenSprite = (int) Mathf.Lerp(0, brokenSpritesCount, brokenProgress);
                transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y,
                    -brokenProgress
                );

                if (brokenSprite >= brokenSpritesCount)
                {
                    Destroy(gameObject);
                }
                else
                {
                    state.animationFrame = firstBrokenSpriteNumber + brokenSprite;
                    SyncSprite();
                }
            }
        }

        public void SyncSprite()
        {
            _spriteRenderer.sprite = spritesByType[(int) state.squareType].sprites[state.animationFrame];
        }

        public void SetSorting(int line) =>
            _spriteRenderer.sortingOrder = Sorting.Square(line);

        private void OnValidate()
        {
            revalidate = true;
        }

        public void Brake()
        {
            state.damageLevel = DamageLevel.Broken;
        }
    }
}