using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public class CltStatement : BinaryOperatorStatement
{
    public CltStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Clt,
        CilCode.Clt_Un
    };

    public override RenderFragment Operator => Context.Renderer.Text("<");
}
