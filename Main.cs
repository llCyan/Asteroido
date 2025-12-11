

using Asteroido;

namespace Asteroid;

internal static class Program
{

    [STAThread]
    public static void Main()
    {
        RaylibRun playgame = new RaylibRun();

        playgame.Execute();
    }
}