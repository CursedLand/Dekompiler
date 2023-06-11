using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public class NotStatement : UnaryOperatorStatement
{
    public NotStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Not
    };

    public override RenderFragment Operator => Context.Renderer.Text("~");
}
