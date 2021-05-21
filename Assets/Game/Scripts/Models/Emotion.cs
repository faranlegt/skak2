using System;
using UnityEngine;

namespace Game.Scripts.Models
{
    [CreateAssetMenu(fileName = "piece", menuName = "Skak/Emotion")]
    public class Emotion : ScriptableObject
    {
        [Serializable]
        public class EyeEmotion
        {
            [Range(1, 3)]
            public int pupilSize = 2;

            [Range(0f, 1f)]
            public float upperEyelidLevel = 1f;

            [Range(0f, 1f)]
            public float bottomEyelidLevel = 0f;
        }

        public EyeEmotion left = new EyeEmotion();
        public EyeEmotion right = new EyeEmotion();
    }
}