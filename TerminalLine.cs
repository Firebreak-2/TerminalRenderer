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
        for (int i = 0; i <= LengthInt; i++)
        {
            yield return PixelType with
            {
                Position = new Point(
                    x: (int) Math.Round((B.X + A.X) / Length * i),
                    y: (int) Math.Round((B.Y + A.Y) / Length * i)
                )
            };
        }
    }
}