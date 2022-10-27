using System.Drawing;

namespace TerminalRenderer;

internal static class Program
{
    public static void Main(string[] args)
    {
        TerminalDisplay display = new();

        display.Draw(TerminalRectangle.Centered(default, 3, 3, new TerminalPixel(Color.Aqua)));
        display.Render();
        
        Console.ReadKey(true);
    }
}