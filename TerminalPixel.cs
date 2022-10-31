using System.Diagnostics;
using System.Drawing;

namespace TerminalRenderer;

[DebuggerDisplay("({Position.X}, {Position.Y}) | {BackgroundColor.Name}")]
public struct TerminalPixel : ITerminalRenderable
{
    public Point Position = Point.Empty;
    public Color BackgroundColor = Color.Black;
    public Color ForegroundColor = Color.White;
    
    private string _pixelContent = "  ";
    public static int PixelContentLength = 2;

    public static TerminalPixel Default = new();

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

    public TerminalPixel(Point position)
    {
        Position = position;
    }

    public bool EquivalentTo(TerminalPixel b)
    {
        return BackgroundColor.ToArgb() == b.BackgroundColor.ToArgb()
               && ForegroundColor.ToArgb() == b.ForegroundColor.ToArgb()
               && _pixelContent == b._pixelContent;
    }

    public Point GetFramePosition(Point viewOffset) => new(Position.X - viewOffset.X, Position.Y - viewOffset.Y);
    
    public Point GetFramePosition(TerminalDisplay display) => GetFramePosition(display.ViewOffset);
    
    public IEnumerable<TerminalPixel> Render() => new[] {this};
}