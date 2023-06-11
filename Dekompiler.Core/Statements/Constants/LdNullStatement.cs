using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Constants;

public class LdNullStatement : PushableStatement
{
    public LdNullStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ldnull
    };

    public override TypeSignature Type => null!;

    public override void Deserialize(CilInstruction instruction)
    {
    }

    public override RenderFragment Render()
    {
        return Context.Renderer.Keyword("null");
    }
}
