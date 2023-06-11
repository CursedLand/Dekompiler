using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core;

public class RendererConstants
{
    private readonly RendererService _service;

    public RendererConstants(RendererService service)
    {
        _service = service;
    }

    public RenderFragment RightCurlyBracket => _service.Text("{");

    public RenderFragment LeftCurlyBracket => _service.Text("}");

    public RenderFragment RightBracket => _service.Text("(");
    public RenderFragment LeftBracket => _service.Text(")");
    
    public RenderFragment RightArrayBracket => _service.Text("[");
    public RenderFragment LeftArrayBracket => _service.Text("]");

    

    public RenderFragment Dot => _service.Text(".");
    public RenderFragment Comma => _service.Text(",");
    public RenderFragment Equal => _service.Text("=");
    public RenderFragment Space => _service.Text(" ");
    public RenderFragment SemiColumn => _service.Text(";");
    public RenderFragment Column => _service.Text(":");
    
    public RenderFragment RightThan => _service.Text("<");
    public RenderFragment LeftThan => _service.Text(">");
    
    public RenderFragment Star => _service.Text("*");
    public RenderFragment Slash => _service.Text("/");
}