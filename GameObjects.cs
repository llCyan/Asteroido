using Raylib_cs;
using System.Numerics;


namespace Asteroido
{
    public abstract class GameObjects
    {
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public int RotationSpeed { get; set; }
        public bool EstaAtivo { get; set; } = false;
        public Rectangle hitBox { get; set; }
        
        public int MeteorSizePicked { get; set; }

        public GameObjects(Vector2 posinit, float rot)
        {
            Position = posinit;
            Rotation = rot;

        }

        public abstract void Update();

        public abstract void Draw();


    }
}
