using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Variables;

public class LdLocStatement : PushableStatement
{
    private bool _loadAddr;
    private CilLocalVariable? _variable;

    public LdLocStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ldloc_0,
        CilCode.Ldloc_1,
        CilCode.Ldloc_2,
        CilCode.Ldloc_3,
        CilCode.Ldloc_S,
        CilCode.Ldloc,
        CilCode.Ldloca,
        CilCode.Ldloca_S
    };

    public override TypeSignature Type => _variable!.VariableType;

    public override void Deserialize(CilInstruction instruction)
    {
        _variable = (CilLocalVariable)instruction.Operand!;

        _loadAddr = instruction.OpCode.Code is CilCode.Ldloca or CilCode.Ldloca_S;
    }

    public override RenderFragment Render()
    {
        var variableSpan = Context.Renderer.Span(Context.GenerateName(_variable!), "local");

        if (_loadAddr && !Type.IsValueType)
        {
            var loadAddrSpan = Context.Renderer.Span("ldloca", "opcode");

            return Context.Renderer.Concat(loadAddrSpan, Context.Renderer.Constants.RightBracket, variableSpan,
                Context.Renderer.Constants.LeftBracket);
        }

        return variableSpan;
    }
}
