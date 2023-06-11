using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Arrays;

public class LdElemStatement : PushableStatement
{
    private IStatement? _index;
    private IStatement? _instance;
    private TypeSignature? _ldElemType;
    private bool _loadAddr;

    public LdElemStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ldelema,
        CilCode.Ldelem,
        CilCode.Ldelem_I1,
        CilCode.Ldelem_U1,
        CilCode.Ldelem_I2,
        CilCode.Ldelem_U2,
        CilCode.Ldelem_I4,
        CilCode.Ldelem_U4,
        CilCode.Ldelem_I8,
        CilCode.Ldelem_I,
        CilCode.Ldelem_R4,
        CilCode.Ldelem_R8,
        CilCode.Ldelem_Ref
    };

    public override TypeSignature Type => _ldElemType!;

    public override void Deserialize(CilInstruction instruction)
    {
        var corLib = Context.Method.Module!.CorLibTypeFactory;

        _index = Context.Stack.Pop();
        _instance = Context.Stack.Pop();

        _ldElemType = instruction.OpCode.Code switch
        {
            CilCode.Ldelem_I1 => corLib.SByte,
            CilCode.Ldelem_U1 => corLib.Byte,
            CilCode.Ldelem_I2 => corLib.Int16,
            CilCode.Ldelem_U2 => corLib.UInt16,
            CilCode.Ldelem_I4 => corLib.Int32,
            CilCode.Ldelem_U4 => corLib.UInt32,
            CilCode.Ldelem_I8 => corLib.Int64,
            CilCode.Ldelem_I => corLib.IntPtr,
            CilCode.Ldelem_R4 => corLib.Single,
            CilCode.Ldelem_R8 => corLib.Double,
            CilCode.Ldelem_Ref => corLib.Object,
            CilCode.Ldelem => ((ITypeDefOrRef)instruction.Operand!).ToTypeSignature(),
            CilCode.Ldelema => ((ITypeDefOrRef)instruction.Operand!).ToTypeSignature(),
            _ => throw new ArgumentOutOfRangeException()
        };

        _loadAddr = instruction.OpCode.Code is CilCode.Ldelema;
    }

    public override RenderFragment Render()
    {
        var instanceSpan = _instance!.Render();
        var indexSpan = _index!.Render();

        var ldElemSpan = Context.Renderer.Concat(instanceSpan, Context.Renderer.Constants.RightArrayBracket, indexSpan,
            Context.Renderer.Constants.LeftArrayBracket);

        if (_loadAddr && !_ldElemType!.IsValueType)
        {
            var loadAddrSpan = Context.Renderer.Span("ldelema", "opcode");

            return Context.Renderer.Concat(loadAddrSpan, Context.Renderer.Constants.RightBracket, ldElemSpan,
                Context.Renderer.Constants.LeftBracket);
        }

        return ldElemSpan;
    }
}
