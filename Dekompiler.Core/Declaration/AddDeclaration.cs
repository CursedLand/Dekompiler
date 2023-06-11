using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Declaration;

public class AddDeclaration : IMemberDeclaration
{
    private RendererService _service;

    public AddDeclaration(RendererService service)
    {
        _service = service;
    }

    public RenderFragment BuildDeclaration()
    {
        return _service.Keyword("add");
    }
}