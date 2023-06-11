using AsmResolver.DotNet.Code.Cil;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Core.Statements.Arguments;
using Dekompiler.Core.Statements.Variables;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Miscellaneous;

public class DupStatement : Statement
{
    private bool _createVariable = true;
    private IStatement? _value;

    public DupStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Dup
    };

    public override TypeSignature Type => _value!.Type;

    public override void InterpretStack()
    {
        if (_createVariable)
        {
            var variable = new CilLocalVariable(Type);

            Context.InitializedVariable[variable] = false;

            var stInstruction = new CilInstruction(CilOpCodes.Stloc, variable);
            var stLoc = new StLocStatement(Context);

            Context.Stack.Push(_value!);

            stLoc.Deserialize(stInstruction);
            stLoc.InterpretStack();

            var ldInstruction = new CilInstruction(CilOpCodes.Ldloc, variable);
            var ldLoc = new LdLocStatement(Context);

            ldLoc.Deserialize(ldInstruction);
            ldLoc.InterpretStack();
            ldLoc.InterpretStack();

            return;
        }

        Context.Stack.Push(_value!);
        Context.Stack.Push(_value!);
    }

    public override void Deserialize(CilInstruction instruction)
    {
        var value = Context.Stack.Pop();

        if (value is LdArgStatement or LdLocStatement)
            _createVariable = false;

        _value = value;
    }

    public override RenderFragment Render()
    {
        throw new NotImplementedException();
    }
}
