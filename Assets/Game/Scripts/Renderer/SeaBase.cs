using System;
using UnityEngine;

namespace Game.Scripts.Renderer
{
    public class SeaBase : MonoBehaviour
    {
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void Update()
        {
            var t = transform;
            var parentZ = t.parent.transform.position.z;
            var seaLevel = Sea.Instance.transform.position.z;
            
            _renderer.enabled = seaLevel < parentZ;
            var pos = t.position;
            pos.z = seaLevel - parentZ;
            t.position = pos;
        }
    }
}
