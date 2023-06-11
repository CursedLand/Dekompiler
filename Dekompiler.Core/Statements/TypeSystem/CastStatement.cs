using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Core.Statements.Operators;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.TypeSystem;

public class CastStatement : PushableStatement
{
    private IStatement? _value;
    private TypeSignature? _castType;
    
    public CastStatement(MethodContext context) : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Box,
        CilCode.Castclass,
        CilCode.Unbox,
        CilCode.Unbox_Any
    };

    public override TypeSignature Type => _castType!;

    public override void Deserialize(CilInstruction instruction)
    {
        _value = Context.Stack.Pop();

        var type = ((ITypeDefOrRef)instruction.Operand!).ToTypeSignature();

        _castType = type;
    }

    public override RenderFragment Render()
    {
        var valueSpan = _value!.Render();

        // redundant cast
        if (_value.Type == _castType)
        {
            return valueSpan;
        }
        
        var convSpan =
            new TypeSignatureRenderer(Context.Renderer, _castType!, Context.Method, Context.Method.DeclaringType)
                .RenderMember();

        return Context.Renderer.Concat(Context.Renderer.Constants.RightBracket, convSpan,
            Context.Renderer.Constants.LeftBracket, valueSpan);
    }
}