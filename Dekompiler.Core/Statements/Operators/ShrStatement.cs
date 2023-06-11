using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public class ShrStatement : BinaryOperatorStatement
{
    public ShrStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Shr,
        CilCode.Shr_Un
    };

    public override RenderFragment Operator => Context.Renderer.Text(">>");
}
