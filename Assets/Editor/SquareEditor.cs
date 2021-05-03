using Game.Scripts.SkakBoard.Squares;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(Square))]
    [CanEditMultipleObjects]
    public class SquareEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Brake"))
            {
                foreach (Object t in targets)
                {
                    var square = (Square) t;

                    square.Brake();
                }
            }
        }
    }
}