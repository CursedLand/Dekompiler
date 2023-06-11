using AsmResolver.DotNet.Collections;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Arguments;

public class StArgStatement : CompleteStatement
{
    private Parameter? _parameter;
    private IStatement? _value;

    public StArgStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Starg,
        CilCode.Starg_S
    };

    public override void Deserialize(CilInstruction instruction)
    {
        _parameter = (Parameter)instruction.Operand!;
        _value = Context.Stack.Pop();
    }

    public override RenderFragment Render()
    {
        var parameter = _parameter!.Index is -1
            ? Context.Renderer.Keyword("this")
            : Context.Renderer.Span(_parameter!.Name, "parameter");
        var value = _value!.Render();

        return Context.Renderer.Concat(parameter, Context.Renderer.Constants.Space, Context.Renderer.Constants.Equal,
            Context.Renderer.Constants.Space, value);
    }
}
