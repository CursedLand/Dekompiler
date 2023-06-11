using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.TypeSystem;

public class FieldRenderer
{
    private readonly RendererService _service;
    private IFieldDescriptor? _member;

    public FieldRenderer(RendererService service, IFieldDescriptor field)
    {
        _member = field;
        _service = service;
    }

    public RenderFragment RenderMember()
    {

        var member = _member switch
        {
            FieldDefinition fieldDefinition => RenderFieldDefinition(fieldDefinition),
            MemberReference memberReference => new FieldRenderer(_service, (IFieldDescriptor)memberReference.Resolve()!).RenderMember(),
            _ => throw new ArgumentOutOfRangeException(nameof(_member))
        };

        return member;
    }

    private RenderFragment RenderFieldDefinition(FieldDefinition field)
    {
        var themeColor = field switch
        {
            { DeclaringType: { BaseType: { FullName: "System.Enum" } } } => "enumfield",
            { IsLiteral: true } => "literalfield",
            { IsStatic: true } => "staticfield",
            { IsStatic: false } => "instancefield"
        };

        return _service.Span(field.Name, themeColor);
    }
}