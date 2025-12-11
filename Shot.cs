using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;

namespace Asteroido
{
    public class Shot : GameObjects
    {

        const float shotWidth = 10.0f;
        const float shotLenght = 15.0f;
        const float ShotSpd = 15.0f;
        double creationTime;
        const float tempoTiro = 10.0f;
        Vector2 positionShoot;
        public static Texture2D Texture;
        public static Sound Sounds;


        public bool EstarAtivo { get; private set; }

        public Shot(Vector2 pos, float rot, Vector2 FacingDirection) : base(pos, rot)
        {
            positionShoot = FacingDirection;
            EstaAtivo = true;
            creationTime = Raylib.GetTime();
  

        }

       
        public static void GetResources() 
        {
            Sounds = Raylib.LoadSound(@"resource\laser-45816.mp3");
        }
        public static void UnloadResources()
        {
            Raylib.UnloadSound(Sounds);
        }

        public override void Draw() 
        {
 
            Rectangle shot = new Rectangle(Position.X, Position.Y, shotWidth, shotLenght);
            Vector2 origin = new (shot.Width / 2, shot.Height / 2);

            Raylib.DrawRectanglePro(shot, origin, Rotation, Color.Green);

        }

        public override void Update()
        {
            if (EstaAtivo)
            {
                Raylib.PlaySound(Sounds);
                EstaAtivo = false;
            }
            ShotUpdate((float)Raylib.GetTime());
            hitBox = new Rectangle(Position.X, Position.Y, shotWidth, shotLenght);
        }


        public bool ShotUpdate(float time)
        {


            if (EstarAtivo)
            {
                return false;
            }

            Position += positionShoot*ShotSpd;
            if(time > creationTime +tempoTiro || Raylib.CheckCollisionPointRec(Position, SimpleMaths.GetScreenArea()))
            {
                return false;
            }

            return true;
        }
            
    }
}
