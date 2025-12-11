using Raylib_cs;
using System.Numerics;

namespace Asteroido
{
    internal static class SimpleMaths
    {
        public static float GetRad(float rotation)
        {
            return rotation * ((float)Math.PI / 180.0f);
        }

        public static Raylib_cs.Rectangle GetScreenArea()
        {
            Raylib_cs.Rectangle area = new(0, 0, Raylib.GetRenderWidth(), Raylib.GetRenderHeight());
            return area;
        }

        public static Vector2 GetFacingDirection(float Rotation)
        {
            float rad = SimpleMaths.GetRad(Rotation);
            Vector2 pos = new(0, -1);
            return Raymath.Vector2Rotate(pos, rad);
        }


    }
}
