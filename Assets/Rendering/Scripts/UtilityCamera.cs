using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Rendering.Scripts
{
    public class UtilityCamera : CameraListener
    {
        public Color backgroundColor;
        public string globalTextureName;
        public LayerMask cullingMask;
        public int customRendererIndex;

        private Camera _createdCamera;
        private RenderTexture _renderTexture;

        public override void Start()
        {
            if (_createdCamera) return;
            
            var createdCameraObject = new GameObject();
            createdCameraObject.transform.parent = gameObject.transform;
            createdCameraObject.name = globalTextureName;

            _createdCamera = createdCameraObject.AddComponent<Camera>();
            createdCameraObject.AddComponent<UniversalAdditionalCameraData>();

            base.Start();
        }

        protected override void Rebuild()
        {
            if (!_createdCamera)
            {
                return;
            }
            
            _createdCamera.CopyFrom(mainCamera);
            _createdCamera.backgroundColor = backgroundColor;
            _createdCamera.cullingMask = cullingMask.value;

            var additionalCameraData = _createdCamera.GetUniversalAdditionalCameraData();
            additionalCameraData.SetRenderer(customRendererIndex);

            if (_renderTexture)
            {
                _renderTexture.Release();
            }
            
            _renderTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32)
            {
                filterMode = FilterMode.Point
            };
            _renderTexture.Create();
            _createdCamera.targetTexture = _renderTexture;
            
            _renderTexture.SetGlobalShaderProperty(globalTextureName);
        }

        public Texture2D GetRenderedTexture()
        {
            var texture = new Texture2D(_renderTexture.width, _renderTexture.height);
            RenderTexture.active = _renderTexture;
            texture.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
            texture.Apply();
            return texture;
        }
    }
}