namespace Dekompiler.Core.Interface;

public class ThemeConfiguration
{
    public static readonly ThemeConfiguration Default = new()
    {
        Branding = "#3b82f6",
        Primary = "#ffffff",
        Secondary = "#f7f8fa",
        Border = "#e5e4ec",
        Icon = "#212121",
        CodeTheme = CodeThemeConfiguration.Default
    };

    public static readonly ThemeConfiguration Dark = new()
    {
        Branding = "#3b82f6",
        Primary = "#141517",
        Secondary = "#161718",
        Border = "#404040",
        Icon = "#f5f5f5",
        CodeTheme = CodeThemeConfiguration.Dark
    };

    public string Icon { get; set; } = "#212121";
    public string Branding { get; set; } = "#3b82f6";

    public string Primary { get; set; } = "#ffffff";

    public string Secondary { get; set; } = "#f7f8fa";

    public string Border { get; set; } = "#e5e4ec";

    public CodeThemeConfiguration CodeTheme { get; set; } = CodeThemeConfiguration.Default;
}