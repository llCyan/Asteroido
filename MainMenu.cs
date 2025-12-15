using Raylib_cs;
using System.Numerics;

namespace Asteroido
{
    public class MainMenu
    {
        static Texture2D StartButton;
        GameManager.GameState running;

        public GameManager.GameState Show()
        {
            Rectangle Source = new Rectangle(0, 0, StartButton.Width / 4, StartButton.Height / 4);
            Vector2 Position = Raylib.GetScreenCenter();



            Raylib.ClearBackground(Color.Black);
            Raylib.DrawText("ASTEROID X", (RaylibRun.ScreenWidth / 2) - 125, (RaylibRun.ScreenHeight / 2) - 100, 50, Color.White);
            Raylib.DrawText("Press ENTER to Start", 325, 300, 20, Color.LightGray);
            Raylib.DrawTextureRec(StartButton, Source, Position, Color.White);
            Raylib.DrawText("Press ESC to Exit", 340 + 10, 350, 20, Color.LightGray);


            if (Raylib.IsKeyPressed(KeyboardKey.Enter))
            {
                running = GameManager.GameState.Playing;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            {
                running = GameManager.GameState.MainMenu;
            }

            return running;

        }

        public void GameOver()
        {
            Raylib.ClearBackground(Color.Black);
            Raylib.DrawText("GAME OVER", (RaylibRun.ScreenWidth / 2) - 125, (RaylibRun.ScreenHeight / 2) - 100, 50, Color.Red);
            Raylib.DrawText("Press ENTER to Restart", 300, 300, 20, Color.LightGray);
            Raylib.DrawText("Press ESC to Exit", 340 + 10, 350, 20, Color.LightGray);
            if (Raylib.IsKeyPressed(KeyboardKey.Enter))
            {
                running = GameManager.GameState.Playing;
            }
            if (Raylib.IsKeyPressed(KeyboardKey.Escape))
            {
                running = GameManager.GameState.MainMenu;
            }
        }

        public void LoadResources()
        {
            StartButton = Raylib.LoadTexture(@"resource\start.png");
        }
        public void UnloadResources()
        {
            Raylib.UnloadTexture(StartButton);
        }

    }
}
