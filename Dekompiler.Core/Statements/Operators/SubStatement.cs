using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public class SubStatement : BinaryOperatorStatement
{
    public SubStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Sub,
        CilCode.Sub_Ovf,
        CilCode.Sub_Ovf_Un
    };

    public override RenderFragment Operator => Context.Renderer.Text("-");
}
