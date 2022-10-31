using System.Diagnostics;
using System.Drawing;
using Pastel;

namespace TerminalRenderer;

public class TerminalDisplay
{
    private Dictionary<Point, TerminalPixel> _pixelDrawList = new();
    public Point ViewOffset = DefaultViewOffset;
    public int Width = DefaultWidth;
    public int Height = DefaultHeight;

    public static int DefaultWidth = Console.WindowWidth / 2 - 1;
    public static int DefaultHeight = Console.WindowHeight - 1;
    public static Point DefaultViewOffset = new(-(DefaultWidth / 2), -(DefaultHeight / 2));

    private int _writeRowIndex = 0;
    private int _writeColumnIndex = 0;

    private TerminalPixel[][] _previousFrame;
    private bool _firstFrameRendered = false;

    public TerminalDisplay()
    {
        _previousFrame = new TerminalPixel[Height][];

        for (int y = 0; y < _previousFrame.Length; y++)
        {
            _previousFrame[y] = new TerminalPixel[Width];
        }
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

        if (!_firstFrameRendered)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    // gets the current point with the camera offset
                    var currentPoint = FramePositionToWorldPosition(new Point(x, y));

                    // checks if any point draws on the current pixel and uses it
                    // if none are found, uses default
                    if (_pixelDrawList.TryGetValue(currentPoint, out var pixel)) ;
                    else pixel = new TerminalPixel(currentPoint);

                    Write(pixel, _writeColumnIndex++, _writeRowIndex);
                }

                DescendRow();
            }
        }
        else
        {
            IEnumerable<TerminalPixel> pointDifference = Diff(_previousFrame, _pixelDrawList);

            foreach (var pixel in pointDifference)
            {
                Point pos = pixel.GetFramePosition(this);
                Write(pixel, pos.X, pos.Y, true);
            }
        }

        EndRender();
    }

    private static IEnumerable<TerminalPixel> Diff(TerminalPixel[][] previous, Dictionary<Point, TerminalPixel> latest)
    {
        for (int y = 0; y < previous.Length; y++)
        {
            TerminalPixel[] row = previous[y];
            for (int x = 0; x < row.Length; x++)
            {
                TerminalPixel prevPixel = row[x];
                TerminalPixel? newPixel = null;
                if (latest.TryGetValue(prevPixel.Position, out TerminalPixel latestPixel))
                    newPixel = latestPixel;

                // __ => EXPLICIT EMPTY
                // .. => NULL / DOESNT EXIST
                // diff logic:
                // - | P    N    D    R 
                // 1 | __ + .. = .. > __
                // 2 | XX + .. = __ > __
                // 3 | XX + YY = YY > YY
                // 4 | XX + XX = .. > XX
                
                if (newPixel is null) // 1 or 2
                {
                    if (!prevPixel.EquivalentTo(TerminalPixel.Default)) // 2
                        yield return TerminalPixel.Default with { Position = prevPixel.Position };
                    
                    continue; // 1
                }
                else if (!prevPixel.EquivalentTo(newPixel.Value)) // 3
                    yield return newPixel.Value;
                
                continue; // 4
            }
        }
    }

    private void Write(TerminalPixel pixel, int x, int y, bool modifyPos = false)
    {
        if (modifyPos)
            Console.SetCursorPosition(x * TerminalPixel.PixelContentLength, y);

        string colorizedContent = pixel.PixelContent
            .PastelBg(pixel.BackgroundColor)
            .Pastel(pixel.ForegroundColor);

        Console.Write(colorizedContent);

        _previousFrame[y][x] = pixel;
    }

    private void DescendRow()
    {
        Console.WriteLine();

        _writeRowIndex++;
        _writeColumnIndex = 0;
    }

    private void EndRender()
    {
        Console.ResetColor();
        Console.SetCursorPosition(0, Height);

        _writeRowIndex = 0;
        _writeColumnIndex = 0;
        _pixelDrawList.Clear();

        _firstFrameRendered = true;
    }

    public void MoveCamera(Point offset)
    {
        ViewOffset.X += offset.X;
        ViewOffset.Y += offset.Y;
    }

    public Point FramePositionToWorldPosition(Point pos) => new(pos.X + ViewOffset.X, pos.Y + ViewOffset.Y);

    public void MoveCamera(int xOffset, int yOffset) => MoveCamera(new Point(xOffset, yOffset));
}