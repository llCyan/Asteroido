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
        Rectangle sourceThruster;
        Rectangle destThruster;
        Vector2 originThruster;
        Rectangle sourcePlayer;
        Rectangle destPlayer;
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
        isPlayerMoving isPlayerSpeeding;
        float frameCounter;
        int currentFrame;
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
            
            sourcePlayer = new(lado1, lado2, SpaceShipTexture.Width, SpaceShipTexture.Height);
            destPlayer = new(Position.X, Position.Y, radius * 2, radius * 2);
            origin = new(destPlayer.Width / 2, destPlayer.Height / 2);
            Raylib.DrawTexturePro(SpaceShipTexture, sourcePlayer, destPlayer, origin, Rotation, color);

            if (isPlayerSpeeding == isPlayerMoving.Accelerating || isPlayerSpeeding == isPlayerMoving.MaxSpeed)
            {
                PlayerThrusterEffect(isPlayerSpeeding);
            }else
            {
                PlayerThrusterEffect(isPlayerMoving.Stopped);
            }
        }
        const int framesSpeed = 12;
        public override void Update()
        {
            frameCounter++;
            if (frameCounter >= (60 / framesSpeed))
            {
                frameCounter = 0;
                currentFrame++;
                if (currentFrame >= 3) currentFrame = 0;

            }

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
                
                isPlayerSpeeding = isPlayerMoving.Accelerating;
                if (Raymath.Vector2Length(Speedmvn) >= PLAYER_SPEED)
                {
                    isPlayerSpeeding = isPlayerMoving.MaxSpeed;
                }
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
                isPlayerSpeeding = isPlayerMoving.Stopped;
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

        const float distanceFromCenter = 34.0f;
            Texture2D AtualThruster;
        public void PlayerThrusterEffect(isPlayerMoving StatusNow)
        {
            float backwardOffset = Rotation-270.0f;
            float rad = SimpleMaths.GetRad(backwardOffset);
            (double sin, double cos) = Math.SinCos(rad);
            Vector2 offset;
            offset.X = Position.X + distanceFromCenter * (float)cos;
            offset.Y = Position.Y + distanceFromCenter * (float)sin;

            if( StatusNow == isPlayerMoving.Accelerating)
            {
                AtualThruster = ThrusterTextureMedium;
            }
            else if (StatusNow == isPlayerMoving.MaxSpeed)
            {
                AtualThruster = ThrusterTextureLarge;
            }
            else
            {
                AtualThruster = ThrusterTextureSmall;
            }

            float newlado1 = currentFrame * AtualThruster.Width / 3;
            sourceThruster = new Rectangle(newlado1, 0, AtualThruster.Width / 3, AtualThruster.Height);
            destThruster = new Rectangle(offset.X  , offset.Y , AtualThruster.Width / 3, AtualThruster.Height );
            originThruster = new Vector2(destThruster.Width / 2, destThruster.Height / 2);


            Raylib.DrawTexturePro(AtualThruster, sourceThruster, destThruster, originThruster, Rotation - 270, color);

            

        }

        public enum isPlayerMoving
        {
            Stopped,
            Accelerating,
            MaxSpeed

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
