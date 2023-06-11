using Dekompiler.Core.Results;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Components.CodeViewer;

public partial class CodeView : ComponentBase
{
    private IDecompilationResult? _result;

    public IDecompilationResult? Result
    {
        get => _result;
        set
        {
            _result = value;
            StateHasChanged();
        } 
    }
}