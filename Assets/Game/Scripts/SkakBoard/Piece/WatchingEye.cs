using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.SkakBoard.Piece
{
    [RequireComponent(typeof(Eye))]
    public class WatchingEye : MonoBehaviour
    {
        private Eye _eye;

        private void Awake()
        {
            _eye = GetComponent<Eye>();
        }

        private void Update()
        {
            if (!Camera.main)
            {
                return;
            }
            var watchingPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            watchingPoint.z = transform.position.z;
            
            Vector3 direction = watchingPoint - transform.position;

            float angle = Vector3.SignedAngle(Vector3.down, direction, Vector3.forward);

            _eye.angleForSkin = angle;
            _eye.watchingPoint = watchingPoint;
        }
    }
}