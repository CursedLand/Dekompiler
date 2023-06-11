using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.TypeSystem;

public class CallStatement : Statement
{
    private IStatement[]? _arguments;
    private IStatement? _instance;
    private IMethodDescriptor? _method;
    private CallType _type;
    private IHasSemantics? _association;

    public CallStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Call,
        CilCode.Callvirt
    };

    public override TypeSignature Type => _method!.Signature!.ReturnType;

    public override void InterpretStack()
    {
        if (_method!.Signature!.ReturnsValue)
            Context.Stack.Push(this);
        else
            Context.CompleteStatements.Add(this);
    }

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

        if (method.Signature!.HasThis)
        {
            _instance = Context.Stack.Pop();
        }

        var resolved = method.Resolve();

        _type = resolved switch
        {
            { IsGetMethod: true } when resolved.Signature!.ParameterTypes.Count >= 1 => CallType.IndexerGetter,
            { IsSetMethod: true } when resolved.Signature!.ParameterTypes.Count >= 2 => CallType.IndexerSetter,
            { IsGetMethod: true } when resolved.Signature!.ParameterTypes.Count <= 0 => CallType.Getter,
            { IsSetMethod: true } when resolved.Signature!.ParameterTypes.Count <= 1 => CallType.Setter,
            _ => CallType.Normal
        };

        if (_type is not CallType.Normal)
        {
            _association = resolved!.Semantics!.Association;
        }

        _method = method;
        _arguments = arguments.ToArray();
    }

    public override RenderFragment Render()
    {
        var instanceSpan = _instance is not null
            ? _instance.Render()
            : new TypeSignatureRenderer(Context.Renderer, _method!.DeclaringType!.ToTypeSignature(), _method,
                _method!.DeclaringType!.ToTypeDefOrRef()).RenderMember();

        var methodSpan = _type switch
        {
            CallType.Normal => new MethodRenderer(Context.Renderer, _method!).RenderMember(),
            _ => new PropertyRenderer(Context.Renderer, (PropertyDefinition)_association!).RenderMember()
        };

        var argumentsSpans =
            Context.Renderer.Join(
                Context.Renderer.Concat(Context.Renderer.Constants.Comma, Context.Renderer.Constants.Space),
                _arguments!.Select(arg => arg.Render()).ToArray());

        return _type switch
        {
            CallType.IndexerGetter => IndexerGet(),
            CallType.IndexerSetter => IndexerSet(),
            CallType.Getter => Get(),
            CallType.Setter => Set(),
            CallType.Normal => Call(),
            _ => throw new ArgumentOutOfRangeException()
        };

        RenderFragment Call()
        {
            return Context.Renderer.Concat(instanceSpan, Context.Renderer.Constants.Dot, methodSpan,
                Context.Renderer.Constants.RightBracket, argumentsSpans, Context.Renderer.Constants.LeftBracket);
        }

        RenderFragment IndexerGet()
        {
            return Context.Renderer.Concat(instanceSpan,
                Context.Renderer.Constants.RightArrayBracket, argumentsSpans,
                Context.Renderer.Constants.LeftArrayBracket);
        }

        RenderFragment IndexerSet()
        {
            var value = _arguments!.Last().Render();

            argumentsSpans =
                Context.Renderer.Join(
                    Context.Renderer.Concat(Context.Renderer.Constants.Comma, Context.Renderer.Constants.Space),
                    _arguments!.SkipLast(1).Select(arg => arg.Render()).ToArray());

            return Context.Renderer.Concat(instanceSpan,
                Context.Renderer.Constants.RightArrayBracket, argumentsSpans,
                Context.Renderer.Constants.LeftArrayBracket, Context.Renderer.Constants.Space,
                Context.Renderer.Constants.Equal, Context.Renderer.Constants.Space, value);
        }

        RenderFragment Get()
        {
            return Context.Renderer.Concat(instanceSpan, Context.Renderer.Constants.Dot, methodSpan);
        }

        RenderFragment Set()
        {
            return Context.Renderer.Concat(instanceSpan, Context.Renderer.Constants.Dot, methodSpan,
                Context.Renderer.Constants.Space, Context.Renderer.Constants.Equal, Context.Renderer.Constants.Space,
                _arguments!.Last().Render());
        }
    }

    // TODO: EventAdd, EventFire, EventRemove
    internal enum CallType
    {
        IndexerGetter,
        IndexerSetter,
        Getter,
        Setter,
        Normal
    }
}