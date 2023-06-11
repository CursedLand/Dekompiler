using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Miscellaneous;

public class RetStatement : Statement
{
    private TypeSignature? _retType;
    private IStatement? _retValue;

    public RetStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ret
    };

    public override TypeSignature Type => _retType!;

    public override void InterpretStack()
    {
        if (Context.Method.Signature!.ReturnsValue)
            Context.CompleteStatements.Add(this);
    }

    public override void Deserialize(CilInstruction instruction)
    {
        if (Context.Method.Signature!.ReturnsValue)
        {
            _retValue = Context.Stack.Pop().TryCast(Context.Method.Signature.ReturnType);
            _retType = _retValue.Type;
        }
    }

    public override RenderFragment Render()
    {
        var keyword = Context.Renderer.Keyword("return");
        if (_retValue is null)
            return keyword;
        return Context.Renderer.Concat(keyword, Context.Renderer.Constants.Space, _retValue.Render());
    }
}
