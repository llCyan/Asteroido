using Raylib_cs;
using System.Numerics;

namespace Asteroido
{
    public class Asteroids : GameObjects
    {
        const int asteroidSpeedMin = 5;
        const int asteroidSpeedMax = 240;
        public int radius;
        static Texture2D asteroidLarge;
        static Texture2D asteroidSmall;
        static Texture2D asteroidMedium;
        Texture2D Texture;
        public static Sound meteorHit;
        public static Sound Sounds;
        int currentFrame = 0;
        int frameCounter = 0;
        int framesSpeed = 3;
        float lado1 = 0.0f, lado2 = 0.0f;
        Rectangle source;
        Rectangle dest;
        Vector2 origin;
        Color color = Color.White;
        static Vector2 playerPos;
        bool playerSound;



        public Asteroids(Vector2 PlayerPos, MeteorSize picked, bool Destroyed, Vector2 lastPos) : base(GetRandomPosition(), GetRandomMetRot())
        {
            playerPos = PlayerPos;
            if (picked == MeteorSize.Small)
            {
                NewMeteorSize = MeteorSize.Small;
            }
            if (picked == MeteorSize.Medium)
            {
                NewMeteorSize = MeteorSize.Medium;
            }
            else if (picked == MeteorSize.Random)
            {
                NewMeteorSize = RandomSize();
            }

            if (Destroyed)
            {
                Position = Raymath.Vector2AddValue(lastPos, 20);
                playerSound = Destroyed;
            }
        }



        public override void Draw()
        {

            CallMeteorite();
            if (playerSound)
            {
                Raylib.PlaySound(meteorHit);
            }

        }

        public override void Update()
        {
            Speedmvn = new(Raylib.GetRandomValue(asteroidSpeedMin, asteroidSpeedMax), Raylib.GetRandomValue(asteroidSpeedMin, asteroidSpeedMax));
            Position += Speedmvn * SimpleMaths.GetFacingDirection(Rotation) * Raylib.GetFrameTime();



            frameCounter++;
            if (frameCounter >= (60 / framesSpeed))
            {
                frameCounter = 0;
                currentFrame++;
                if (currentFrame >= 4) currentFrame = 0;

            }
            HitBox = new Rectangle(Position.X, Position.Y, Texture.Width / 4, Texture.Height);

        }


        public void CallMeteorite()
        {


            if (NewMeteorSize == MeteorSize.Large)
            {
                Texture = asteroidLarge;
            }
            else if (NewMeteorSize == MeteorSize.Medium)
            {
                Texture = asteroidMedium;
            }
            else if (NewMeteorSize == MeteorSize.Small)
            {
                Texture = asteroidSmall;
            }

            lado1 = currentFrame * asteroidLarge.Width / 4;
            source = new Rectangle(lado1, lado2, Texture.Width / 4, Texture.Height);
            dest = new Rectangle(Position.X, Position.Y, Texture.Width / 2, Texture.Height * 2);
            origin = new Vector2(dest.Width / 2, dest.Height / 2);
            Raylib.DrawTexturePro(Texture, source, dest, origin, Rotation, color);
        }
        public enum MeteorSize
        {
            Small,
            Medium,
            Large,
            Random

        }
        public MeteorSize RandomSize()
        {
            Random rnd = new Random();
            int maxNumber = Enum.GetValues(typeof(MeteorSize)).Length - 1;
            int randomNumber = rnd.Next(0, maxNumber);
            return (MeteorSize)randomNumber;
        }

        public static void GetResources()
        {

            asteroidLarge = Raylib.LoadTexture(@"resource\roids_large.png");
            asteroidMedium = Raylib.LoadTexture(@"resource\roids_medium.png");
            asteroidSmall = Raylib.LoadTexture(@"resource\roids_small.png");
            meteorHit = Raylib.LoadSound(@"resource\HitSound.mp3");

        }



        public static void UnloadResources()
        {
            Raylib.UnloadTexture(asteroidLarge);
            Raylib.UnloadTexture(asteroidMedium);
            Raylib.UnloadTexture(asteroidSmall);
        }


        public static Vector2 GetRandomPosition()
        {
            int outOfScreen = 300;
            int ScreenX = (int)playerPos.X;
            int ScreenY = (int)playerPos.Y;
            int SideDecider = Raylib.GetRandomValue(0, 3);

            Vector2 SpawnPos = playerPos;

            if (SideDecider == 0) // Top
            {
                SpawnPos = new Vector2(Raylib.GetRandomValue(ScreenX - outOfScreen, ScreenX) + outOfScreen, ScreenY - outOfScreen);
            }
            else if (SideDecider == 1) // Right
            {
                SpawnPos = new Vector2((int)ScreenX + outOfScreen, Raylib.GetRandomValue(ScreenY - outOfScreen, ScreenY + outOfScreen));
            }
            else if (SideDecider == 2) // Bottom
            {
                SpawnPos = new Vector2(Raylib.GetRandomValue(ScreenX - outOfScreen, ScreenX + outOfScreen), ScreenY + outOfScreen);
            }
            else if (SideDecider == 3) // Left
            {
                SpawnPos = new Vector2(ScreenX - outOfScreen, Raylib.GetRandomValue(ScreenY - outOfScreen, ScreenY + outOfScreen));
            }



            return SpawnPos;
        }


        public static float GetRandomMetRot()
        {


            return (float)Raylib.GetRandomValue(0, 360);
        }


        public static float GetAsteroidRadius()
        {
            Random rnd = new Random();
            int maxNumber = Enum.GetValues(typeof(MeteorSize)).Length;
            int randomNumber = rnd.Next(0, maxNumber - 1);
            return 16.0f * randomNumber;
        }
    }
}
