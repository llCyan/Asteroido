using Raylib_cs;

namespace Asteroido
{
    internal class RaylibRun
    {
        public const int ScreenWidth = 832;
        public const int ScreenHeight = 624;
        public static Texture2D background;

        public void Execute()
        {


            Raylib.InitWindow(ScreenWidth, ScreenHeight, "Asteroido X");
            Raylib.InitAudioDevice();
            Raylib.SetTargetFPS(60);
            GameManager game = new GameManager();
            game.Inicializar();
            background = Raylib.LoadTexture(@"resource\background.png");
            game.LoadsResources();


            while (!Raylib.WindowShouldClose())
            {
                if (Raylib.IsKeyPressed(KeyboardKey.F11)) Raylib.ToggleFullscreen();
                Raylib.BeginDrawing();
                Raylib.DrawTexture(background, 0, 0, Color.RayWhite);
                Raylib.ClearBackground(Color.Black);
                Raylib.DrawFPS(10, 10);
                game.UpdateGame();

                game.DrawGame();

                Raylib.EndMode2D();
                Raylib.EndDrawing();


            }

            game.UnloadsResources();
            Raylib.CloseAudioDevice();
            Raylib.CloseWindow();
        }

    }
}
