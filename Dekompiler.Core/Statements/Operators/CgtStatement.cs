using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public class CgtStatement : BinaryOperatorStatement
{
    public CgtStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Cgt,
        CilCode.Cgt_Un
    };

    public override RenderFragment Operator => Context.Renderer.Text(">");
}
