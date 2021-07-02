using System.Linq;
using SharedRuntime;
using UnityEngine;
using UnityEngine.Rendering;

namespace Rendering.Scripts
{
    public class HeightModificatorManager : MonoBehaviour
    {
        private HeightModificator[] _cache;
        
        private void Start()
        {
            RenderPipelineManager.beginFrameRendering += BeginFrameRendering;
            RenderPipelineManager.endFrameRendering += EndFrameRendering;
        }

        private void BeginFrameRendering(ScriptableRenderContext ctx, Camera[] cameras)
        {
            _cache = FindObjectsOfType<HeightModificator>()
                .Where(m => m.enabled)
                .OrderBy(m => m.priority)
                .ToArray();

            foreach (var modificator in _cache)
            {
                var t = modificator.transform;
                var pos = t.position;
                modificator.original = pos;
                pos -= Offset(pos.z);

                if (modificator.snap)
                {
                    pos.z = Mathf.Round(pos.z * SkakGlobals.Instance.pixelPerUnit) / SkakGlobals.Instance.pixelPerUnit;
                }
                
                t.position = pos;
            }
        }

        private Vector3 Offset(float z) => new Vector3(0, z * SkakGlobals.Instance.heightCoefficient);

        private void EndFrameRendering(ScriptableRenderContext ctx, Camera[] cameras)
        {
            foreach (var modificator in _cache.Reverse())
            {
                modificator.transform.position = modificator.original;
            }
        }
    }
}