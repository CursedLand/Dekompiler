using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.TypeSystem;

public class LdTokenStatement : PushableStatement
{
    private TypeSignature? _ldTokenType;
    private IMemberDescriptor? _member;
    private LdTok? _tokenType;

    public LdTokenStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Ldtoken
    };

    public override TypeSignature Type => _ldTokenType!;

    public override void Deserialize(CilInstruction instruction)
    {
        var member = (IMemberDescriptor)instruction.Operand!;
        var scope = Context.Method.Module!.CorLibTypeFactory.CorLibScope;

        (_ldTokenType, _tokenType) = member switch
        {
            IFieldDescriptor => (
                scope.CreateTypeReference(nameof(System), nameof(RuntimeFieldHandle)).ToTypeSignature(), LdTok.Field),
            IMethodDescriptor => (
                scope.CreateTypeReference(nameof(System), nameof(RuntimeMethodHandle)).ToTypeSignature(), LdTok.Method),
            ITypeDescriptor => (scope.CreateTypeReference(nameof(System), nameof(RuntimeTypeHandle)).ToTypeSignature(),
                LdTok.Type),
            _ => throw new ArgumentOutOfRangeException()
        };


        _member = member;
    }

    public override RenderFragment Render()
    {
        var (keyword, property) = _tokenType switch
        {
            LdTok.Field => ("fieldof", "FieldHandle"),
            LdTok.Method => ("methodof", "MethodHandle"),
            LdTok.Type => ("typeof", "TypeHandle"),
            _ => throw new ArgumentOutOfRangeException()
        };

        var memberSpan = _tokenType switch
        {
            LdTok.Field => new FieldRenderer(Context.Renderer, ((IFieldDescriptor)_member!).Resolve()!).RenderMember(),
            LdTok.Method => new MethodRenderer(Context.Renderer, ((IMethodDescriptor)_member!).Resolve()!).RenderMember(),
            LdTok.Type => new TypeRenderer(Context.Renderer, ((ITypeDescriptor)_member!).Resolve()!).RenderMember(),
            _ => throw new ArgumentOutOfRangeException()
        };

        if (_member.DeclaringType is not null)
        {
            var declaringSpan = new TypeSignatureRenderer(Context.Renderer, _member.DeclaringType.ToTypeSignature(),
                _member, _member.DeclaringType.ToTypeDefOrRef()).RenderMember();

            return Context.Renderer.Concat(Context.Renderer.Keyword(keyword), Context.Renderer.Constants.RightBracket,
                declaringSpan, Context.Renderer.Constants.Dot, memberSpan, Context.Renderer.Constants.LeftBracket,
                Context.Renderer.Constants.Dot,
                Context.Renderer.Span(property, "instanceproperty"));
        }

        return Context.Renderer.Concat(Context.Renderer.Keyword(keyword), Context.Renderer.Constants.RightBracket,
            memberSpan, Context.Renderer.Constants.LeftBracket, Context.Renderer.Constants.Dot,
            Context.Renderer.Span(property, "instanceproperty"));
    }

    internal enum LdTok
    {
        Type,
        Method,
        Field
    }
}