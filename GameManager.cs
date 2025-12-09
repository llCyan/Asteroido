using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Asteroido
{
    public class GameManager
    {
        Vector2 posInicial;

        public List<GameObjects> Objetos { get; set; } = new List<GameObjects>();
        public Player PlayableCharacter { get; set; }
        public Camera2D Camera;


        public GameManager()
        {
            posInicial = new Vector2(RaylibRun.ScreenWidth / 2, RaylibRun.ScreenHeight / 2);
            PlayableCharacter = new Player(posInicial, 0 );
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

        public void Meteorite()
        {

            Asteroids novoMeteorito = new Asteroids(Asteroids.GetRandomPosition(), Asteroids.GetRandomMetRot(), PlayableCharacter.Position);


            Objetos.Add(novoMeteorito);
        }

        public void UpdateGame()
        {
            Camera.Target = PlayableCharacter.Position;
            
            if (Raylib.IsKeyPressed(KeyboardKey.Space))
            {
                Atirar();
                Meteorite();
            }

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
