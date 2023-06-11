namespace Dekompiler.Core.Statements;

public abstract class PushableStatement : Statement
{
    protected PushableStatement(MethodContext context)
        : base(context)
    {
    }

    public override void InterpretStack()
    {
        Context.Stack.Push(this);
    }
}
