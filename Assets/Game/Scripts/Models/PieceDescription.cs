using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Models
{
    [CreateAssetMenu(fileName = "piece", menuName = "Skak/Piece")]
    public class PieceDescription : ScriptableObject
    {
        public float eyesBaseLevel = 1f;
        
        [Range(0f, 2f)]
        public float eyesRadius = 0.1f;

        [Range(0, 360)]
        public float angleBetweenEyes = 10f;
    }
}