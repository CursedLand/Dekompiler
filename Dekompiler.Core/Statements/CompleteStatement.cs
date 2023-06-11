using AsmResolver.DotNet.Signatures.Types;

namespace Dekompiler.Core.Statements;

public abstract class CompleteStatement : Statement
{
    protected CompleteStatement(MethodContext context)
        : base(context)
    {
    }

    public override TypeSignature Type => null!;

    public override void InterpretStack()
    {
        Context.CompleteStatements.Add(this);
    }
}
