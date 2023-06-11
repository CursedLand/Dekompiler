using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Arrays;

public class LdLenStatement : PushableStatement
{
    private IStatement? _instance;

    public LdLenStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ldlen
    };

    public override TypeSignature Type => Context.Method.Module!.CorLibTypeFactory.Int32;

    public override void Deserialize(CilInstruction instruction)
    {
        _instance = Context.Stack.Pop();
    }

    public override RenderFragment Render()
    {
        var instanceSpan = _instance!.Render();

        return Context.Renderer.Concat(instanceSpan, Context.Renderer.Constants.Dot,
            Context.Renderer.Span("Length", "instanceproperty"));
    }
}
