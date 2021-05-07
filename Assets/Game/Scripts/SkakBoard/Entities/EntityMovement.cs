using System;
using Game.Scripts.Models;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Entities
{
    public delegate void MovementCallback(Movement m);
    
    [RequireComponent(typeof(BoardEntity))]
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

            m.Progress += Time.deltaTime / m.MovementTime;

            Vector3 source = m.Source;
            Vector3 dest = _entity.Board.GetPositionFor(m.Destination);
            Vector3 up = SkakGlobals.Instance.up * jumpCurve.Evaluate(m.Progress); 
            
            transform.position = Vector3.Lerp(source, dest, m.Progress) + up;

            if (m.Progress >= 1)
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