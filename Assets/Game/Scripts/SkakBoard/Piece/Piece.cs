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
        private static readonly int ColorNumber = Shader.PropertyToID("colorNumber");
        
        private SpriteRenderer _renderer;
        
        public PieceDescription piece;
        public int color;

        public Eyes eyes;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            _renderer.material.SetFloat(ColorNumber, color);
            eyes.color = color;
        }
    }
}