using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Prefixes;

public class PrefixStatement : Statement
{
    public PrefixStatement(MethodContext context) : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Volatile,
        CilCode.Unaligned,
        CilCode.Constrained,
        CilCode.Readonly,
        CilCode.Tailcall,
        CilCode.Prefix7,
        CilCode.Prefix6,
        CilCode.Prefix5,
        CilCode.Prefix4,
        CilCode.Prefix3,
        CilCode.Prefix2,
        CilCode.Prefix1,
        CilCode.Prefixref,
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