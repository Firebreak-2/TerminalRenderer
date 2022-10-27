using System.Drawing;

namespace TerminalRenderer;

public struct TerminalPixel : ITerminalRenderable
{
    public static readonly Color DefaultColor = Color.White;
    
    public Point Position = Point.Empty;
    public Color Color = DefaultColor;
    private byte _pixelContentId = 0;

    private static Dictionary<byte, string> s_pixelContentMapping = new();
    private static Dictionary<string, byte> s_inversePixelContentMapping = new();

    public string PixelContent
    {
        get
        {
            try
            {
                return s_pixelContentMapping[_pixelContentId];
            }
            catch (KeyNotFoundException e)
            {
                throw new Exception("Cannot fetch unregistered pixel content", e);
            }
        }
        init
        {
            if (value.Length != 2)
                throw new Exception("Pixel value has to be two characters");

            if (s_inversePixelContentMapping.TryGetValue(value, out var id))
                _pixelContentId = id;
            else
            {
                _pixelContentId = (byte) s_pixelContentMapping.Count;
                s_pixelContentMapping.Add(_pixelContentId, value);
                s_inversePixelContentMapping.Add(value, _pixelContentId);
            }
        }
    }

    public TerminalPixel(Color color, string content = "  ") : this(default, content, color) { }
    public TerminalPixel(Point position, string content, Color color)
    {
        Position = position;
        Color = color;
        PixelContent = content;
    }
    
    public TerminalPixel(Point position) : this(position, "  ", DefaultColor) { }
    
    public TerminalPixel(Point position, Color color) : this(position, "  ", color) { }

    public TerminalPixel() : this(Point.Empty) { }

    public IEnumerable<TerminalPixel> Render() => new[] {this};
}