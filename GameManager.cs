using Raylib_cs;
using System.Numerics;

namespace Asteroido
{


    public class GameManager
    {
        Vector2 posInicial;

        public List<GameObjects> Objetos { get; set; } = new List<GameObjects>();
        public Player PlayableCharacter { get; set; }
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
        int numberpos;

        public GameManager()
        {
            posInicial = new Vector2(RaylibRun.ScreenWidth / 2, RaylibRun.ScreenHeight / 2);
            PlayableCharacter = new Player(posInicial, 0);
            Camera = new Camera2D();


        }

        public void Inicializar()
        {

            Objetos.Add(PlayableCharacter);
            CameraStuff();
        }

        public void LoadsResources()
        {
            Player.GetResources();
            Shot.GetResources();
            Asteroids.GetResources();

        }
        public void UnloadsResources()
        {
            Player.UnloadResources();
            Shot.UnloadResources();
            Asteroids.UnloadResources();
            Raylib.EndMode2D();
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

        public void Meteorite()
        {


            Asteroids novoMeteorito = new Asteroids(Asteroids.GetRandomPosition(), Asteroids.GetRandomMetRot(), PlayableCharacter.Position);


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
                    Meteorite();
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
            bool tiroMeteorColision = false;


            for (int i = Objetos.Count - 1; i >= 0; i--)
            {
                GameObjects tiro = Objetos[i];
                if (tiro is not Shot) continue;
                for (int j = Objetos.Count - 1; j >= 0; j--)
                {
                    GameObjects Meteor = Objetos[j];
                    if (i == j || Meteor is not Asteroids) continue;
                    bool colision = Raylib.CheckCollisionRecs(tiro.hitBox, Meteor.hitBox);
                    if (colision)
                    {
                        Objetos.RemoveAt(j);
                        tiroMeteorColision = true;
                        numberpos = i;
                        break;
                    }

                }
                break;
            }

            if (tiroMeteorColision)
            {
                Objetos.RemoveAt(numberpos);
                tiroMeteorColision = false;

            }
        }

        public void UpdateGame()
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



            foreach (GameObjects obj in Objetos)
            {
                obj.Update();
            }
        }

        public void DrawGame()
        {

            Raylib.DrawText("Asteroido X:" + PlayableCharacter.Position.X + " Y :" + PlayableCharacter.Position.Y, 10, 10, 20, Color.White);
            Raylib.BeginMode2D(Camera);
            foreach (GameObjects obj in Objetos)
            {
                obj.Draw();
            }
        }


        public void CameraStuff()
        {
            Camera.Rotation = 0;
            Camera.Zoom = 1.5f;
            Camera.Offset = new Vector2(RaylibRun.ScreenWidth / 2, RaylibRun.ScreenHeight / 2);
            Camera.Target = PlayableCharacter.Position;
        }
    }
}
