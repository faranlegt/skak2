using System;
using Game.Scripts.Renderer;
using Game.Scripts.SkakBoard.Generators;
using Popcron.Console;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Game.Scripts.SkakBoard.Management
{
    [RequireComponent(typeof(EntityManager))]
    [RequireComponent(typeof(Squares))]
    public class Board : MonoBehaviour
    {
        [HideInInspector] public EntityManager entityManager;
        [HideInInspector] public Squares squares;
        [HideInInspector] public BoardSpawner spawner;
        public Sorting sorting;

        public int Size { get; private set; }

        public BoardGenerator boardGenerator;
        
        public float squareSize = 1;

        private void Awake()
        {
            sorting = new Sorting(this);
            entityManager = GetComponent<EntityManager>();
            squares = GetComponent<Squares>();
            spawner = GetComponent<BoardSpawner>();
        }

        private void Start()
        {
            Assert.IsNotNull(boardGenerator, "Board generator wasn't set. Board will not be generated.");

            BuildBoard();
        }

        public void OnValidate()
        {
            if (squares)
            {
                squares.SyncSquaresPositions();
            }
        }

        public Vector3 GetPositionFor(Vector2Int p) => GetPositionFor(p.x, p.y);

        public Vector3 GetPositionFor(int x, int y) =>
            transform.position +
            new Vector3((x - Size / 2f + 0.5f) * squareSize, (y - Size / 2f + 0.5f) * squareSize, 0);

        private void BuildBoard()
        {
            Size = boardGenerator.size;

            squares.Generate(boardGenerator);
            squares.SyncSquaresPositions();
        }

        public Vector3 ToBoard(Vector3 world)
        {
            float centerOffset = Size / 2f;
            Vector3 board = new Vector3(centerOffset - 0.5f, centerOffset - 0.5f) + (world - transform.position) / squareSize;
            board.z = 0;
            return board;
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                var cell = ToBoard(mousePos);

                Debug.Log(cell);
            }
        }
    }
}