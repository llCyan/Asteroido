

using Asteroido;

namespace Asteroid;

using Raylib_cs;


internal static class Program
{

    [STAThread]
    public static void Main()
    {
        RaylibRun playgame = new RaylibRun();

        playgame.Execute();
    }
}