using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Miscellaneous;

public class CommentStatement : CompleteStatement
{
    private string _comment;

    public CommentStatement(MethodContext context, string comment) : base(context)
    {
        _comment = comment;
    }

    public override CilCode[] Codes => Array.Empty<CilCode>();

    public override void Deserialize(CilInstruction instruction)
    {
    }

    public override RenderFragment Render()
    {
        return Context.Renderer.Span($@"/* {_comment} */", "comment");
    }
}