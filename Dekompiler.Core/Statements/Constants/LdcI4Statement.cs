using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Constants;

public class LdcI4Statement : PushableStatement
{
    private int _value;

    public LdcI4Statement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ldc_I4_M1,
        CilCode.Ldc_I4_0,
        CilCode.Ldc_I4_1,
        CilCode.Ldc_I4_2,
        CilCode.Ldc_I4_3,
        CilCode.Ldc_I4_4,
        CilCode.Ldc_I4_5,
        CilCode.Ldc_I4_6,
        CilCode.Ldc_I4_7,
        CilCode.Ldc_I4_8,
        CilCode.Ldc_I4_S,
        CilCode.Ldc_I4
    };

    public override TypeSignature Type => Context.Method.Module!.CorLibTypeFactory.Int32;

    public int Value => _value;
    
    public override void Deserialize(CilInstruction instruction)
    {
        _value = instruction.GetLdcI4Constant();
    }

    public override RenderFragment Render()
    {
        return Context.Renderer.Span(_value.ToString(), "number");
    }
}
