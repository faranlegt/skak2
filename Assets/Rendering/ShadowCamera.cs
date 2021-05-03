using System;
using UnityEngine;

namespace Rendering
{
    [ExecuteInEditMode]
    public class ShadowCamera : MonoBehaviour
    {
        private static readonly int PiecesShadowMapPropertyId = Shader.PropertyToID("_PiecesShadowMap");
        private Camera _camera;
        private RenderTexture _shadowMap;


        public Camera mainCamera;

        private void Start() => Rebuild();

        private void OnValidate() => Rebuild();

        private void OnEnable() => Rebuild();

        private void Rebuild()
        {
            if (!mainCamera)
            {
                Debug.LogError("Main camera is not set");
                return;
            }
            
            _camera = GetComponent<Camera>();
            if (_shadowMap)
            {
                _shadowMap.Release();
            }

            _shadowMap = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32)
            {
                filterMode = FilterMode.Point
            };
            _shadowMap.Create();
            _camera.targetTexture = _shadowMap;
            
            Shader.SetGlobalTexture(PiecesShadowMapPropertyId, _shadowMap);
        }
    }
}