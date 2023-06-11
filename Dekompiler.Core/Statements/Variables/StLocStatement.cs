using AsmResolver.DotNet.Code.Cil;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Variables;

public class StLocStatement : CompleteStatement
{
    private IStatement? _value;
    private CilLocalVariable? _variable;

    public StLocStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Stloc_0,
        CilCode.Stloc_1,
        CilCode.Stloc_2,
        CilCode.Stloc_3,
        CilCode.Stloc_S,
        CilCode.Stloc
    };

    public override void Deserialize(CilInstruction instruction)
    {
        _variable = (CilLocalVariable)instruction.Operand!;
        _value = Context.Stack.Pop();
    }

    public override RenderFragment Render()
    {
        var variableSpan = Context.Renderer.Span(Context.GenerateName(_variable!), "local");
        var value = _value!.Render();

        if (!Context.InitializedVariable[_variable!])
        {
            variableSpan = Context.Renderer.Concat(
                new TypeSignatureRenderer(Context.Renderer, _variable!.VariableType, Context.Method,
                    Context.Method.DeclaringType).RenderMember(),
                Context.Renderer.Constants.Space, variableSpan);
            Context.InitializedVariable[_variable] = true;
        }

        return Context.Renderer.Concat(variableSpan, Context.Renderer.Constants.Space, Context.Renderer.Constants.Equal,
            Context.Renderer.Constants.Space, value);
    }
}
