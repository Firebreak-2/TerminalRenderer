using System.Drawing;
using SixLabors.ImageSharp.PixelFormats;

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
                Position = new Point(0, -(y++ % TerminalDisplay.DefaultCameraPosition.Y * 2) - TerminalDisplay.DefaultCameraPosition.Y)
            });
            
            display.Render();
            Thread.Sleep(8);
        }
    }
}