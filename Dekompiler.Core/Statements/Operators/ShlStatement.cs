using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public class ShlStatement : BinaryOperatorStatement
{
    public ShlStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Shl
    };

    public override RenderFragment Operator => Context.Renderer.Text("<<");
}
