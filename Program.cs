using System.Drawing;
using TerminalRenderer;

TerminalDisplay display = new();

Point pos = default;

display.Draw(new TerminalPixel(pos with {Y = pos.Y - 1}, Color.Chartreuse));
display.Draw(new TerminalText(pos, "Hello, World!", new TerminalPixel{ForegroundColor = Color.Red}, TerminalText.Alignment.Center));

display.Render();

Console.ReadLine();