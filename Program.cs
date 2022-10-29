using System.Drawing;

namespace TerminalRenderer;

internal static class Program
{
    public static void Main(string[] args)
    {
        TerminalDisplay display = new();

        display.Draw(new TerminalLine(default, new Point(14, 4), new TerminalPixel(Color.Aqua)));
        display.Render();
        
        Console.ReadKey(true);
    }
}