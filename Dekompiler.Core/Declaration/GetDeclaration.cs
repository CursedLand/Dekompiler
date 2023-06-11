using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Declaration;

public class GetDeclaration : IMemberDeclaration
{
    private RendererService _service;

    public GetDeclaration(RendererService service)
    {
        _service = service;
    }

    public RenderFragment BuildDeclaration()
    {
        return _service.Keyword("get");
    }
}