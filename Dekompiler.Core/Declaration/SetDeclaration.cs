using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Declaration;

public class SetDeclaration : IMemberDeclaration
{
    private RendererService _service;

    public SetDeclaration(RendererService service)
    {
        _service = service;
    }
        
    public RenderFragment BuildDeclaration()
    {
        return _service.Keyword("set");
    }
}