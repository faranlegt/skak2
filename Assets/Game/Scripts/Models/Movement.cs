using UnityEngine;

namespace Game.Scripts.Models
{
    public class Movement
    {
        /// <summary>
        /// Board square destination.
        /// </summary>
        public Vector2Int Destination;
        
        /// <summary>
        /// Point where movement started. Is not necessarily board square, so it's vector.
        /// </summary>
        public Vector3 Source;

        /// <summary>
        /// Float in range of 0 to 1, describing movement progress.
        /// </summary>
        public float Progress;
        
        /// <summary>
        /// Time entity takes to move from source to destination.
        /// </summary>
        public float MovementTime;
    }
}