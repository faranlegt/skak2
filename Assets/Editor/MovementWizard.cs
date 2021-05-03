using Game.Scripts.SkakBoard.Entities;
using Game.Scripts.SkakBoard.Management;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class MovementWizard : ScriptableWizard
    {
        public EntityMovement entityMovement;

        public Vector2Int destination;

        [Range(0.1f, 5f)] public float timeToMove = 1;

        public void OnWizardUpdate()
        {
            helpString = "Moves entity on board";
            
            errorString = "";
            isValid = true;
            
            if (!entityMovement)
            {
                errorString = "Please assign an entity";
                isValid = false;
                return;
            }

            bool destWalkable = entityMovement
                .GetComponent<BoardEntity>()
                .Board
                .GetComponent<Squares>()
                .IsWalkable(destination);

            if (!destWalkable)
            {
                errorString = "Destination is not walkable";
                isValid = false;
            }
        }

        public void OnWizardCreate()
        {
            entityMovement.Move(new Movement()
            {
                destination = destination,
                source = entityMovement.transform.position,
                movementTime = timeToMove
            });
        }
    }
}