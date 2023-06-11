using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements;

public abstract class Statement : IStatement
{
    protected Statement(MethodContext context)
    {
        Context = context;
    }

    protected MethodContext Context
    {
        get;
    }

    public abstract CilCode[] Codes
    {
        get;
    }

    public abstract TypeSignature Type
    {
        get;
    }

    public abstract void InterpretStack();
    public abstract void Deserialize(CilInstruction instruction);
    public abstract RenderFragment Render();

    public virtual MethodContext GetContext() => Context;
}
