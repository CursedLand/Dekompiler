using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements;

public interface IStatement
{
    CilCode[] Codes
    {
        get;
    }

    TypeSignature Type
    {
        get;
    }

    void InterpretStack();
    void Deserialize(CilInstruction instruction);
    RenderFragment Render();
}
