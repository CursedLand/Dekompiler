using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Declaration;

public class RemoveDeclaration : IMemberDeclaration
{
    private RendererService _service;

    public RemoveDeclaration(RendererService service)
    {
        _service = service;
    }

    public RenderFragment BuildDeclaration()
    {
        return _service.Keyword("remove");
    }
}