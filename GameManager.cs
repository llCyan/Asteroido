using Raylib_cs;
using System.Numerics;

namespace Asteroido
{


    public class GameManager
    {
        Vector2 posInicial;

        public List<GameObjects> Objetos { get; set; } = new List<GameObjects>();
        public Player PlayableCharacter { get; set; }
        public MainMenu MainMenuScreen;
        public Background background;
        DrawScore Score;
        public Camera2D Camera;
        const int MaxAsteroid = 12;
        const float TimeUntilNextSpawn = 2.0f;
        float SpawnTimer = 0.0f;
        float cullingRadius;
        float cullingRadiusSqred;
        float distance;
        const float shotCooldown = 0.70f;
        bool canShoot = true;
        float lastShotTime = 0.0f;
        int meteorSize = -1;
        bool MeteorState = false;
        GameState gameState;

        public GameManager()
        {
            posInicial = new Vector2(RaylibRun.ScreenWidth / 2, RaylibRun.ScreenHeight / 2);
            PlayableCharacter = new Player(posInicial, 0);
            Camera = new Camera2D();
            MainMenuScreen = new MainMenu();
            Score = new DrawScore();
            
        }

        public void Inicializar()
        {

            ResetGame();
            CameraStuff();
            background = new Background(PlayableCharacter.Position);
            background.InitializeStars();
        }

        public void ResetGame()
        {
            Objetos.Clear();
            PlayableCharacter = new Player(posInicial, 0);
            Objetos.Add(PlayableCharacter);
            CameraStuff();
        }

        public void LoadsResources()
        {
            Player.GetResources();
            Shot.GetResources();
            Asteroids.GetResources();
            MainMenuScreen.LoadResources();
        }
        public void UnloadsResources()
        {
            Player.UnloadResources();
            Shot.UnloadResources();
            Asteroids.UnloadResources();
            Raylib.EndMode2D();
            MainMenuScreen.UnloadResources();
        }


        public void Atirar()
        {
            Vector2 playerPos = PlayableCharacter.Position;
            float playerRot = PlayableCharacter.Rotation;

            Shot novoTiro = new Shot(PlayableCharacter.GetPlayerPosition(), PlayableCharacter.Rotation, PlayableCharacter.GetFacingDirection());


            Objetos.Add(novoTiro);
        }
        public void ControlarTiros()
        {

            if (Raylib.IsKeyPressed(KeyboardKey.Space) && canShoot)
            {
                Atirar();
                canShoot = false;
                lastShotTime += Raylib.GetFrameTime();


            }


            for (int i = Objetos.Count - 1; i >= 0; i--)
            {
                GameObjects obj = Objetos[i];
                if (obj is Shot)
                {
                    distance = Vector2.Distance(PlayableCharacter.Position, obj.Position);
                    if (distance >= 300)
                    {

                        Objetos.RemoveAt(i);

                    }
                }
            }


        }

        public void Meteorite(int meteorSize)
        {

            Vector2 spawnPosition = Asteroids.GetRandomPosition();

            Asteroids novoMeteorito = new Asteroids(PlayableCharacter.Position, Asteroids.MeteorSize.Random, MeteorState, spawnPosition);


            Objetos.Add(novoMeteorito);

        }


        public void MeteoriteControl()
        {
            SpawnTimer += Raylib.GetFrameTime();
            cullingRadius = PlayableCharacter.Position.X + 500.0f;
            cullingRadiusSqred = cullingRadius * cullingRadius;

            for (int i = Objetos.Count - 1; i >= 0; i--)
            {
                GameObjects obj = Objetos[i];

                if (obj is Asteroids)
                {
                    float distanceSqred = Vector2.DistanceSquared(PlayableCharacter.Position, obj.Position);
                    if (distanceSqred > cullingRadiusSqred)
                    {
                        Objetos.RemoveAt(i);
                    }
                }
            }

            if (SpawnTimer > TimeUntilNextSpawn)
            {
                if (Objetos.OfType<Asteroids>().Count() < MaxAsteroid)
                {
                    Meteorite(meteorSize);
                    SpawnTimer = 0.0f;
                }
                else
                {
                    SpawnTimer = SpawnTimer * 0.5f;
                }
            }


        }

