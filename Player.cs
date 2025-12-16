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
        static Texture2D SpaceShipTexture;
        static Texture2D ThrusterTextureSmall;
        static Texture2D ThrusterTextureMedium;
        static Texture2D ThrusterTextureLarge;
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
            Speedmvn = new Vector2(0, 0);



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
        public static void GetResources()
        {
            SpaceShipTexture = Raylib.LoadTexture(@"resource\ship32.png");
            Sounds = Raylib.LoadSound(@"resource\low-spaceship-rumble-195722.mp3");
            Explosion = Raylib.LoadSound(@"resource\HitSound.mp3");
            ThrusterTextureLarge = Raylib.LoadTexture(@"resource\thruster_large.png");
            ThrusterTextureMedium = Raylib.LoadTexture(@"resource\thruster_medium.png");
            ThrusterTextureSmall = Raylib.LoadTexture(@"resource\thruster_small.png");
        }
        public static void UnloadResources()
        {
            Raylib.UnloadTexture(SpaceShipTexture);
            Raylib.UnloadSound(Sounds);
        }
        public override void Draw()
        {

            Rectangle source = new(lado1, lado2, SpaceShipTexture.Width, SpaceShipTexture.Height);
            Rectangle dest = new(Position.X, Position.Y, radius * 2, radius * 2);
            origin = new(dest.Width / 2, dest.Height / 2);
            Raylib.DrawTexturePro(SpaceShipTexture, source, dest, origin, Rotation, color);
            PlayerThrusterEffect();

        }

        public override void Update()
        {


            BlinkEffect();
            PlayerMove(Sounds);
            HitBox = new Rectangle(Position.X, Position.Y, SpaceShipTexture.Width, SpaceShipTexture.Height);
        }

        public void UpdateSpeed(float frame, Sound SomMovimento)
        {
            float rad = SimpleMaths.GetRad(Rotation);
            float magSqr = Raymath.Vector2LengthSqr(Speedmvn);
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

                Speedmvn = Raymath.Vector2Add(Speedmvn, Raymath.Vector2Scale(FacingDirection, (PLAYER_ACCELERATION * frame)));

                if (mag > PLAYER_SPEED)
                {
                    Speedmvn = Raymath.Vector2Scale(Speedmvn, PLAYER_SPEED / mag);
                }
            }
            else
            {
                if (mag > 0)
                {
                    Vector2 direction = Raymath.Vector2Normalize(Speedmvn);
                    float decelMagnitude = PLAYER_DECELERATION * frame;
                    Vector2 decel = Raymath.Vector2Scale(direction, -decelMagnitude);
                    if (Raymath.Vector2Length(decel) > mag)
                    {
                        Speedmvn = new Vector2(0, 0);
                    }
                    else
                    {
                        Speedmvn = Raymath.Vector2Add(Speedmvn, decel);
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
            this.Position = Raymath.Vector2Add(Position, Raymath.Vector2Scale(Speedmvn, frame));
            UpdateSpeed(frame, somMovimento);
            ChangeDirection(frame);
        }

        public void PlayerThrusterEffect()
        {
            float rad = SimpleMaths.GetRad(Rotation);
            float posSquared = Raymath.Vector2LengthSqr(Position);
            float length = (float)Math.Sqrt(posSquared);
            (double sin, double cos) = Math.SinCos(length);
            Vector2 offset = new Vector2((float)sin * 5, (float)cos * 5);

            float newlado1 = ThrusterTextureSmall.Width / 3;
            Rectangle source = new Rectangle(newlado1, 0, ThrusterTextureSmall.Width / 3, ThrusterTextureSmall.Height);
            Rectangle dest = new Rectangle(offset.X, offset.Y, ThrusterTextureSmall.Width / 3, ThrusterTextureSmall.Height );
            Vector2 origin = new Vector2(dest.Width / 2, dest.Height / 2);

            Raylib.DrawTexturePro(ThrusterTextureSmall, source, dest, origin, -Rotation, color);


            float angle = Rotation + 180;
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
