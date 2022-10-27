namespace TerminalRenderer;

public interface ITerminalRenderable
{
    public IEnumerable<TerminalPixel> Render();
}