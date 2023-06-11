using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.TypeSystem;

public class LdFldStatement : PushableStatement
{
    private IFieldDescriptor? _field;
    private IStatement? _instance;
    private bool _loadAddr;

    public LdFldStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ldfld,
        CilCode.Ldflda,
        CilCode.Ldsfld,
        CilCode.Ldsflda
    };

    public override TypeSignature Type => _field!.Signature!.FieldType;

    public override void Deserialize(CilInstruction instruction)
    {
        var fieldRef = (IFieldDescriptor)instruction.Operand!;

        var hasInstance = instruction.OpCode.Code is CilCode.Ldfld or CilCode.Ldflda;

        if (hasInstance)
            _instance = Context.Stack.Pop();

        _field = fieldRef;
        _loadAddr = instruction.OpCode.Code is CilCode.Ldflda or CilCode.Ldsflda;
    }

    public override RenderFragment Render()
    {
        var instanceSpan = _instance is null
            ? new TypeSignatureRenderer(Context.Renderer, _field!.DeclaringType!.ToTypeSignature(), Context.Method,
                _field!.DeclaringType!.Resolve()!).RenderMember()
            : _instance.Render();

        var fieldSpan = new FieldRenderer(Context.Renderer, _field!).RenderMember();

        var ldFldSpan = Context.Renderer.Concat(instanceSpan, Context.Renderer.Constants.Dot, fieldSpan);

        if (_loadAddr)
        {
            var loadAddrSpan = Context.Renderer.Span("ld(s)flda", "opcode");

            return Context.Renderer.Concat(loadAddrSpan, Context.Renderer.Constants.RightBracket, ldFldSpan,
                Context.Renderer.Constants.LeftBracket);
        }

        return ldFldSpan;
    }
}
