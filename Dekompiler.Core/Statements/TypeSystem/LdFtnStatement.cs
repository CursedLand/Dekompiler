using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.TypeSystem;

public class LdFtnStatement : PushableStatement
{
    private IMethodDescriptor? _method;
    private bool _virtual = false;

    public LdFtnStatement(MethodContext context) : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ldftn,
        CilCode.Ldvirtftn
    };

    public override TypeSignature Type => Context.Method.Module!.CorLibTypeFactory.IntPtr;

    public override void Deserialize(CilInstruction instruction)
    {
        _method = (IMethodDescriptor)instruction.Operand!;
        _virtual = instruction.OpCode.Code is CilCode.Ldvirtftn;
    }

    public override RenderFragment Render()
    {
        var ftnSpan = Context.Renderer.Span(_virtual ? "ldvirtftn" : "ldftn", "opcode");

        return Context.Renderer.Concat(ftnSpan, Context.Renderer.Constants.RightBracket,
            new MethodRenderer(Context.Renderer, _method!).RenderMember(), Context.Renderer.Constants.LeftBracket);
    }
}