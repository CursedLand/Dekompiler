using AsmResolver.DotNet;
using Dekompiler.Core.Declaration;
using Dekompiler.Core.Statements;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Results;

public class MethodDecompilationResult : IDecompilationResult
{
    private readonly MethodContext _ctx;

    public MethodDecompilationResult(Icons.IconInfo icon, RenderFragment header, IMemberDeclaration declaration,
        MethodContext ctx)
    {
        _ctx = ctx;
        Icon = icon;
        Header = header;
        Declaration = declaration;
    }

    public Icons.IconInfo Icon { get; }
    public RenderFragment Header { get; }
    public IMemberDefinition Member => _ctx.Method;
    public IMemberDeclaration Declaration { get; }

    public bool Empty => _ctx.Empty;

    public RenderFragment RenderResult()
    {
        return builder =>
        {
            builder.OpenElement(0, "p");
            builder.AddAttribute(1, "methodToken", _ctx.Method.MetadataToken.ToString());

            builder.AddContent(2, _ctx.Renderer.Concat(_ctx.Renderer.Span($"// Token: 0x{_ctx.Method.MetadataToken.ToInt32():x8} RID: {_ctx.Method.MetadataToken.Rid}" + (_ctx.Method.CilMethodBody is not null ? $" RVA: 0x{_ctx.Method.MethodBody?.Address?.Rva:x8}" : string.Empty), "comment"), IDecompilationResult.NewLine()));
            
            if (_ctx.Empty)
            {
                // null method body/multi cast delegate impl/interface 
                builder.AddContent(3,
                    _ctx.Renderer.Concat(Declaration.BuildDeclaration(), _ctx.Renderer.Constants.SemiColumn));
            }
            else
            {
                builder.AddContent(3, Declaration.BuildDeclaration());

                builder.AddContent(4, IDecompilationResult.NewLine());

                builder.AddContent(5, _ctx.Renderer.Constants.RightCurlyBracket);

                builder.AddContent(6, IDecompilationResult.NewLine());

                var sequence = 6;

                foreach (var statement in _ctx.CompleteStatements)
                {
                    builder.AddContent(sequence++,
                        IDecompilationResult.Indent(_ctx.Renderer.Concat(statement.Render(),
                            _ctx.Renderer.Constants.SemiColumn)));
                }

                builder.AddContent(sequence, _ctx.Renderer.Constants.LeftCurlyBracket);
            }

            builder.CloseElement();
        };
    }
}