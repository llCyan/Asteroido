using Raylib_cs;
using System.Numerics;
using static Asteroido.Asteroids;


namespace Asteroido
{
    public abstract class GameObjects
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public int RotationSpeed { get; set; }
        public Vector2 Speedmvn { get; set; }
        public bool EstaAtivo { get; set; } = false;
        public Rectangle HitBox { get; set; }

        public MeteorSize NewMeteorSize { get; set; }

        public GameObjects(Vector2 posinit, float rot)
        {
            Position = posinit;
            Rotation = rot;

        }

        public abstract void Update();

        public abstract void Draw();


    }
}
