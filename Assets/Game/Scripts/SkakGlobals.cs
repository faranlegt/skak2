using UnityEngine;

namespace Game.Scripts
{
    public class SkakGlobals : MonoBehaviour
    {
        public static SkakGlobals Instance { get; private set; }
    
        private static readonly int ShadowHeightCoefficient = Shader.PropertyToID("_ShadowHeightCoefficient");
        private static readonly int HeightCoefficient = Shader.PropertyToID("_HeightCoefficient");
        private static readonly int ShadowColor = Shader.PropertyToID("_ShadowColor");
    
        public float shadowHeightCoefficient = 0;
        public float heightCoefficient = 1f;
        public Color shadowColor = Color.black;
        
        public Vector3 up = Vector3.back;

        private void Awake()
        {
            if (Instance)
            {
                Debug.LogWarning("Skak globals instance is set already. Destroying duplicate");
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            SetShaderGlobals();
        }

        private void OnValidate()
        {
            SetShaderGlobals();
        }

        private void SetShaderGlobals()
        {
            Shader.SetGlobalFloat(ShadowHeightCoefficient, shadowHeightCoefficient);
            Shader.SetGlobalFloat(HeightCoefficient, heightCoefficient);
            Shader.SetGlobalColor(ShadowColor, shadowColor);
        }
    }
}