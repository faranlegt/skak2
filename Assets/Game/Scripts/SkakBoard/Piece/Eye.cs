using System;
using Game.Scripts.SkakBoard.Management;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Piece
{
    public class Eye : MonoBehaviour
    {
        private Piece _piece;
        private SpriteRenderer _spriteRenderer;
        private SpriteRenderer _pupilSpriteRenderer;
        private Board _board;

        public Transform pupil;
        
        public float eyeballRadius = 2.5f / 16;
        public bool snapping = true;
        public Vector3 snapOffset = Vector3.zero;

        public Vector3 watchingPoint;
        public bool eyeFixed = false;
        public Vector2 pupilPixelOffset;
        
        /// <summary>
        /// True if eyes is upon second eye. Used for sorting, so the eye is behind won't get above.
        /// </summary>
        public bool isFrontEye = false;

        private void Awake()
        {
            _piece = GetComponentInParent<Piece>();
            _board = GetComponentInParent<Board>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _pupilSpriteRenderer = pupil.GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Late, so eyes update is called earlier, where watching point is set.
        /// </summary>
        private void LateUpdate()
        {
            Vector3 piecePosition = _piece.transform.position;
            Vector3 localPosition = transform.localPosition;
            
            _spriteRenderer.sortingOrder = _board.Sorting.Eye(piecePosition, localPosition.y < 0, isFrontEye);
            _pupilSpriteRenderer.sortingOrder = _board.Sorting.Pupil(piecePosition, localPosition.y < 0, isFrontEye);

            Vector3 position = transform.position;
            Vector3 watchingDirection = watchingPoint - position;
            watchingDirection.z = 0;
            
            Vector3 pupilPos = watchingDirection * (eyeballRadius / watchingDirection.magnitude);
            if (snapping)
            {
                pupilPos = pupilPos.PixelSnap(Vector3.zero, pupilPixelOffset);
            }

            pupil.transform.localPosition = pupilPos;
        }
    }
}