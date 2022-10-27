using System.Drawing;
using System.Numerics;

namespace TerminalRenderer;

public class TerminalCircle : ITerminalRenderable
{
    public Point Origin = default;
    public int Radius = 0;
    public TerminalPixel PixelType = default;

    public TerminalCircle(Point origin, int radius, TerminalPixel pixels)
    {
        Origin = origin;
        Radius = radius;
        PixelType = pixels;
    }

    public bool Contains(Point point)
    {
        return Vector2.Distance(new Vector2(Origin.X, Origin.Y), new Vector2(point.X, point.Y)) <= Radius;
    }

    public IEnumerable<TerminalPixel> Render()
    {
        int lMost = Origin.X - Radius;
        int rMost = Origin.X + Radius;
        int tMost = Origin.Y - Radius;
        int bMost = Origin.Y + Radius;
        
        for (int y = tMost; y < bMost - tMost; y++)
        {
            for (int x = lMost; x < rMost - lMost; x++)
            {
                var p = new Point(lMost + x, tMost + y);
                if (Contains(p))
                    yield return PixelType with { Position = p };
            }
        }
    }
}