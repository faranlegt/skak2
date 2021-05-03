using Game.Scripts.SkakBoard.Entities;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(EntityMovement))]
    [CanEditMultipleObjects]
    public class MovementInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Move"))
            {
                var movementWizard = (MovementWizard)ScriptableWizard.DisplayWizard(
                    "Move entity",
                    typeof(MovementWizard),
                    "Move"
                );
                movementWizard.entityMovement = (EntityMovement) target;
            }
        }
    }
}