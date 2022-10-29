using System.Drawing;

namespace TerminalRenderer;

internal static class Program
{
    public static void Main(string[] args)
    {
        TerminalDisplay display = new();

        int y = 0;
        while (true)
        {
            display.Draw(new TerminalPixel(Color.Aqua)
            {
                Position = new Point(0, -(y++ % TerminalDisplay.DefaultViewOffset.Y * 2) - TerminalDisplay.DefaultViewOffset.Y)
            });
            
            display.Render();
            Thread.Sleep(8);
        }
    }
}