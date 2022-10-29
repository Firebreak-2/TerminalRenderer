using System.Drawing;

namespace TerminalRenderer;

public struct TerminalPixel : ITerminalRenderable
{
    public Point Position = Point.Empty;
    public Color BackgroundColor = Color.Black;
    public Color ForegroundColor = Color.Transparent;
    
    private string _pixelContent = "  ";
    public static int PixelContentLength = 2;

    public string PixelContent
    {
        get => _pixelContent;
        init
        {
            if (value.Length != PixelContentLength)
                throw new Exception($"Pixel value has to be {PixelContentLength} characters");

            _pixelContent = value;
        }
    }

    public TerminalPixel()
    {
        PixelContent = "  ";
        BackgroundColor = Color.Black;
        ForegroundColor = Color.White;
        Position = Point.Empty;
    }
    
    public TerminalPixel(Point position, Color color)
    {
        Position = position;
        BackgroundColor = color;
    }

    public TerminalPixel(Color color)
    {
        BackgroundColor = color;
    }
    
    public IEnumerable<TerminalPixel> Render() => new[] {this};
}