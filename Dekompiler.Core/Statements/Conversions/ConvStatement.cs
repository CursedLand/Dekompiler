using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Core.Statements.Operators;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Conversions;

public class ConvStatement : PushableStatement
{
    private IStatement? _value;
    private TypeSignature? _convType;

    public ConvStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Conv_I1,
        CilCode.Conv_I2,
        CilCode.Conv_I4,
        CilCode.Conv_I8,
        CilCode.Conv_R4,
        CilCode.Conv_R8,
        CilCode.Conv_U4,
        CilCode.Conv_U1,
        CilCode.Conv_U2,
        CilCode.Conv_U8,
        CilCode.Conv_I,
        CilCode.Conv_U,
        CilCode.Conv_R_Un,
        CilCode.Conv_Ovf_I1,
        CilCode.Conv_Ovf_U1,
        CilCode.Conv_Ovf_I2,
        CilCode.Conv_Ovf_U2,
        CilCode.Conv_Ovf_I4,
        CilCode.Conv_Ovf_U4,
        CilCode.Conv_Ovf_I8,
        CilCode.Conv_Ovf_U8,
        CilCode.Conv_Ovf_I,
        CilCode.Conv_Ovf_U,
        CilCode.Conv_Ovf_I1_Un,
        CilCode.Conv_Ovf_I2_Un,
        CilCode.Conv_Ovf_I4_Un,
        CilCode.Conv_Ovf_I8_Un,
        CilCode.Conv_Ovf_U1_Un,
        CilCode.Conv_Ovf_U2_Un,
        CilCode.Conv_Ovf_U4_Un,
        CilCode.Conv_Ovf_U8_Un,
        CilCode.Conv_Ovf_I_Un,
        CilCode.Conv_Ovf_U_Un,
    };

    public override TypeSignature Type => _convType!;

    public override void Deserialize(CilInstruction instruction)
    {
        _value = Context.Stack.Pop();

        var corLib = Context.Method.Module!.CorLibTypeFactory;

        _convType = instruction.OpCode.Code switch
        {
            CilCode.Conv_I or CilCode.Conv_Ovf_I or CilCode.Conv_Ovf_I_Un => corLib.IntPtr,
            CilCode.Conv_I1 or CilCode.Conv_Ovf_I1 or CilCode.Conv_Ovf_I1_Un => corLib.SByte,
            CilCode.Conv_I2 or CilCode.Conv_Ovf_I2 or CilCode.Conv_Ovf_I2_Un => corLib.Int16,
            CilCode.Conv_I4 or CilCode.Conv_Ovf_I4 or CilCode.Conv_Ovf_I4_Un => corLib.Int32,
            CilCode.Conv_I8 or CilCode.Conv_Ovf_I8 or CilCode.Conv_Ovf_I8_Un => corLib.Int64,
            CilCode.Conv_U or CilCode.Conv_Ovf_U or CilCode.Conv_Ovf_U_Un => corLib.UIntPtr,
            CilCode.Conv_U1 or CilCode.Conv_Ovf_U1 or CilCode.Conv_Ovf_U1_Un => corLib.Byte,
            CilCode.Conv_U2 or CilCode.Conv_Ovf_U2 or CilCode.Conv_Ovf_U2_Un => corLib.UInt16,
            CilCode.Conv_U4 or CilCode.Conv_Ovf_U4 or CilCode.Conv_Ovf_U4_Un => corLib.UInt32,
            CilCode.Conv_U8 or CilCode.Conv_Ovf_U8 or CilCode.Conv_Ovf_U8_Un => corLib.UInt64,
            CilCode.Conv_R4 => corLib.Single,
            CilCode.Conv_R8 or CilCode.Conv_R_Un => corLib.Double,
            _ => throw new ArgumentOutOfRangeException(nameof(instruction.OpCode.Code))
        };
    }

    public override RenderFragment Render()
    {
        var valueSpan = _value!.Render();

        var convSpan =
            new TypeSignatureRenderer(Context.Renderer, _convType!, Context.Method, Context.Method.DeclaringType)
                .RenderMember();

        if (_value is BinaryOperatorStatement or UnaryOperatorStatement)
        {
            valueSpan = Context.Renderer.Concat(Context.Renderer.Constants.RightBracket, valueSpan,
                Context.Renderer.Constants.LeftBracket);
        }

        return Context.Renderer.Concat(Context.Renderer.Constants.RightBracket, convSpan,
            Context.Renderer.Constants.LeftBracket, valueSpan);
    }
}