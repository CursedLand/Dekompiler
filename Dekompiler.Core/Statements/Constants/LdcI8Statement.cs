using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Constants;

public class LdcI8Statement : PushableStatement
{
    private long _value;

    public LdcI8Statement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ldc_I8
    };

    public override TypeSignature Type => Context.Method.Module!.CorLibTypeFactory.Int64;

    public override void Deserialize(CilInstruction instruction)
    {
        _value = (long)instruction.Operand!;
    }

    public override RenderFragment Render()
    {
        return Context.Renderer.Span(string.Format("{0}L", _value), "number");
    }
}
