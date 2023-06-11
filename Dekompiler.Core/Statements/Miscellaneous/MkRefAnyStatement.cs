using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Miscellaneous;

public class MkRefAnyStatement : PushableStatement
{
    private IStatement? _value;

    public MkRefAnyStatement(MethodContext context) : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Mkrefany
    };

    public override TypeSignature Type => Context.Method.Module!.CorLibTypeFactory.TypedReference;

    public override void Deserialize(CilInstruction instruction)
    {
        _value = Context.Stack.Pop();
    }

    public override RenderFragment Render()
    {
        var makeRefKeyword = Context.Renderer.Keyword("__makeref");

        var valueSpan = _value!.Render();

        return Context.Renderer.Concat(makeRefKeyword, Context.Renderer.Constants.RightBracket, valueSpan,
            Context.Renderer.Constants.LeftBracket);
    }
}