        public void ColisionCheck()
        {

            var toRemove = new HashSet<GameObjects>();

            for (int i = Objetos.Count - 1; i >= 0; i--)
            {
                GameObjects tiro = Objetos[i];
                if (tiro is not Shot) continue;
                for (int j = Objetos.Count - 1; j >= 0; j--)
                {
                    GameObjects Meteor = Objetos[j];
                    if (i == j || Meteor is not Asteroids) continue;
                    bool colision = Raylib.CheckCollisionRecs(tiro.HitBox, Meteor.HitBox);
                    if (colision)
                    {

                        toRemove.Add(tiro);
                        toRemove.Add(Meteor);


                        if (Meteor.NewMeteorSize == Asteroids.MeteorSize.Large)
                        {
                            MeteorState = true;
                            for (int k = 0; k < 2; k++)
                            {

                                Asteroids novoMeteorito = new Asteroids(PlayableCharacter.Position, Asteroids.MeteorSize.Medium, MeteorState, Meteor.Position);
                                Objetos.Add(novoMeteorito);
                            }
                            MeteorState = false;
                            Score.MathScore(DrawScore.ScoreLevels.High);

                        }
                        else if (Meteor.NewMeteorSize == Asteroids.MeteorSize.Medium)
                        {

                            MeteorState = true;
                            for (int k = 0; k < 2; k++)
                            {

                                Asteroids novoMeteorito = new Asteroids(PlayableCharacter.Position, Asteroids.MeteorSize.Small, MeteorState, Meteor.Position);
                                Objetos.Add(novoMeteorito);
                            }
                            MeteorState = false;
                            Score.MathScore(DrawScore.ScoreLevels.Medium);
                        }
                        else if (Meteor.NewMeteorSize == Asteroids.MeteorSize.Small)
                        {
                            Score.MathScore(DrawScore.ScoreLevels.Low);
                        }


                        break;
                    }

                }


            }
            Objetos.RemoveAll(o => toRemove.Contains(o));

        }

        public void PlayerColision()
        {
            var toRemove = new HashSet<GameObjects>();
            for (int i = Objetos.Count - 1; i >= 0; i--)
            {
                GameObjects obj = Objetos[i];
                if (obj is not Asteroids) continue;
                else
                {
                    IsMeteorPlayerColiding(PlayableCharacter, obj.HitBox);
                    bool colision = Raylib.CheckCollisionRecs(PlayableCharacter.HitBox, obj.HitBox);
                    if (colision)
                    {
                        PlayableCharacter.PlayerTakeDamage();


                        if (PlayableCharacter.playerHitPoint == 0)
                        {
                            gameState = GameState.GameOver;
                            toRemove.Add(PlayableCharacter);
                            PlayableCharacter.PlayExplosionSound();
                        }
                    }
                }
            }

            Objetos.RemoveAll(o => toRemove.Contains(o));
        }

        public void IsMeteorPlayerColiding(Player Player, Rectangle meteor)
        {
            if (Player.Position.X < meteor.Position.X + meteor.Width &&
               Player.Position.X + Player.HitBox.Width > meteor.Position.X &&
               Player.Position.Y < meteor.Position.Y + meteor.Height &&
               Player.Position.Y + Player.HitBox.Height > meteor.Position.Y)
            {
                Player.Speedmvn *= -1;
            }
        }
        public void UpdateGame()
        {
            background.Update(PlayableCharacter.Position);
            if (gameState == GameState.MainMenu)
                gameState = MainMenuScreen.Show();

            if (gameState == GameState.GameOver)
            {
                MainMenuScreen.GameOver();
                if (Raylib.IsKeyPressed(KeyboardKey.Enter))
                {
                    ResetGame();
                    gameState = GameState.Playing;
                }
            }


            if (gameState == GameState.Playing)
            {
                Camera.Target = PlayableCharacter.Position;



                lastShotTime += Raylib.GetFrameTime();
                if (!canShoot)
                {

                    if (lastShotTime >= shotCooldown)
                    {
                        canShoot = true;
                        lastShotTime = 0.0f;
                    }
                }


                MeteoriteControl();
                ControlarTiros();
                ColisionCheck();
                PlayerColision();



                foreach (GameObjects obj in Objetos)
                {
                    obj.Update();
                }
            }
        }

        public void DrawGame()
        {


            
            background.Draw();

            if (gameState == GameState.Playing)
            {
                Score.Draw();
                Raylib.BeginMode2D(Camera);
                foreach (GameObjects obj in Objetos)
                {
                    obj.Draw();
                }
            }
        }


        public void CameraStuff()
        {
            Camera.Rotation = 0;
            Camera.Zoom = 1.5f;
            Camera.Offset = new Vector2(RaylibRun.ScreenWidth / 2, RaylibRun.ScreenHeight / 2);
            Camera.Target = PlayableCharacter.Position;
        }

        public enum GameState
        {
            MainMenu,
            Playing,
            GameOver
        }
    }
}
