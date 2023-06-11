using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Miscellaneous;

public class PopStatement : CompleteStatement
{
    private IStatement? _value;

    public PopStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Pop
    };

    public override void Deserialize(CilInstruction instruction)
    {
        _value = Context.Stack.Pop();
    }

    public override RenderFragment Render()
    {
        return _value!.Render();
    }
}
