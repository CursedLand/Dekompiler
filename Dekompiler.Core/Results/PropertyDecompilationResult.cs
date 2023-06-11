using AsmResolver.DotNet;
using Dekompiler.Core.Declaration;
using Dekompiler.Core.Statements;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Results;

public class PropertyDecompilationResult : IDecompilationResult
{
    private readonly RendererService _service;
    private PropertyDefinition _property;

    public PropertyDecompilationResult(Icons.IconInfo icon, RenderFragment header, IMemberDeclaration declaration,
        MethodDecompilationResult? getDecompilationResult, MethodDecompilationResult? setDecompilationResult,
        RendererService service, PropertyDefinition property)
    {
        _service = service;
        _property = property;
        Icon = icon;
        Header = header;
        Declaration = declaration;
        GetDecompilationResult = getDecompilationResult;
        SetDecompilationResult = setDecompilationResult;
    }

    public Icons.IconInfo Icon { get; }
    public RenderFragment Header { get; }
    public IMemberDefinition Member => _property;
    public IMemberDeclaration Declaration { get; }

    public MethodDecompilationResult? GetDecompilationResult { get; }
    public MethodDecompilationResult? SetDecompilationResult { get; }


    public RenderFragment RenderResult()
    {
        return builder =>
        {
            builder.OpenElement(0, "p");

            builder.AddContent(2, _service.Concat(_service.Span($"// Token: 0x{_property.MetadataToken.ToInt32():x8} RID: {_property.MetadataToken.Rid}", "comment"), IDecompilationResult.NewLine()));
            
            builder.AddContent(2, Declaration.BuildDeclaration());

            builder.AddContent(3, IDecompilationResult.NewLine());

            builder.AddContent(4, _service.Constants.RightCurlyBracket);

            var sequence = 5;

            if (GetDecompilationResult is not null)
            {
                builder.AddContent(sequence++, IDecompilationResult.Indent(GetDecompilationResult.RenderResult()));
            }
            
            if (SetDecompilationResult is not null)
            {
                builder.AddContent(sequence++, IDecompilationResult.Indent(SetDecompilationResult.RenderResult()));
            }
            


            builder.AddContent(sequence++, _service.Constants.LeftCurlyBracket);

            builder.CloseElement();
        };
    }
}