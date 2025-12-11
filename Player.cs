using Raylib_cs;
using System.Numerics;

namespace Asteroido
{
    public class Player : GameObjects
    {

        Vector2 speedmvn;
        int rotationSpeed = 360;
        const float radius = 32;
        const int PLAYER_SPEED = 250;
        const int PLAYER_ACCELERATION = 750;
        const int PLAYER_DECELERATION = 175;
        static Texture2D Texture;
        public static Sound Sounds;

        Vector2 origin;
        Color color = Color.White;
        float lado1 = 0.0f, lado2 = 0.0f;


        public Player(Vector2 Pos, float rot) : base(Pos, rot)
        {




        }


        public override void Draw()
        {

            Rectangle source = new(lado1, lado2, Texture.Width, Texture.Height);
            Rectangle dest = new(Position.X, Position.Y, radius * 2, radius * 2);
            origin = new(dest.Width / 2, dest.Height / 2);
            Raylib.DrawTexturePro(Texture, source, dest, origin, Rotation, color);

        }
        public static void GetResources()
        {
            Texture = Raylib.LoadTexture(@"resource\ship32.png");
            Sounds = Raylib.LoadSound(@"resource\low-spaceship-rumble-195722.mp3");
        }
        public static void UnloadResources()
        {
            Raylib.UnloadTexture(Texture);
            Raylib.UnloadSound(Sounds);
        }

        public override void Update()
        {
            PlayerMove(Sounds);

        }

        public void UpdateSpeed(float frame, Sound SomMovimento)
        {
            float rad = SimpleMaths.GetRad(Rotation);
            float magSqr = Raymath.Vector2LengthSqr(speedmvn);
            float mag = (float)Math.Sqrt(magSqr);
            Vector2 FacingDirection;
            bool playSound = false;


            FacingDirection = GetFacingDirection();

            if (playSound)
            {
            }
            if (Raylib.IsKeyDown(KeyboardKey.W))
            {

                Raylib.SetSoundVolume(SomMovimento, 0.3f);
                if (Raylib.IsKeyPressed(KeyboardKey.W))
                {
                    Raylib.PlaySound(SomMovimento);
                    Raylib.SetSoundVolume(SomMovimento, 0.8f);

                }

                speedmvn = Raymath.Vector2Add(speedmvn, Raymath.Vector2Scale(FacingDirection, (PLAYER_ACCELERATION * frame)));

                if (mag > PLAYER_SPEED)
                {
                    speedmvn = Raymath.Vector2Scale(speedmvn, PLAYER_SPEED / mag);
                }
            }
            else
            {
                if (mag > 0)
                {
                    float xSign = (speedmvn.X < 0) ? -1.0f : 1.0f;
                    float ySign = (speedmvn.Y < 0) ? -1.0f : 1.0f;

                    float xAbs = speedmvn.X * xSign;
                    float yAbs = speedmvn.Y * ySign;

                    float xWeight = xAbs * xAbs / magSqr;
                    float yWeight = yAbs * yAbs / magSqr;

                    float xDecel = xWeight * PLAYER_DECELERATION * xSign * Raylib.GetFrameTime();
                    float yDecel = yWeight * PLAYER_DECELERATION * ySign * Raylib.GetFrameTime();

                    speedmvn.X = (xAbs > xDecel) ? speedmvn.X - xDecel : 0;
                    speedmvn.Y = (yAbs > yDecel) ? speedmvn.Y - yDecel : 0;
                }
            }

        }

        public void ChangeDirection(float frame)
        {

            if (Raylib.IsKeyDown(KeyboardKey.D))
            {
                this.Rotation += (rotationSpeed * frame);
            }
            if (Raylib.IsKeyDown(KeyboardKey.A))
            {
                this.Rotation -= (rotationSpeed * frame);
            }


        }

        public void PlayerMove(Sound somMovimento)
        {
            float frame = Raylib.GetFrameTime();
            this.Position = Raymath.Vector2Add(Position, Raymath.Vector2Scale(speedmvn, frame));
            UpdateSpeed(frame, somMovimento);
            ChangeDirection(frame);
        }

        public Vector2 GetFacingDirection()
        {
            float rad = SimpleMaths.GetRad(Rotation);
            Vector2 pos = new(0, -1);
            return Raymath.Vector2Rotate(pos, rad);
        }



        public Vector2 GetPlayerPosition()
        {
            return Position;
        }

        public float GetPlayerRot()
        {
            return Rotation;
        }

    }
}
