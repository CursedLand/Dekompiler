using AsmResolver.PE.DotNet.Cil;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.Arrays;

public class StElemStatement : CompleteStatement
{
    private IStatement? _index;
    private IStatement? _instance;
    private IStatement? _value;

    public StElemStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Stelem_I,
        CilCode.Stelem_I1,
        CilCode.Stelem_I2,
        CilCode.Stelem_I4,
        CilCode.Stelem_I8,
        CilCode.Stelem_R4,
        CilCode.Stelem_R8,
        CilCode.Stelem_Ref,
        CilCode.Stelem
    };

    public override void Deserialize(CilInstruction instruction)
    {
        _value = Context.Stack.Pop();
        _index = Context.Stack.Pop();
        _instance = Context.Stack.Pop();

        _value = _value.TryCast(_instance.Type.GetUnderlyingTypeDefOrRef().ToTypeSignature());
    }

    public override RenderFragment Render()
    {
        var instanceSpan = _instance!.Render();
        var indexSpan = _index!.Render();
        var valueSpan = _value!.Render();

        return Context.Renderer.Concat(instanceSpan, Context.Renderer.Constants.RightArrayBracket, indexSpan,
            Context.Renderer.Constants.LeftArrayBracket, Context.Renderer.Constants.Space,
            Context.Renderer.Constants.Equal, Context.Renderer.Constants.Space, valueSpan);
    }
}
