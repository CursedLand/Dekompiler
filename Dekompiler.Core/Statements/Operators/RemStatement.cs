using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public class RemStatement : BinaryOperatorStatement
{
    public RemStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Rem,
        CilCode.Rem_Un
    };

    public override RenderFragment Operator => Context.Renderer.Text("%");
}
