using Game.Scripts.SkakBoard.Generators;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Scripts.SkakBoard.Management
{
    [RequireComponent(typeof(EntityManager))]
    [RequireComponent(typeof(Squares))]
    public class Board : MonoBehaviour
    {
        private EntityManager _entityManager;
        private Squares _squares;

        public int Size { get; private set; }

        public BoardGenerator boardGenerator;
        public float squareSize = 1;

        private void Awake()
        {
            _entityManager = GetComponent<EntityManager>();
            _squares = GetComponent<Squares>();
        }

        private void Start()
        {
            Assert.IsNotNull(boardGenerator, "Board generator wasn't set. Board will not be generated.");

            BuildBoard();
        }

        public void OnValidate()
        {
            if (_squares)
            {
                _squares.SyncSquaresPositions();
            }
        }

        public Vector3 GetPositionFor(Vector2Int p) => GetPositionFor(p.x, p.y);
        
        public Vector3 GetPositionFor(int x, int y) => 
            transform.position + new Vector3((x - Size / 2f + 0.5f) * squareSize, (y - Size / 2f + 0.5f) * squareSize, 0);

        private void BuildBoard()
        {
            Size = boardGenerator.size;
            
            _squares.Generate(boardGenerator);
            _squares.SyncSquaresPositions();
        }
    }
}