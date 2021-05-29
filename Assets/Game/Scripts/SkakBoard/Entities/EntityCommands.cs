using System;
using Game.Scripts.Models;
using Popcron.Console;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Entities
{
    [Category("Entities")]
    [RequireComponent(typeof(BoardEntity))]
    public class EntityCommands : MonoBehaviour
    {
        private BoardEntity _entity;
        private EntityMovement _movement;
        private bool _canMove;
        private Piece.Piece _piece;
        private bool _isPiece;

        private void Start()
        {
            _entity = GetComponent<BoardEntity>();
            _canMove = TryGetComponent(out _movement);
            _isPiece = TryGetComponent(out _piece);
            
            Parser.Register(this, $"e{_entity.id}");
        }

        private void OnDestroy()
        {
            Parser.Unregister(this);
        }

        [Command("make_movable")]
        public bool MakeMovable()
        {
            if (_canMove) return false;
            _movement = gameObject.AddComponent<EntityMovement>();

            return _canMove = true;
        }

        [Alias("m")]
        [Command("move")]
        public string Move(int x, int y) => Move(x, y, 1);

        [Alias("mt")]
        [Command("move_t")]
        public string Move(int x, int y, float time)
        {
            
            if (!_canMove) return "This entity is static";

            var m = new Movement
            {
                Destination = new Vector2Int(x, y),
                MovementTime = time,
                Source = _entity.transform.position
            };

            _movement.Move(m);

            return "Whooh";
        }

        [Alias("c")]
        [Command("color")]
        public void ChangeColor(int color)
        {
            if (!_isPiece) return;

            _piece.color = color;
        }

        [Alias("w")]
        [Command("watch_mouse")]
        public void WatchMouse()
        {
            if (!_isPiece) return;

            _piece.eyes.watchMouse = true;
        }
    }
}