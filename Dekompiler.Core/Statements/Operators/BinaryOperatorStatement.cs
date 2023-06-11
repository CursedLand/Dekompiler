using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Operators;

public abstract class BinaryOperatorStatement : PushableStatement
{
    private IStatement? _left;
    private IStatement? _right;

    protected BinaryOperatorStatement(MethodContext context)
        : base(context)
    {
    }

    public abstract RenderFragment Operator
    {
        get;
    }

    public override TypeSignature Type => _left!.Type;

    public override void Deserialize(CilInstruction instruction)
    {
        _right = Context.Stack.Pop();
        _left = Context.Stack.Pop();
    }

    public override RenderFragment Render()
    {
        var leftSpan = _left!.Render();
        var rightSpan = _right!.Render();
        
        if (_right is BinaryOperatorStatement)
        {
            rightSpan = Context.Renderer.Concat(Context.Renderer.Constants.RightBracket, rightSpan,
                Context.Renderer.Constants.LeftBracket);
        }

        return Context.Renderer.Concat(leftSpan, Context.Renderer.Constants.Space, Operator,
            Context.Renderer.Constants.Space, rightSpan);
    }
}
