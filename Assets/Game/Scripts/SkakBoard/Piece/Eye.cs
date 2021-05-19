using System;
using Game.Scripts.SkakBoard.Management;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts.SkakBoard.Piece
{
    public class Eye : MonoBehaviour
    {
        private SortingGroup _sortingGroup;

        public Transform pupil;
        
        public float eyeballRadius = 2.5f / 16;
        public bool snapping = true;

        public Vector3 watchingPoint;
        public bool eyeFixed = false;
        public Vector2 pupilPixelOffset;
        
        /// <summary>
        /// True if eyes is upon second eye. Used for sorting, so the eye is behind won't get above.
        /// </summary>
        public bool isFrontEye = false;


        private void Awake()
        {
            _sortingGroup = GetComponent<SortingGroup>();
        }

        /// <summary>
        /// Late, so eyes update is called earlier, where watching point is set.
        /// </summary>
        private void LateUpdate()
        {
            var transform1 = transform;
            var localPosition = transform1.localPosition;
            
            _sortingGroup.sortingOrder = (localPosition.y < 0 ? 2 : -2) + (isFrontEye ? -1 : 0);

            var position = transform1.position;
            var watchingDirection = watchingPoint - position;
            watchingDirection.z = 0;
            
            var pupilPos = watchingDirection * (eyeballRadius / watchingDirection.magnitude);
            if (snapping)
            {
                pupilPos = pupilPos.PixelSnap(Vector3.zero, pupilPixelOffset);
            }

            pupil.transform.localPosition = pupilPos;
        }
    }
}