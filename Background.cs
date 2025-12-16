using System.Numerics;
using Raylib_cs;

namespace Asteroido
{
    public class Background
    {

        Vector3[] Stars;
        Vector2[] starPosition;
        const float startCount = 320;
        Vector2 Playerpos;


        public Background(Vector2 playerPos)
        {
            Playerpos = playerPos;
            Stars = new Vector3[(int)startCount];
            starPosition = new Vector2[(int)startCount];
        }

        public void Draw()
        {
            Raylib.ClearBackground(Color.Black);
        }

        public void InitializeStars()
        {
            
            for (int i = 0; i < startCount; i++)
            {
                RestartStars(i, Playerpos);
            }
        }
        public void Update(Vector2 newPlayerPos)
        {

            for (int i = 0; i < startCount; i++)
            {
                Vector2 Playerpos2 = newPlayerPos;
                float centerX = Playerpos2.X;
                float centerY = Playerpos2.Y;
                
                float ParalaxFactor = 1.0f / Stars[i].Z;

                float offsetX = Playerpos2.X * ParalaxFactor;
                float offsetY = Playerpos2.Y * ParalaxFactor;

                starPosition[i] = new Vector2(
                    centerX + Stars[i].X  - offsetX,
                    centerY + Stars[i].Y  - offsetY
                    );

                if (starPosition[i].X < 0 || starPosition[i].X > RaylibRun.ScreenWidth ||
                   starPosition[i].Y < 0 || starPosition[i].Y > RaylibRun.ScreenHeight)
                {
                    RestartStars(i, Playerpos2);
                }

                float radius = Math.Clamp(ParalaxFactor * 0.5f, 0.5f, 2.0f);

                Raylib.DrawCircle((int)starPosition[i].X, (int)starPosition[i].Y, radius, Color.White);


            }
            
        }

        public void RestartStars(int i, Vector2 pPos)
        {
            int ScreenX = (int)pPos.X;
            int ScreenY = (int)pPos.Y;
            int offCamera = 1000;

            Stars[i].X = (float)Raylib.GetRandomValue(ScreenX - offCamera, ScreenX + offCamera);
            Stars[i].Y = (float)Raylib.GetRandomValue(ScreenY - offCamera, ScreenY + offCamera);
            Stars[i].Z = (float)Raylib.GetRandomValue(50, 200) / 100.0f;
        }
    }
}
