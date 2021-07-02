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
            float seaLevel = Sea.Instance.transform.position.z;
            _renderer.enabled = seaLevel > t.parent.transform.position.z;
            var pos = t.position;
            pos.z = seaLevel;
            t.position = pos;
        }
    }
}
