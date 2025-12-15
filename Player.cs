using Raylib_cs;
using System.Numerics;

namespace Asteroido
{
    public class Player : GameObjects
    {


        int rotationSpeed = 360;
        const float radius = 32;
        const int PLAYER_SPEED = 250;
        const int PLAYER_ACCELERATION = 750;
        const int PLAYER_DECELERATION = 175;
        static Texture2D Texture;
        public static Sound Sounds;
        public static Sound Explosion;
        Vector2 origin;
        Color color = Color.White;
        float lado1 = 0.0f, lado2 = 0.0f;
        public int playerHitPoint = 2;
        public float Iframes = 3.0f;
        public bool isInvincible = false;
        float blinkTimer = 0.0f;
        float invulnerabilityTimer = 0.0f;
        public bool PlayerDmgTaken = false;


        public Player(Vector2 Pos, float rot) : base(Pos, rot)
        {
            speedmvn = new Vector2(0, 0);



        }

        public void PlayerTakeDamage()
        {

            if (!isInvincible)
            {
                playerHitPoint--;

                isInvincible = true;

            }




        }

        public void BlinkEffect()
        {
            if (isInvincible)
            {
                blinkTimer += Raylib.GetFrameTime();
                invulnerabilityTimer -= Raylib.GetFrameTime();

                if (blinkTimer <= Iframes)
                    color = Color.Red;

                if (blinkTimer >= Iframes)
                {
                    color = Color.White;
                    isInvincible = false;
                }

            }
        }

        public void PlayExplosionSound()
        {
            Raylib.PlaySound(Explosion);
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
            Explosion = Raylib.LoadSound(@"resource\HitSound.mp3");
        }
        public static void UnloadResources()
        {
            Raylib.UnloadTexture(Texture);
            Raylib.UnloadSound(Sounds);
        }

        public override void Update()
        {


            BlinkEffect();
            PlayerMove(Sounds);
            hitBox = new Rectangle(Position.X, Position.Y, Texture.Width, Texture.Height);
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
                    Vector2 direction = Raymath.Vector2Normalize(speedmvn);
                    float decelMagnitude = PLAYER_DECELERATION * frame;
                    Vector2 decel = Raymath.Vector2Scale(direction, -decelMagnitude);
                    if (Raymath.Vector2Length(decel) > mag)
                    {
                        speedmvn = new Vector2(0, 0);
                    }
                    else
                    {
                        speedmvn = Raymath.Vector2Add(speedmvn, decel);
                    }

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
