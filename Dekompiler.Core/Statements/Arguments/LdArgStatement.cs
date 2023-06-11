using AsmResolver.DotNet.Collections;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Arguments;

public class LdArgStatement : PushableStatement
{
    private bool _loadAddr;
    private Parameter? _parameter;

    public LdArgStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ldarg,
        CilCode.Ldarg_0,
        CilCode.Ldarg_1,
        CilCode.Ldarg_2,
        CilCode.Ldarg_3,
        CilCode.Ldarg_S,
        CilCode.Ldarga,
        CilCode.Ldarga_S
    };

    public override TypeSignature Type => _parameter!.ParameterType;

    public override void Deserialize(CilInstruction instruction)
    {
        _parameter = (Parameter)instruction.Operand!;

        _loadAddr = instruction.OpCode.Code is CilCode.Ldarga or CilCode.Ldarga_S;
    }

    public override RenderFragment Render()
    {
        var parameterSpan = _parameter!.Index is -1
            ? Context.Renderer.Keyword("this")
            : Context.Renderer.Span(_parameter!.Name, "parameter");

        if (_loadAddr)
        {
            var loadAddrSpan = Context.Renderer.Span("ldarga", "opcode");

            return Context.Renderer.Concat(loadAddrSpan, Context.Renderer.Constants.RightBracket, parameterSpan,
                Context.Renderer.Constants.LeftBracket);
        }

        return parameterSpan;
    }
}
