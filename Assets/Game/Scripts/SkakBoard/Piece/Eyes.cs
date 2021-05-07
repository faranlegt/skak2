using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.SkakBoard.Piece
{
    public class Eyes : MonoBehaviour
    {
        private Piece _piece;

        public Vector3 leftWatchingPoint = Vector2.zero;
        public Vector3 rightWatchingPoint = Vector3.zero;

        public Eye left;
        public Eye right;

        public bool watchMouse = false;
        public bool pixelSnapping = true;
        public Vector2 pixelSnapOffset;

        public float yCoefficient = 0.5f;

        private void Awake()
        {
            _piece = GetComponentInParent<Piece>();
        }

        private void Start()
        {
            SetPositions();
        }

        private void Update()
        {
            SetPositions();

            if (watchMouse)
            {
                leftWatchingPoint = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                leftWatchingPoint.z = transform.position.z;
                rightWatchingPoint = leftWatchingPoint;
            }
        }

        private void SetPositions()
        {
            transform.localPosition = new Vector3(0, _piece.piece.eyesBaseLevel, 0);
            
            
            Vector3 leftDirection = leftWatchingPoint - transform.position;
            Vector3 rightDirection = leftWatchingPoint - transform.position;

            float leftAngle = Vector3.SignedAngle(Vector3.down, leftDirection, Vector3.forward);
            float rightAngle = Vector3.SignedAngle(Vector3.down, rightDirection, Vector3.forward);
            float missingAnglesDiff = _piece.piece.angleBetweenEyes - Mathf.Abs(leftAngle - rightAngle);

            if (missingAnglesDiff > 0)
            {
                leftAngle += missingAnglesDiff;
                rightAngle -= missingAnglesDiff;
            }

            if (!left.eyeFixed)
            {
                left.transform.localPosition = GetEyePosition(leftAngle);
            }
            
            if (!right.eyeFixed)
            {
                right.transform.localPosition = GetEyePosition(rightAngle);
            }

            left.watchingPoint = leftWatchingPoint;
            right.watchingPoint = rightWatchingPoint;

            left.isFrontEye = left.transform.localPosition.y > right.transform.localPosition.y;
            right.isFrontEye = !left.isFrontEye;
        }

        private Vector3 GetEyePosition(float a)
        {
            float rad = a * Mathf.Deg2Rad;
            float x = Mathf.Sin(rad);
            float y = Mathf.Cos(rad) * yCoefficient;

            var freePos = new Vector3(x, y, 0) * _piece.piece.eyesRadius;
            if (pixelSnapping)
            {
                freePos = freePos.PixelSnap(transform.localPosition, pixelSnapOffset);
            }

            return freePos;
        }
    }
}