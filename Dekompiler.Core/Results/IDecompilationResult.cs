using AsmResolver.DotNet;
using Dekompiler.Core.Declaration;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Results;

public interface IDecompilationResult
{
    Icons.IconInfo Icon { get; }
    
    RenderFragment Header { get; }
    
    IMemberDefinition Member { get; }

    IMemberDeclaration Declaration { get; }

    RenderFragment RenderResult();

    internal static RenderFragment NewLine()
    {
        return builder =>
        {
            builder.OpenElement(0, "br");
            builder.CloseElement();
        };
    }
    
    internal static RenderFragment Indent(RenderFragment fragment, int indent = 10)
    {
        return builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddAttribute(1, "class", $"ml-{indent}");
            builder.AddContent(2, fragment);
            builder.CloseElement();
        };
    }
}