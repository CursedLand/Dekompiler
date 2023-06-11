using AsmResolver.DotNet;
using Dekompiler.Core.Declaration;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Results;

public class EventDecompilationResult : IDecompilationResult
{
    private readonly RendererService _service;
    private EventDefinition _event;

    public EventDecompilationResult(Icons.IconInfo icon, RenderFragment header, IMemberDeclaration declaration,
        MethodDecompilationResult? addDecompilationResult, MethodDecompilationResult? removeDecompilationResult,
        RendererService service, EventDefinition @event)
    {
        _service = service;
        _event = @event;
        Icon = icon;
        Header = header;
        Declaration = declaration;
        AddDecompilationResult = addDecompilationResult;
        RemoveDecompilationResult = removeDecompilationResult;
    }

    public Icons.IconInfo Icon { get; }
    public RenderFragment Header { get; }
    public IMemberDefinition Member => _event;
    public IMemberDeclaration Declaration { get; }

    public MethodDecompilationResult? AddDecompilationResult { get; }
    public MethodDecompilationResult? RemoveDecompilationResult { get; }


    public RenderFragment RenderResult()
    {
        return builder =>
        {
            builder.OpenElement(0, "p");

            builder.AddContent(1, Declaration.BuildDeclaration());

            builder.AddContent(2, IDecompilationResult.NewLine());

            builder.AddContent(3, _service.Constants.RightCurlyBracket);

            var sequence = 4;

            if (AddDecompilationResult is not null)
            {
                builder.AddContent(sequence++, IDecompilationResult.Indent(AddDecompilationResult.RenderResult()));
            }
            
            if (RemoveDecompilationResult is not null)
            {
                builder.AddContent(sequence++, IDecompilationResult.Indent(RemoveDecompilationResult.RenderResult()));
            }
            


            builder.AddContent(sequence++, _service.Constants.LeftCurlyBracket);

            builder.CloseElement();
        };
    }
}