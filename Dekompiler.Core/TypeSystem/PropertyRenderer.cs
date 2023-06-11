using AsmResolver.DotNet;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.TypeSystem;

public class PropertyRenderer
{
    private readonly RendererService _service;
    private PropertyDefinition? _property;

    public PropertyRenderer(RendererService service, PropertyDefinition property)
    {
        _property = property;
        _service = service;
    }

    public RenderFragment RenderMember()
    {
        var themeColor = _property!.Signature!.HasThis ? "instanceproperty" : "staticproperty";

        return _service.Span(_property.Name, themeColor);
    }
}