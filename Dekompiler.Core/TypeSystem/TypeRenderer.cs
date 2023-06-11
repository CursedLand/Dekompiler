using AsmResolver.DotNet;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.TypeSystem;

public class TypeRenderer
{
    private readonly RendererService _service;
    private ITypeDefOrRef? _member;

    public TypeRenderer(RendererService service, ITypeDefOrRef type)
    {
        _member = type;
        _service = service;
    }

    public RenderFragment RenderMember()
    {
        return _member switch
        {
            TypeDefinition typeDefinition => RenderTypeDefinition(typeDefinition),
            TypeReference typeReference => RenderTypeReference(typeReference),
            TypeSpecification typeSpecification => RenderTypeSpecification(typeSpecification),
            _ => throw new ArgumentOutOfRangeException(nameof(_member))
        };
    }

    private RenderFragment RenderTypeSpecification(TypeSpecification type)
    {
        return new TypeSignatureRenderer(_service, type.Signature!, type).RenderMember();
    }

    private RenderFragment RenderTypeReference(TypeReference type)
    {
        return RenderTypeDefinition(type.Resolve()!);
    }

    private RenderFragment RenderTypeDefinition(TypeDefinition type)
    {
        var themeColor = type switch
        {
            { IsInterface: true } => "interface",
            { IsDelegate: true } => "delegate",
            { IsEnum: true } => "enum",
            { IsValueType: true } => "valuetype",
            { IsSealed: true, IsAbstract: true } => "statictype",
            { IsSealed: true } => "sealedtype",
            _ => "type"
        };

        var index = type.Name!.LastIndexOf('`');
        return _service.Span(type.GenericParameters.Any() && index is not -1
            ? type.Name?.Value.Substring(0, index)
            : type.Name?.Value, themeColor);
    }
}