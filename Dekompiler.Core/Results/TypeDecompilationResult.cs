using System.Collections;
using AsmResolver.DotNet;
using Dekompiler.Core.Declaration;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Results;

public class TypeDecompilationResult : IDecompilationResult
{
    private readonly TypeDefinition _type;
    private readonly RendererService _service;

    public TypeDecompilationResult(Icons.IconInfo icon, RenderFragment header, IMemberDeclaration declaration,
        TypeDefinition type, RendererService service)
    {
        _type = type;
        _service = service;
        Icon = icon;
        Header = header;
        Declaration = declaration;
    }

    public Icons.IconInfo Icon { get; }
    public RenderFragment Header { get; }
    public IMemberDefinition Member => _type;
    public IMemberDeclaration Declaration { get; }
    public List<MethodDecompilationResult> MethodResults { get; } = new();
    public List<PropertyDecompilationResult> PropertyResults { get; } = new();
    public List<EventDecompilationResult> EventResults { get; } = new();
    public List<TypeDecompilationResult> NestedTypeResults { get; } = new();
    public List<FieldDecompilationResult> FieldResults { get; } = new();


    public RenderFragment RenderResult()
    {
        var internalRender = InternalRender();

        if (!string.IsNullOrEmpty(_type.Namespace) && !_type.IsNested)
        {
            return builder =>
            {
                var ns = _service.Join(_service.Constants.Dot,
                    _type.Namespace.Value.Split('.').Select(n => _service.Span(n, "namespace")).ToArray());
                var nsKeyword = _service.Keyword("namespace");

                builder.OpenElement(0, "p");

                builder.AddContent(1, _service.Concat(nsKeyword, _service.Constants.Space, ns));

                builder.AddContent(2, IDecompilationResult.NewLine());

                builder.AddContent(3, _service.Constants.RightCurlyBracket);

                builder.AddContent(4, IDecompilationResult.Indent(internalRender));

                builder.AddContent(5, _service.Constants.LeftCurlyBracket);

                builder.CloseElement();
            };
        }
        else
        {
            return internalRender;
        }
    }


    private RenderFragment InternalRender()
    {
        return builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddAttribute(1, "typeToken", _type.MetadataToken.ToString());
            
            builder.AddContent(2, _service.Concat(_service.Span($"// Token: 0x{_type.MetadataToken.ToInt32():x8} RID: {_type.MetadataToken.Rid}", "comment"), IDecompilationResult.NewLine()));

            builder.AddContent(3, Declaration.BuildDeclaration());

            builder.AddContent(4, IDecompilationResult.NewLine());

            builder.AddContent(5, _service.Constants.RightCurlyBracket);

            builder.AddContent(6, IDecompilationResult.NewLine());

            var sequence = 6;

            foreach (var methodResult in MethodResults)
            {
                builder.AddContent(sequence++,
                    IDecompilationResult.Indent(methodResult.RenderResult()));
                builder.AddContent(sequence++,
                    IDecompilationResult.NewLine());
            }

            foreach (var propertyResult in PropertyResults)
            {
                builder.AddContent(sequence++,
                    IDecompilationResult.Indent(propertyResult.RenderResult()));
                builder.AddContent(sequence++,
                    IDecompilationResult.NewLine());
            }
            
            foreach (var eventResult in EventResults)
            {
                builder.AddContent(sequence++,
                    IDecompilationResult.Indent(eventResult.RenderResult()));
                builder.AddContent(sequence++,
                    IDecompilationResult.NewLine());
            }

            foreach (var fieldResult in FieldResults)
            {
                if ((FieldDefinition)fieldResult.Member is
                    { DeclaringType: { BaseType: { FullName: "System.Enum" } } })
                {
                    builder.AddContent(sequence++,
                        IDecompilationResult.Indent(fieldResult.RenderResult()));
                }
                else
                {
                    builder.AddContent(sequence++,
                        IDecompilationResult.Indent(_service.Concat(fieldResult.RenderResult(),
                            _service.Constants.SemiColumn)));
                    builder.AddContent(sequence++,
                        IDecompilationResult.NewLine());
                }
            }

            foreach (var nestedTypeResult in NestedTypeResults)
            {
                builder.AddContent(sequence++,
                    IDecompilationResult.Indent(nestedTypeResult.RenderResult()));
                builder.AddContent(sequence++,
                    IDecompilationResult.NewLine());
            }

            builder.AddContent(sequence, _service.Constants.LeftCurlyBracket);

            builder.CloseElement();
        };
    }
}