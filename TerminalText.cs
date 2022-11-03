using System.Drawing;
using System.Numerics;
using System.Text.RegularExpressions;

namespace TerminalRenderer;

public class TerminalText : ITerminalRenderable
{
    public Point Position; 
    public string Text;
    public Alignment TextAlignment;
    public TerminalPixel PixelType;

    public TerminalText(Point pos, string text, TerminalPixel pixels, Alignment textAlignment = Alignment.Left)
    {
        Position = pos;
        Text = text;
        TextAlignment = textAlignment;
        PixelType = pixels;
    }

    public enum Alignment
    {
        Left, Center, Right
    }
    
    public IEnumerable<TerminalPixel> Render()
    {
        var chunks = Text.Chunk(2).Select(x => new string(x).PadRight(2, ' '));
        if (TextAlignment == Alignment.Right)
            chunks = chunks.Reverse();
        int length = chunks.Count();
        
        int i = 0;
        foreach (string chunk in chunks)
        {
            yield return PixelType with
            {
                Position = Position with
                {
                    X = TextAlignment switch
                    {
                        Alignment.Left => Position.X + i++,
                        Alignment.Center => Position.X + i++ - length / 2,
                        Alignment.Right => Position.X - i++,
                        _ => throw new ArgumentOutOfRangeException()
                    }
                }, PixelContent = chunk
            };
        }
    }
}