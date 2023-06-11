using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Miscellaneous;

public class ThrowStatement : CompleteStatement
{
    private IStatement? _exception;

    public ThrowStatement(MethodContext context) : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Throw
    };

    public override void Deserialize(CilInstruction instruction)
    {
        _exception = Context.Stack.Pop();
    }

    public override RenderFragment Render()
    {
        var throwSpan = Context.Renderer.Keyword("throw");

        return Context.Renderer.Join(Context.Renderer.Constants.Space, throwSpan, _exception!.Render());
    }
}