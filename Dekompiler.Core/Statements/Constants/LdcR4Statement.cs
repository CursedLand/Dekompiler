using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Constants;

public class LdcR4Statement : PushableStatement
{
    private float _value;

    public LdcR4Statement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ldc_R4
    };

    public override TypeSignature Type => Context.Method.Module!.CorLibTypeFactory.Single;

    public override void Deserialize(CilInstruction instruction)
    {
        _value = (float)instruction.Operand!;
    }

    public override RenderFragment Render()
    {
        return Context.Renderer.Span(string.Format("{0}f", _value), "number");
    }
}
