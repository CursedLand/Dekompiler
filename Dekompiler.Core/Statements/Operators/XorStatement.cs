using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public class XorStatement : BinaryOperatorStatement
{
    public XorStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Xor
    };

    public override RenderFragment Operator => Context.Renderer.Text("^");
}
