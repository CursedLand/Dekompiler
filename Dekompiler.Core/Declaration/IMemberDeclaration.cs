using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Declaration;

public interface IMemberDeclaration
{
    RenderFragment BuildDeclaration();
}