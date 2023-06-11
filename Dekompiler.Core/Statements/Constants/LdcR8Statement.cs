using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Constants;

public class LdcR8Statement : PushableStatement
{
    private double _value;

    public LdcR8Statement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ldc_R8
    };

    public override TypeSignature Type => Context.Method.Module!.CorLibTypeFactory.Double;

    public override void Deserialize(CilInstruction instruction)
    {
        _value = (double)instruction.Operand!;
    }

    public override RenderFragment Render()
    {
        return Context.Renderer.Span(string.Format("{0}f", _value), "number");
    }
}
