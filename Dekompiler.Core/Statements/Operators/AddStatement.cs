using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public class AddStatement : BinaryOperatorStatement
{
    public AddStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Add,
        CilCode.Add_Ovf,
        CilCode.Add_Ovf_Un
    };

    public override RenderFragment Operator => Context.Renderer.Text("+");
}
