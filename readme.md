# TerminalRenderer
The TerminalRenderer library is one that makes it easy to render shapes and images directly in the terminal.

### External Libraries
* [Pastel](https://github.com/silkfire/Pastel): for custom background and foreground color usage in the terminal
* [ImageSharp](https://github.com/SixLabors/ImageSharp): as a cross-platform means of loading, using, or generating images to render in the terminal

# Example

Code:
```csharp
TerminalDisplay display = new();

// create a new pixel with a content of "* "
// this uses the default foreground and background colors (white, black)
TerminalPixel pixel = new TerminalPixel()
{
    BackgroundColor = Color.SteelBlue,
    PixelContent = "* "
};

// change background color
display.Draw(TerminalRectangle.Centered(new Point(0, 0), display.Width, display.Height, new TerminalPixel(Color.SteelBlue)));

// draw the origin point
display.Draw(new TerminalPixel(new Point(0, 0), Color.Red));

// example shapes
display.Draw(TerminalRectangle.Centered(new Point(-13, 10), 12, 6, new TerminalPixel(Color.Coral)));
display.Draw(new TerminalLine(new Point(5, -6), new Point(17, 8), pixel with {ForegroundColor = Color.Chartreuse}));
const int P = -12;
display.Draw(new TerminalPolygon(pixel with {ForegroundColor = Color.Beige}, new Point[]
{
    new (P - 06, P - 08),
    new (P + 06, P - 08),
    new (P + 12, P + 00),
    new (P - 00, P - 04),
    new (P - 12, P + 00),
}));

// clears the console and renders what has been drawn
display.Render();

Console.ReadLine(); // pause execution
```
Output:
![The render output](https://i.imgur.com/u57ezfi.png)

# More Advanced Rendering
This library allows and is optimized for rendering more than once. This means that animations or dynamic rendering is possible.
![Rainbow spinny thing](https://i.imgur.com/GT6VjUg.gif)

