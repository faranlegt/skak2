using Popcron.Console;
using UnityEngine;

namespace SharedRuntime
{
    public class SkakGlobals : MonoBehaviour
    {
        public static SkakGlobals Instance => _instance != null 
                ? _instance 
                : Application.isPlaying 
                    ? _instance = FindObjectOfType<SkakGlobals>() 
                    : null;
        
        private static SkakGlobals _instance;
    
        private static readonly int ShadowHeightCoefficient = Shader.PropertyToID("_ShadowHeightCoefficient");
        private static readonly int HeightCoefficient = Shader.PropertyToID("_HeightCoefficient");
        private static readonly int ShadowColor = Shader.PropertyToID("_ShadowColor");
    
        public float shadowHeightCoefficient = 0;
        public float heightCoefficient = 1f;
        public Color shadowColor = Color.black;
        public int pixelPerUnit = 16;
        
        public Vector3 up = Vector3.forward;

        public Vector3 SnapGrid => new Vector2(1f / pixelPerUnit, 1f / pixelPerUnit); 

        private void Awake()
        {
            Debug.Assert( 
                _instance == null || _instance == this,
                "More than one singleton instance instantiated!", 
                this
            );
            
            _instance = this;
            
            SetShaderGlobals();
        }

        private void Start()
        {
            Console.IsOpen = false;
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        private void OnEnable()
        {
            Parser.Register(this, "globals");
        }

        private void OnDisable()
        {
            Parser.Unregister(this);
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