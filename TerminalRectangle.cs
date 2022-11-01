using System.Drawing;

namespace TerminalRenderer;

public class TerminalRectangle : ITerminalRenderable
{
    public Point TopLeft = Point.Empty;
    public Point BottomRight = Point.Empty;
    public TerminalPixel PixelType = new();

    public int Width => Math.Abs(BottomRight.X - TopLeft.X);
    public int Height => Math.Abs(BottomRight.Y - TopLeft.Y);

    public int X
    {
        get => TopLeft.X;
        set
        {
            int w = Width;
            TopLeft.X = value;
            BottomRight.X = value + w;
        }
    }

    public int Y
    {
        get => TopLeft.Y;
        set
        {
            int h = Height;
            TopLeft.Y = value;
            BottomRight.Y = value + h;
        }
    }

    public TerminalRectangle(Rectangle rectangle, TerminalPixel pixels)
    {
        TopLeft = new Point(rectangle.Left, rectangle.Top);
        BottomRight = new Point(rectangle.Right, rectangle.Bottom);
        PixelType = pixels;
    }
    
    public TerminalRectangle(Point topLeft, Point bottomRight, TerminalPixel pixels)
    {
        TopLeft = topLeft;
        BottomRight = bottomRight;
        PixelType = pixels;
    }

    public TerminalRectangle(Point topLeft, int width, int height, TerminalPixel pixels)
        : this(topLeft, new Point(topLeft.X + width, topLeft.Y + height), pixels)
    { }

    public static TerminalRectangle Centered(Point center, int width, int height, TerminalPixel pixels)
    {
        var tl = new Point(center.X - width / 2, center.Y - height / 2);
        return new TerminalRectangle(tl, width, height, pixels);
    }

    public IEnumerable<TerminalPixel> Render()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                yield return PixelType with
                {
                    Position = new Point(TopLeft.X + x, TopLeft.Y + y)
                };
            }
        }
    }
}