using System;
using Game.Scripts.SkakBoard.Management;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts.SkakBoard.Piece
{
    public class Eye : MonoBehaviour
    {
        private SortingGroup _sortingGroup;
        private Eyes _eyes;
        private Piece _piece;

        public Transform pupil;
        
        public float eyeballRadius = 2.5f / 16;
        public bool snapping = true;

        public Vector3 watchingPoint;
        public bool eyeFixed = false;
        public Vector2 pupilPixelOffset;

        public SpriteRenderer skinRenderer;

        public float angle;
        public float angleForSkin;
        
        /// <summary>
        /// True if eyes is upon second eye. Used for sorting, so the eye is behind won't get above.
        /// </summary>
        public bool isFrontEye = false;

        private static readonly int Angle = Shader.PropertyToID("angle");


        private void Awake()
        {
            _sortingGroup = GetComponent<SortingGroup>();
            _piece = GetComponentInParent<Piece>();
            _eyes = GetComponentInParent<Eyes>();
        }

        /// <summary>
        /// Late, so eyes update is called earlier, where watching point is set.
        /// </summary>
        private void LateUpdate()
        {
            var transform_ = this.transform;
            
            var localPosition = GetEyePosition(angle);
            transform_.localPosition = localPosition;
            
            _sortingGroup.sortingOrder = (localPosition.y < 0 ? 2 : -2) + (isFrontEye ? -1 : 0);

            var position = transform_.position;
            var watchingDirection = watchingPoint - position;
            watchingDirection.z = 0;
            
            var pupilPos = watchingDirection * (eyeballRadius / watchingDirection.magnitude);
            if (snapping)
            {
                pupilPos = pupilPos.PixelSnap(Vector3.zero, pupilPixelOffset);
            }

            pupil.transform.localPosition = pupilPos;

            skinRenderer.material.SetFloat(Angle, angleForSkin * Mathf.Deg2Rad);
        }

        private Vector3 GetEyePosition(float a)
        {
            float rad = a * Mathf.Deg2Rad;
            float x = Mathf.Sin(rad);
            float y = Mathf.Cos(rad) * _eyes.yCoefficient;

            var freePos = new Vector3(x, y, 0) * _piece.piece.eyesRadius;
            if (_eyes.pixelSnapping)
            {
                freePos = freePos.PixelSnap(transform.localPosition, _eyes.pixelSnapOffset);
            }

            return freePos;
        }
    }
}