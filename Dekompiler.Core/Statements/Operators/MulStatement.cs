using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public class MulStatement : BinaryOperatorStatement
{
    public MulStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Mul,
        CilCode.Mul_Ovf,
        CilCode.Mul_Ovf_Un
    };

    public override RenderFragment Operator => Context.Renderer.Text("*");
}
