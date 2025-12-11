using Raylib_cs;
using System.Numerics;

namespace Asteroido
{
    public class Asteroids : GameObjects
    {
        const int asteroidSpeedMin = 5;
        const int asteroidSpeedMax = 240;
        float asteroidSpeed;
        const float asteroidHP = 10.0f;
        public int radius;
        public static Texture2D Texture { get; set; }
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



        public Asteroids(Vector2 pos, float rot, Vector2 PlayerPos) : base(GetRandomPosition(), GetRandomMetRot())
        {
            playerPos = PlayerPos;

        }



        public override void Draw()
        {


            lado1 = currentFrame * Texture.Width / 4;
            source = new Rectangle(lado1, lado2, Texture.Width / 4, Texture.Height);
            dest = new Rectangle(Position.X, Position.Y, Texture.Width / 4, Texture.Height);
            origin = new Vector2(dest.Width / 2, dest.Height / 2);
            Raylib.DrawTexturePro(Texture, source, dest, origin, Rotation, color);

        }

        public override void Update()
        {
            Vector2 asteroidSpeed = new(Raylib.GetRandomValue(asteroidSpeedMin, asteroidSpeedMax), Raylib.GetRandomValue(asteroidSpeedMin, asteroidSpeedMax));
            Position += asteroidSpeed * SimpleMaths.GetFacingDirection(Rotation) * Raylib.GetFrameTime();


            frameCounter++;
            if (frameCounter >= (60 / framesSpeed))
            {
                frameCounter = 0;
                currentFrame++;
                if (currentFrame >= 4) currentFrame = 0;

            }
            hitBox = new Rectangle(Position.X, Position.Y, Texture.Width / 4, Texture.Height);

        }

        enum AsteSize
        {
            Small = 1,
            Medium = 2,
            Large = 4,

        }

        public static void GetResources()
        {

            Texture = Raylib.LoadTexture(@"resource\roids_large.png");
        }



        public static void UnloadResources()
        {
            Raylib.UnloadTexture(Texture);
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
            int maxNumber = Enum.GetValues(typeof(AsteSize)).Length;
            int randomNumber = rnd.Next(0, maxNumber - 1);
            return 16.0f * randomNumber;
        }
    }
}
