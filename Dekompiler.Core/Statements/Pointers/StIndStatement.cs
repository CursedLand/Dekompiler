using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Core.Statements.Operators;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Pointers;

public class StIndStatement : CompleteStatement
{
    private IStatement? _address;
    private TypeSignature? _stIndType;
    private IStatement? _value;

    public StIndStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Stind_I1,
        CilCode.Stind_I2,
        CilCode.Stind_I4,
        CilCode.Stind_I8,
        CilCode.Stind_I,
        CilCode.Stind_R4,
        CilCode.Stind_R8,
        CilCode.Stind_Ref,
        CilCode.Stobj
    };

    public override void Deserialize(CilInstruction instruction)
    {
        var corLib = Context.Method.Module!.CorLibTypeFactory;
        _value = Context.Stack.Pop();
        _address = Context.Stack.Pop();

        _stIndType = instruction.OpCode.Code switch
        {
            CilCode.Stind_I1 => corLib.SByte,
            CilCode.Stind_I2 => corLib.Int16,
            CilCode.Stind_I4 => corLib.Int32,
            CilCode.Stind_I8 => corLib.Int64,
            CilCode.Stind_I => corLib.IntPtr,
            CilCode.Stind_R4 => corLib.Single,
            CilCode.Stind_R8 => corLib.Double,
            CilCode.Stind_Ref => corLib.Object,
            CilCode.Stobj => ((ITypeDefOrRef)instruction.Operand!).ToTypeSignature(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public override RenderFragment Render()
    {
        var castAddrSpan = Context.Renderer.Concat(Context.Renderer.Constants.Star,
            Context.Renderer.Constants.RightBracket,
            new TypeSignatureRenderer(Context.Renderer, _stIndType!, Context.Method, Context.Method.DeclaringType)
                .RenderMember(),
            Context.Renderer.Constants.Star, Context.Renderer.Constants.LeftBracket);

        var addressSpan = _address!.Render();
        var valueSpan = _value!.Render();

        // *(TYPE*)(ADDRESS + SIZE) = ...
        if (_address is BinaryOperatorStatement)
            addressSpan = Context.Renderer.Concat(Context.Renderer.Constants.RightBracket, addressSpan,
                Context.Renderer.Constants.LeftBracket);

        return Context.Renderer.Concat(castAddrSpan, addressSpan, Context.Renderer.Constants.Space,
            Context.Renderer.Constants.Equal, Context.Renderer.Constants.Space, valueSpan);
    }
}
