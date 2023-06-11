using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Miscellaneous;

public class KeywordStatement : PushableStatement
{
    private string? _keyword;
    private TypeSignature? _type;
    
    public KeywordStatement(MethodContext context, string keyword, TypeSignature? type) : base(context)
    {
        _keyword = keyword;
        _type = type;
    }

    public override CilCode[] Codes => Array.Empty<CilCode>();
    public override TypeSignature Type => _type!;

    public override void Deserialize(CilInstruction instruction)
    {
    }

    public override RenderFragment Render()
    {
        return Context.Renderer.Keyword(_keyword);
    }
}