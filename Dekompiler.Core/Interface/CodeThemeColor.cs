namespace Dekompiler.Core.Interface;

public class CodeThemeColor
{
    public CodeThemeColor(string foreground, string? background = null, bool italic = false, bool bold = false)
    {
        Foreground = foreground;
        Background = background;
        Italic = italic;
        Bold = bold;
    }

    public string Foreground
    {
        get;
    }

    public string? Background
    {
        get;
    }

    public bool Bold
    {
        get;
    }

    public bool Italic
    {
        get;
    }

    public override string ToString()
    {
        return string.Join(' ', $"text-[{Foreground}]",
            Italic ? "italic" : "not-italic",
            Bold ? "font-semibold" : "font-normal",
            Background is null ? null : $"bg-[{Background}]");
    }
}
