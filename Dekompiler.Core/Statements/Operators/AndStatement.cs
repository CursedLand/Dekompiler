using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public class AndStatement : BinaryOperatorStatement
{
    public AndStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.And
    };

    public override RenderFragment Operator => Context.Renderer.Text("&");
}
