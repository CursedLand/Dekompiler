using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public abstract class UnaryOperatorStatement : PushableStatement
{
    private IStatement? _right;

    protected UnaryOperatorStatement(MethodContext context)
        : base(context)
    {
    }

    public abstract RenderFragment Operator
    {
        get;
    }

    public override TypeSignature Type => _right!.Type;

    public override void Deserialize(CilInstruction instruction)
    {
        _right = Context.Stack.Pop();
    }

    public override RenderFragment Render()
    {
        var rightSpan = _right!.Render();

        return Context.Renderer.Concat(Operator, rightSpan);
    }
}
