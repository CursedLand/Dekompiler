using AsmResolver;
using Dekompiler.Core.Interface;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core;

public class RendererService
{
    public RendererService(ThemeConfiguration themeConfiguration)
    {
        Theme = themeConfiguration;
        Constants = new RendererConstants(this);
    }

    public ThemeConfiguration Theme { get; }

    public RendererConstants Constants { get; }

    public RenderFragment Concat(params RenderFragment?[] fragments)
    {
        fragments = fragments.Where(f => f is not null).ToArray();
        return builder =>
        {
            builder.OpenElement(0, "span");
            for (var i = 1; i < fragments.Length + 1; i++)
                builder.AddContent(i, fragments[i - 1]);
            builder.CloseElement();
        };
    }

    public RenderFragment Join(RenderFragment separator, params RenderFragment?[] fragments)
    {
        var sequence = 1;
        fragments = fragments.Where(f => f is not null).ToArray();
        return builder =>
        {
            builder.OpenElement(0, "span");
            for (var i = 0; i < fragments.Length; i++)
            {
                var content = fragments[i];

                if (content is null)
                    continue;

                builder.AddContent(sequence++, content);
                if (i != fragments.Length - 1)
                    builder.AddContent(sequence++, separator);
            }

            builder.CloseElement();
        };
    }

    public RenderFragment Span(Utf8String? content, string themeColor) => Span((string?)content, themeColor);

    public RenderFragment Span(Utf8String? content, CodeThemeColor themeColor) => Span((string?)content, themeColor);

    public RenderFragment Span(string? content, string themeColor) =>
        Span(content, Theme.CodeTheme.GetThemeColor(themeColor));

    public RenderFragment Span(string? content, CodeThemeColor themeColor)
    {
        return builder =>
        {
            builder.OpenElement(0, "span");
            builder.AddAttribute(1, "class", themeColor.ToString());
            builder.AddContent(2, content);
            builder.CloseElement();
        };
    }

    public RenderFragment Keyword(string? keyword) => Span(keyword, "keyword");
    public RenderFragment Text(string? text) => Span(text, "defaulttext");
}