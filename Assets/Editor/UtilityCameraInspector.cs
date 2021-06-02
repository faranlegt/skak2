using Rendering.Scripts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(UtilityCamera))]
    public class UtilityCameraInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // var camera = target as UtilityCamera;
            //
            // if (!camera!.isActiveAndEnabled || !Application.isPlaying)
            // {
            //     return;
            // }
            //
            // var texture = camera!.GetRenderedTexture();
            // GUILayout.Label(texture);
        }
    }
}