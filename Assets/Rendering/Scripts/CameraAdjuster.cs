using UnityEngine;

namespace Rendering.Scripts
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class CameraAdjuster : CameraListener
    {
        [Range(1, 5)]
        public int scale = 1;

        private UtilityCamera[] _utilityCameras;

        public override void Awake()
        {
            base.Awake();
            
            _utilityCameras = GetComponents<UtilityCamera>();
        }

        protected override void Rebuild()
        {   
            mainCamera.orthographicSize = resolution.y / (scale * 16f * 2f);

            foreach (var utilityCamera in _utilityCameras)
            {
                utilityCamera.RequestRebuild();
            }
        }
    }
}
