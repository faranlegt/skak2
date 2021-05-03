using System;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Entities
{
    public delegate void MovementCallback(Movement m);
    
    public class EntityMovement : MonoBehaviour
    {
        private BoardEntity _entity;

        public AnimationCurve jumpCurve;
        public Movement movement;

        private void Start()
        {
            _entity = GetComponent<BoardEntity>();
        }

        private void Update()
        {
            if (!(movement is { } m)) return;

            m.progress += Time.deltaTime / m.movementTime;

            Vector3 source = m.source;
            Vector3 dest = _entity.Board.GetPositionFor(m.destination);
            Vector3 up = SkakGlobals.Instance.up * jumpCurve.Evaluate(m.progress); 
            
            transform.position = Vector3.Lerp(source, dest, m.progress) + up;

            if (m.progress >= 1)
            {
                OnMovementFinished?.Invoke(m);
                movement = null;
            }
        }

        public void Move(Movement m)
        {
            movement = m;
            
            OnMovementStarted?.Invoke(m);
        }

        public event MovementCallback OnMovementStarted;
        public event MovementCallback OnMovementFinished;
    }
}