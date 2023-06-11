using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.TypeSystem;

public class SizeOfStatement : PushableStatement
{
    private ITypeDefOrRef? _type;

    public SizeOfStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Sizeof
    };

    public override TypeSignature Type => Context.Method.Module!.CorLibTypeFactory.Int32;

    public override void Deserialize(CilInstruction instruction)
    {
        _type = (ITypeDefOrRef)instruction.Operand!;
    }

    public override RenderFragment Render()
    {
        var sizeOfSpan = Context.Renderer.Keyword("sizeof");

        return Context.Renderer.Concat(sizeOfSpan, Context.Renderer.Constants.RightBracket,
            new TypeSignatureRenderer(Context.Renderer, _type!.ToTypeSignature(), Context.Method, Context.Method.DeclaringType!).RenderMember(),
            Context.Renderer.Constants.LeftBracket);
    }
}