using Raylib_cs;
using System.Numerics;

namespace Asteroido
{
    public class Shot : GameObjects
    {

        const float shotWidth = 25.0f;
        const float shotLenght = 25.0f;
        const float ShotSpd = 8.0f;
        Vector2 positionShoot;
        public static Texture2D Texture;
        public static Sound Sounds;


        public bool EstarAtivo { get; private set; }

        public Shot(Vector2 pos, float rot, Vector2 FacingDirection) : base(pos, rot)
        {
            positionShoot = FacingDirection;
            EstaAtivo = true;


        }


        public static void GetResources()
        {
            Sounds = Raylib.LoadSound(@"resource\laser-45816.mp3");
            Texture = Raylib.LoadTexture(@"resource\Projectiles.png");
        }
        public static void UnloadResources()
        {
            Raylib.UnloadSound(Sounds);
            Raylib.UnloadTexture(Texture);
        }

        public override void Draw()
        {
            Rectangle source = new(39, 269, 49, 76);
            Rectangle shot = new Rectangle(Position.X, Position.Y, shotWidth, shotLenght);
            Vector2 origin = new(shot.Width / 2, shot.Height / 2);

            Raylib.DrawTexturePro(Texture,source, shot, origin, Rotation, Color.RayWhite);

        }

        public override void Update()
        {
            if (EstaAtivo)
            {
                Raylib.PlaySound(Sounds);
                EstaAtivo = false;
            }
            ShotUpdate();
            hitBox = new Rectangle(Position.X, Position.Y, shotWidth, shotLenght);
        }


        public void ShotUpdate()
        {

            Position += positionShoot * ShotSpd;
    
        }

    }
}
