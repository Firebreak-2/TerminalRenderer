using System.Drawing;

namespace TerminalRenderer;

public class TerminalPolygon : ITerminalRenderable
{
    public Point[] Points = Array.Empty<Point>();
    public TerminalPixel PixelType = default;

    /// <summary>Determines if the given point is inside the polygon</summary>
    /// <param name="polygon">the vertices of polygon</param>
    /// <param name="testPoint">the given point</param>
    /// <returns>true if the point is inside the polygon; otherwise, false</returns>
    /// <remarks>Original by https://stackoverflow.com/users/1860071/meownet, refactored by Firebreak</remarks>
    private static bool IsPointInPolygon4(PointF[] polygon, PointF testPoint)
    {
        bool result = false;
        int length = polygon.Length;

        for (int i = 0, j = length - 1; i < length; i++)
        {
            result ^= (polygon[i].Y  < testPoint.Y  &&
                       polygon[j].Y >= testPoint.Y  ||
                       polygon[j].Y  < testPoint.Y  &&
                       polygon[i].Y >= testPoint.Y) 
                       &&
                       polygon[i].X + 
                      (testPoint.Y  - polygon[i].Y) /
                      (polygon[j].Y - polygon[i].Y) *
                      (polygon[j].X - polygon[i].X) < testPoint.X;
            
            j = i;
        }

        return result;
    }

    public TerminalPolygon(TerminalPixel pixel, params Point[] points)
    {
        PixelType = pixel;
        Points = points;
    }

    public bool Contains(Point point)
    {
        return IsPointInPolygon4(Points.Select(p => (PointF) p).ToArray(), point);
    }

    public IEnumerable<TerminalPixel> Render()
    {
        int lMost = Points.Min(p => p.X);
        int rMost = Points.Max(p => p.X);
        int tMost = Points.Min(p => p.Y);
        int bMost = Points.Max(p => p.Y);

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