using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Constants;

public class LdStrStatement : PushableStatement
{
    private string _value;

    public LdStrStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ldstr
    };

    public override TypeSignature Type => Context.Method.Module!.CorLibTypeFactory.String;

    public override void Deserialize(CilInstruction instruction)
    {
        _value = (string)instruction.Operand!;
    }

    public override RenderFragment Render()
    {
        var str = _value
            .Replace("\\", "\\\\")
            .Replace("\"", "\\\"")
            .Replace("\r", "\\r")
            .Replace("\n", "\\n");

        return Context.Renderer.Span(string.Format("\"{0}\"", str), "string");
    }
}
