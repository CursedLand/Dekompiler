using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.TypeSystem;

public class CalliStatement : Statement
{
    private MethodSignature? _signature;
    private IStatement[]? _arguments;
    private IStatement? _address;

    public CalliStatement(MethodContext context) : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Calli
    };

    public override TypeSignature Type => _signature!.ReturnType;

    public override void InterpretStack()
    {
        if (_signature!.ReturnsValue)
            Context.Stack.Push(this);
        else
            Context.CompleteStatements.Add(this);
    }

    public override void Deserialize(CilInstruction instruction)
    {
        _signature = (MethodSignature?)((StandAloneSignature)instruction.Operand!).Signature;

        var arguments = new List<IStatement>();

        var totalArguments = _signature!.ParameterTypes.Count;

        _address = Context.Stack.Pop();
        
        for (var i = 0; i < totalArguments; i++)
            arguments.Add(Context.Stack.Pop());
        
        if (_signature.HasThis)
        {
            arguments.Add(Context.Stack.Pop());
        }
        
        arguments.Reverse();

        _arguments = arguments.ToArray();
    }

    public override RenderFragment Render()
    {
        var calliSpan = Context.Renderer.Span("calli", "opcode");

        var sigSpan = Context.Renderer.Text(_signature!.ToString());

        var separator = Context.Renderer.Concat(Context.Renderer.Constants.Comma, Context.Renderer.Constants.Space);

        var arguments = new List<RenderFragment>();

        arguments.Add(sigSpan);
        arguments.AddRange(_arguments!.Select(arg => arg.Render()));
        arguments.Add(_address!.Render());

        return Context.Renderer.Concat(calliSpan, Context.Renderer.Constants.RightBracket,
            Context.Renderer.Join(separator, arguments.ToArray()), Context.Renderer.Constants.LeftBracket);
    }
}