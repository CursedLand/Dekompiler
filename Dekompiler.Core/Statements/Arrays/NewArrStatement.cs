using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Arrays;

public class NewArrStatement : PushableStatement
{
    private IStatement? _arrayLength;
    private TypeSignature? _arrayType;

    public NewArrStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Newarr
    };

    public override TypeSignature Type => _arrayType!.MakeSzArrayType();

    public override void Deserialize(CilInstruction instruction)
    {
        _arrayLength = Context.Stack.Pop();
        _arrayType = ((ITypeDefOrRef)instruction.Operand!).ToTypeSignature();
    }

    public override RenderFragment Render()
    {
        var lengthSpan = _arrayLength!.Render();
        var newSpan = Context.Renderer.Keyword("new");

        return Context.Renderer.Concat(newSpan, Context.Renderer.Constants.Space,
            new TypeSignatureRenderer(Context.Renderer, _arrayType!, Context.Method, Context.Method.DeclaringType)
                .RenderMember(),
            Context.Renderer.Constants.RightArrayBracket, lengthSpan,
            Context.Renderer.Constants.LeftArrayBracket);
    }
}
