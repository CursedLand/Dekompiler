using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Miscellaneous;

public class NopStatement : Statement
{
    public NopStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Nop
    };

    public override TypeSignature Type => null!;

    public override void InterpretStack()
    {
    }

    public override void Deserialize(CilInstruction instruction)
    {
    }

    public override RenderFragment Render()
    {
        throw new NotImplementedException();
    }
}
