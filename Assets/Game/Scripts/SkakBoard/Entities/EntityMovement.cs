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

        private void Awake()
        {
            _entity = GetComponent<BoardEntity>();
        }

        private void Update()
        {
            if (!(movement is { } m)) return;

            if (!_entity.IsPlaced)
            {
                Debug.LogError("Trying to move board which isn't placed on board");
                return;
            }

            m.Progress += Time.deltaTime / m.MovementTime;

            Vector3 source = m.Source;
            Vector3 dest = _entity.board.GetPositionFor(m.Destination);
            Vector3 up = SkakGlobals.Instance.up * jumpCurve.Evaluate(m.Progress); 

            if (m.Progress >= 1)
            {
                OnMovementFinished?.Invoke(m);
                movement = null;
                transform.position = dest;
            }
            else
            {
                transform.position = Vector3.Lerp(source, dest, m.Progress) + up;
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