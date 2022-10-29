using System.Drawing;
using System.Numerics;

namespace TerminalRenderer;

public class TerminalLine : ITerminalRenderable
{
    public Point A = default;
    public Point B = default;
    public TerminalPixel PixelType = default;
    public float Length => Vector2.Distance(new Vector2(A.X, A.Y), new Vector2(B.X, B.Y));
    public int LengthInt => (int) Math.Round(Length);

    public TerminalLine(Point a, Point b, TerminalPixel pixels)
    {
        A = a;
        B = b;
        PixelType = pixels;
    }

    public IEnumerable<TerminalPixel> Render()
    {
        int x = A.X;
        int y = A.Y;
        int x2 = B.X;
        int y2 = B.Y;
        
        int w = x2 - x;
        int h = y2 - y;
        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
        
        if (w < 0) dx1 = -1;
        else if (w > 0) dx1 = 1;
        if (h < 0) dy1 = -1;
        else if (h > 0) dy1 = 1;
        if (w < 0) dx2 = -1;
        else if (w > 0) dx2 = 1;
        
        int longest = Math.Abs(w);
        int shortest = Math.Abs(h);
        
        if (!(longest > shortest))
        {
            longest = Math.Abs(h);
            shortest = Math.Abs(w);
            if (h < 0) dy2 = -1;
            else if (h > 0) dy2 = 1;
            dx2 = 0;
        }

        int numerator = longest >> 1;
        for (int i = 0; i <= longest; i++)
        {
            yield return PixelType with { Position = new Point(x, y) };
            numerator += shortest;
            if (!(numerator < longest))
            {
                numerator -= longest;
                x += dx1;
                y += dy1;
            }
            else
            {
                x += dx2;
                y += dy2;
            }
        }
    }
}