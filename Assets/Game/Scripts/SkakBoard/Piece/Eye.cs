using System;
using Game.Scripts.Models;
using Game.Scripts.SkakBoard.Management;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts.SkakBoard.Piece
{
    public class Eye : MonoBehaviour
    {
        private static readonly int AngleProperty = Shader.PropertyToID("angle");
        private static readonly int UpperEyelidLevelProperty = Shader.PropertyToID("closeLevelUp");
        private static readonly int BottomEyelidLevelProperty = Shader.PropertyToID("closeLevelDown");
        
        private SortingGroup _sortingGroup;

        public Transform pupil;
        
        public float eyeballRadius = 2.5f / 16;
        public bool snapping = true;

        public Vector3 watchingPoint;
        public bool eyeFixed = false;

        public SpriteRenderer skinRenderer;
        public Emotion.EyeEmotion emotion = new Emotion.EyeEmotion();

        public float angleForSkin;
        
        /// <summary>
        /// True if eyes is upon second eye. Used for sorting, so the eye is behind won't get above.
        /// </summary>
        public bool isFrontEye = false;


        private void Awake()
        {
            _sortingGroup = GetComponent<SortingGroup>();
            GetComponentInParent<Piece>();
            GetComponentInParent<Eyes>();
        }

        /// <summary>
        /// Late, so eyes update is called earlier, where watching point is set.
        /// </summary>
        private void LateUpdate()
        {
            var eyelidsMaterial = skinRenderer.material;
            eyelidsMaterial.SetFloat(AngleProperty, angleForSkin * Mathf.Deg2Rad);
            eyelidsMaterial.SetFloat(UpperEyelidLevelProperty, emotion.upperEyelidLevel);
            eyelidsMaterial.SetFloat(BottomEyelidLevelProperty, emotion.bottomEyelidLevel);

            pupil.localScale = Vector3.one * emotion.pupilSize / 2f;
            var pupilPixelOffset = Vector2.one / (2 - emotion.pupilSize % 2);

            var transform_ = this.transform;
            var localPosition = transform_.localPosition;
            
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
        }
    }
}