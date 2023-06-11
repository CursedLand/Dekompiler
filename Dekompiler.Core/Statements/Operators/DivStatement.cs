using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public class DivStatement : BinaryOperatorStatement
{
    public DivStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Div,
        CilCode.Div_Un
    };

    public override RenderFragment Operator => Context.Renderer.Text("/");
}
