using System;
using UnityEngine;

namespace Rendering.Scripts
{
    [RequireComponent(typeof(Camera))]
    public abstract class CameraListener : MonoBehaviour
    {
        protected Camera mainCamera;
        protected Vector2Int resolution;
        private bool _rebuildRequested;
        
        protected abstract void Rebuild();

        public virtual void Awake()
        {
            mainCamera = GetComponent<Camera>();
            resolution = new Vector2Int(Screen.width, Screen.height);
        }

        public virtual void Start()
        {
            Rebuild();
        }

        public virtual void Update()
        {
            _rebuildRequested |= resolution.x != Screen.width || resolution.y != Screen.height;
            if (!_rebuildRequested) return;

            resolution = new Vector2Int(Screen.width, Screen.height);

            Rebuild();
        }

        protected virtual void OnValidate()
        {
            RequestRebuild();
        }

        public void RequestRebuild()
        {
            _rebuildRequested = true;
        }
    }
}