using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public class OrStatement : BinaryOperatorStatement
{
    public OrStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Or
    };

    public override RenderFragment Operator => Context.Renderer.Text("|");
}
