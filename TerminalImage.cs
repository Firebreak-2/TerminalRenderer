using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace TerminalRenderer;

public class TerminalImage : ITerminalRenderable
{
    public Point TopLeft = Point.Empty;
    public Image<Argb32> ImageData;
    public TerminalImage(Point topLeft, Image<Argb32> image)
    {
        TopLeft = topLeft;
        ImageData = image;
    }

    public static TerminalImage Centered(Point center, int width, int height, Image<Argb32> image)
    {
        var tl = new Point(center.X - width / 2, center.Y - height / 2);
        return new TerminalImage(tl, image);
    }

    public IEnumerable<TerminalPixel> Render()
    {
        for (int y = 0; y < ImageData.Height; y++)
        {
            for (int x = 0; x < ImageData.Width; x++)
            {
                var pixel = ImageData[x, y];

                Color col = Color.FromArgb(pixel.A, pixel.R, pixel.G, pixel.B);
                
                yield return new TerminalPixel(new Point(TopLeft.X + x, TopLeft.Y + y), col);
            }
        }
    }
}