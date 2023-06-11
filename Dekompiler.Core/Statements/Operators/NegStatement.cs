using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public class NegStatement : UnaryOperatorStatement
{
    public NegStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Neg
    };

    public override RenderFragment Operator => Context.Renderer.Text("-");
}
