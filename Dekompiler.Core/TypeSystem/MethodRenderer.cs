using AsmResolver.DotNet;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.TypeSystem;

public class MethodRenderer
{
    private readonly RendererService _service;
    private IMethodDescriptor? _member;

    public MethodRenderer(RendererService service, IMethodDescriptor method)
    {
        _member = method;
        _service = service;
    }

    public RenderFragment RenderMember()
    {
        return _member switch
        {
            MemberReference memberReference => new MethodRenderer(_service,
                (IMethodDescriptor)memberReference.Resolve()!).RenderMember(),
            MethodDefinition methodDefinition => RenderMethodDefinition(methodDefinition),
            MethodSpecification methodSpecification => RenderMethodSpecification(methodSpecification),
            _ => throw new ArgumentOutOfRangeException(nameof(_member))
        };
    }

    private RenderFragment RenderMethodDefinition(MethodDefinition method)
    {
        var themeColor = method switch
        {
            { IsStatic: true } staticMethod => staticMethod.Signature!.HasThis ? "extensionmethod" : "staticmethod",
            { IsStatic: false } => "instancemethod"
        };

        return _service.Span(method.Name, themeColor);
    }

    private RenderFragment RenderMethodSpecification(MethodSpecification method)
    {
        var methodSpan = RenderMethodDefinition(method.Resolve()!);

        var typeArgs = method.Signature!.TypeArguments
            .Select(arg =>
                new TypeSignatureRenderer(_service, arg, _member!, _member!.DeclaringType!.ToTypeDefOrRef())
                    .RenderMember()).ToArray();

        var separator = _service.Concat(_service.Constants.Comma, _service.Constants.Space);

        return _service.Concat(methodSpan, _service.Constants.RightThan, _service.Join(separator, typeArgs),
            _service.Constants.LeftThan);
    }
}