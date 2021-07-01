using SharedRuntime;
using UnityEngine;

namespace Game.Scripts
{
    public static class VectorUtils
    {
        public static Vector3 Snap(this Vector3 v, Vector2 grid) =>
            v.Snap(grid, Vector2.zero, Vector2.zero);
        public static Vector3 Snap(this Vector3 v, Vector2 grid, Vector3 offset, Vector2 pixelOffset)
        {
            Vector3 rounded = Vector3Int.RoundToInt((v - offset) / grid - pixelOffset);
            rounded += (Vector3)pixelOffset;
            rounded *= grid;
            rounded.z = v.z;
            return rounded + offset;
        }

        public static Vector3 PixelSnap(this Vector3 v, Vector3 offset, Vector2 pixelOffset) =>
            v.Snap(SkakGlobals.Instance.SnapGrid, offset, pixelOffset);

        public static Vector3 PixelSnap(this Vector3 v, Vector3 offset) =>
            v.PixelSnap(offset, Vector2.zero);

        public static Vector3 PixelSnap(this Vector3 v) =>
            v.PixelSnap(Vector3.zero, Vector2.zero);
    }
}