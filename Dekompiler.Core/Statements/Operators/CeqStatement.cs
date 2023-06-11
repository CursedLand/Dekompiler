using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public class CeqStatement : BinaryOperatorStatement
{
    public CeqStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ceq
    };

    public override RenderFragment Operator => Context.Renderer.Text("==");
}
