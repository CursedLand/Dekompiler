using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.TypeSystem;

public class NewObjStatement : PushableStatement
{
    private IStatement[]? _arguments;
    private IMethodDescriptor? _constructor;

    public NewObjStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Newobj
    };

    public override TypeSignature Type => _constructor!.DeclaringType!.ToTypeSignature();

    public override void Deserialize(CilInstruction instruction)
    {
        var method = (IMethodDescriptor)instruction.Operand!;


        var arguments = new List<IStatement>();

        var totalArguments = method.Signature!.ParameterTypes.Count;

        var ctx = GenericContext.FromMember(method);

        for (var i = totalArguments; i-- > 0;)
        {
            var t = method.Signature!.ParameterTypes[i];
            if (t is GenericParameterSignature genericParameter)
            {
                arguments.Add(Context.Stack.Pop().TryCast(ctx.GetTypeArgument(genericParameter)));
            }
            else
            {
                arguments.Add(Context.Stack.Pop().TryCast(t));
            }
        }

        arguments.Reverse();

        _constructor = method;
        _arguments = arguments.ToArray();
    }

    public override RenderFragment Render()
    {
        var methodSpan =
            new TypeSignatureRenderer(Context.Renderer, _constructor!.DeclaringType!.ToTypeSignature(), _constructor,
                _constructor.DeclaringType.Resolve()!).RenderMember();

        var argumentsSpans = _arguments!.SelectMany(arg =>
        {
            if (arg != _arguments!.Last())
                return new[] { arg.Render(), Context.Renderer.Constants.Comma, Context.Renderer.Constants.Space };
            return new[] { arg.Render() };
        });

        var temp = new List<RenderFragment>();

        temp.Add(Context.Renderer.Keyword("new"));
        temp.Add(Context.Renderer.Constants.Space);
        temp.Add(methodSpan);
        temp.Add(Context.Renderer.Constants.RightBracket);
        temp.AddRange(argumentsSpans);
        temp.Add(Context.Renderer.Constants.LeftBracket);

        return Context.Renderer.Concat(temp.ToArray());
    }
}
