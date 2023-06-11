using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Core.Statements.Operators;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Pointers;

public class LdIndStatement : PushableStatement
{
    private IStatement? _address;
    private TypeSignature? _ldIndType;

    public LdIndStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ldind_I1,
        CilCode.Ldind_U1,
        CilCode.Ldind_I2,
        CilCode.Ldind_U2,
        CilCode.Ldind_I4,
        CilCode.Ldind_U4,
        CilCode.Ldind_I8,
        CilCode.Ldind_I,
        CilCode.Ldind_R4,
        CilCode.Ldind_R8,
        CilCode.Ldind_Ref
    };

    public override TypeSignature Type => _ldIndType!;

    public override void Deserialize(CilInstruction instruction)
    {
        var corLib = Context.Method.Module!.CorLibTypeFactory;

        _address = Context.Stack.Pop();

        _ldIndType = instruction.OpCode.Code switch
        {
            CilCode.Ldind_I1 => corLib.SByte,
            CilCode.Ldind_U1 => corLib.Byte,
            CilCode.Ldind_I2 => corLib.Int16,
            CilCode.Ldind_U2 => corLib.UInt16,
            CilCode.Ldind_I4 => corLib.Int32,
            CilCode.Ldind_U4 => corLib.UInt32,
            CilCode.Ldind_I8 => corLib.Int64,
            CilCode.Ldind_I => corLib.IntPtr,
            CilCode.Ldind_R4 => corLib.Single,
            CilCode.Ldind_R8 => corLib.Double,
            CilCode.Ldind_Ref => corLib.Object,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override RenderFragment Render()
    {
        var castAddrSpan = Context.Renderer.Concat(Context.Renderer.Constants.Star,
            Context.Renderer.Constants.RightBracket,
            new TypeSignatureRenderer(Context.Renderer, _ldIndType!, Context.Method, Context.Method.DeclaringType)
                .RenderMember(),
            Context.Renderer.Constants.Star, Context.Renderer.Constants.LeftBracket);

        var addressSpan = _address!.Render();

        // *(TYPE*)(ADDRESS + SIZE)
        if (_address is BinaryOperatorStatement)
            addressSpan = Context.Renderer.Concat(Context.Renderer.Constants.RightBracket, addressSpan,
                Context.Renderer.Constants.LeftBracket);

        return Context.Renderer.Concat(castAddrSpan, addressSpan);
    }
}
