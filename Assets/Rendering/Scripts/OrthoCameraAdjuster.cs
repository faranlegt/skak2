using UnityEngine;

namespace Rendering.Scripts
{
    [RequireComponent(typeof(Camera))]
    public class OrthoCameraAdjuster : MonoBehaviour
    {
        public int ppu = 16;
        public int units = 6;

        private void Awake()
        {
            Adjust();
        }

        private void OnValidate()
        {
           Adjust(); 
        }

        private void Adjust()
        {
            var cam = GetComponent<Camera>();
            
            var maxUnits = Screen.height / ppu;
            int scale = maxUnits / units;
            var originResolution = maxUnits * ppu;
            
            cam.orthographicSize = Screen.height / (float)ppu;
        }
    }
}