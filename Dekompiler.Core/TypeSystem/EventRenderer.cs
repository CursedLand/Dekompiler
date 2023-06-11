using AsmResolver.DotNet;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.TypeSystem;

public class EventRenderer
{
    private readonly RendererService _service;
    private EventDefinition? _event;

    public EventRenderer(RendererService service, EventDefinition @event)
    {
        _event = @event;
        _service = service;
    }

    public RenderFragment RenderMember()
    {
        var tmp = _event!.AddMethod ?? _event!.FireMethod ?? _event!.RemoveMethod;
        
        var themeColor = tmp!.IsStatic ? "staticevent" : "instanceevent";

        return _service.Span(_event.Name, themeColor);
    }
}