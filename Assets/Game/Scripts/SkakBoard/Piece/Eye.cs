using System;
using Game.Scripts.Models;
using Game.Scripts.SkakBoard.Management;
using UnityEngine;
using UnityEngine.Rendering;

namespace Game.Scripts.SkakBoard.Piece
{
    public class Eye : MonoBehaviour
    {
        private static readonly int ColorNumber = Shader.PropertyToID("colorNumber");
        private static readonly int AngleProperty = Shader.PropertyToID("angle");
        private static readonly int UpperEyelidLevelProperty = Shader.PropertyToID("closeLevelUp");
        private static readonly int BottomEyelidLevelProperty = Shader.PropertyToID("closeLevelDown");
        private static readonly int PupilSize = Shader.PropertyToID("pupilSize");
        private static readonly int PupilPos = Shader.PropertyToID("pupilPos");
        
        private SpriteRenderer _renderer;
        
        public float eyeballRadius = 2.5f / 16;
        public bool snapping = true;

        public Vector3 watchingPoint;
        public bool eyeFixed = false;

        public Emotion.EyeEmotion emotion = new Emotion.EyeEmotion();

        public float angleForSkin;
        
        /// <summary>
        /// True if eyes is upon second eye. Used for sorting, so the eye is behind won't get above.
        /// </summary>
        public bool isFrontEye = false;

        public int color;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Late, so eyes update is called earlier, where watching point is set.
        /// </summary>
        private void LateUpdate()
        {
            var pupilPixelOffset = Vector2.one / (2 - emotion.pupilSize % 2);

            var transform_ = this.transform;
            var localPosition = transform_.localPosition;
            
            _renderer.sortingOrder = (localPosition.y < 0 ? 2 : -2) + (isFrontEye ? -1 : 0);

            var position = transform_.position;
            var watchingDirection = watchingPoint - position;
            watchingDirection.z = 0;
            
            var pupilPos = watchingDirection * (eyeballRadius / watchingDirection.magnitude);
            if (snapping)
            {
                pupilPos = pupilPos.PixelSnap(Vector3.zero, pupilPixelOffset);
            }

            var material = _renderer.material;
            material.SetFloat(AngleProperty, angleForSkin * Mathf.Deg2Rad);
            material.SetFloat(UpperEyelidLevelProperty, emotion.upperEyelidLevel);
            material.SetFloat(BottomEyelidLevelProperty, emotion.bottomEyelidLevel);
            material.SetFloat(ColorNumber, color);
            material.SetFloat(PupilSize, emotion.pupilSize);
            material.SetVector(PupilPos, pupilPos);
        }
    }
}