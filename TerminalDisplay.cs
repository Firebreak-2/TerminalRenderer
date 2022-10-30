using System.Drawing;
using Pastel;

namespace TerminalRenderer;

public class TerminalDisplay
{
    private List<ITerminalRenderable> _renderList = new();
    private Dictionary<Point, TerminalPixel> _pixelDrawList = new();
    public Point ViewOffset = DefaultViewOffset;
    public static Point DefaultViewOffset = new((-Console.WindowWidth / 2 - 1) / 2, (-Console.WindowHeight - 1) / 2);

    public T Add<T>(T terminalRenderable) where T : ITerminalRenderable
    {
        _renderList.Add(terminalRenderable);
        return terminalRenderable;
    }

    public T Draw<T>(T terminalRenderable) where T : ITerminalRenderable
    {
        foreach (TerminalPixel pixel in terminalRenderable.Render())
        {
            _pixelDrawList[pixel.Position] = pixel;
        }
        return terminalRenderable;
    }

    /// <summary>
    /// Draws over the terminal and renders the draw list
    /// </summary>
    public void Render()
    {
        Console.SetCursorPosition(0, 0);

        int screenWidth = Console.WindowWidth / 2 - 1;
        int screenHeight = Console.WindowHeight - 1;

        Dictionary<Point, TerminalPixel> pointsToRender = new();

        foreach (TerminalPixel pixel in _renderList.SelectMany(item => item.Render()))
        {
            pointsToRender[pixel.Position] = pixel;
        }

        foreach (var (pos, pixel) in _pixelDrawList)
        {
            pointsToRender[pos] = pixel;
        }
        _pixelDrawList.Clear();

        for (int y = 0; y < screenHeight; y++)
        {
            for (int x = 0; x < screenWidth; x++)
            {
                // gets the current point with the camera offset
                var currentPoint = new Point(x + ViewOffset.X, y + ViewOffset.Y);

                // checks if any point draws on the current pixel and uses it
                // if none are found, uses default
                if (pointsToRender.TryGetValue(currentPoint, out var pixel)) ;
                else pixel = new TerminalPixel();
                
                string colorizedContent = pixel.PixelContent
                    .PastelBg(pixel.BackgroundColor)
                    .Pastel(pixel.ForegroundColor);

                Console.Write(colorizedContent);
            }

            Console.WriteLine();
        }
        
        Console.ResetColor();
    }

    public void MoveCamera(Point offset)
    {
        ViewOffset.X += offset.X;
        ViewOffset.Y += offset.Y;
    }

    public void MoveCamera(int xOffset, int yOffset) => MoveCamera(new Point(xOffset, yOffset));
}