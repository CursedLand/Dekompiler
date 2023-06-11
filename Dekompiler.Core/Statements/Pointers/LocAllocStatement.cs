using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Pointers;

// TODO: Use dfg (Data flow graph) to indicate this instruction type before deserializing process. Example: locAlloc, stLoc.0 (int32*, PointerTypeSignature).
public class LocAllocStatement : PushableStatement
{
    private IStatement? _size;

    public LocAllocStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Localloc
    };

    public override TypeSignature Type => Context.Method.Module!.CorLibTypeFactory.Byte;

    public override void Deserialize(CilInstruction instruction)
    {
        _size = Context.Stack.Pop();
    }

    public override RenderFragment Render()
    {
        var sizeSpan = _size!.Render();
        var allocSpan = Context.Renderer.Keyword("stackalloc");

        return Context.Renderer.Concat(allocSpan, Context.Renderer.Constants.Space,
            new TypeSignatureRenderer(Context.Renderer, Type, Context.Method, Context.Method.DeclaringType)
                .RenderMember(), Context.Renderer.Constants.RightArrayBracket, sizeSpan,
            Context.Renderer.Constants.LeftArrayBracket);
    }
}
