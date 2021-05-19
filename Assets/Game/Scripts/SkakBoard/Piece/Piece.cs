using System;
using Game.Scripts.Models;
using Game.Scripts.SkakBoard.Entities;
using UnityEngine;

namespace Game.Scripts.SkakBoard.Piece
{
    [RequireComponent(typeof(BoardEntity))]
    [RequireComponent(typeof(EntityMovement))]
    public class Piece : MonoBehaviour
    {
        private Eyes _eyes;
        public PieceDescription piece;

        private void Awake()
        {
            _eyes = GetComponentInChildren<Eyes>();
        }
    }
}