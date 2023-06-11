using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Statements.TypeSystem;

public class StFldStatement : CompleteStatement
{
    private IFieldDescriptor? _field;
    private IStatement? _instance;
    private IStatement? _value;

    public StFldStatement(MethodContext context)
        : base(context)
    {
    }

    public override CilCode[] Codes => new[]
    {
        CilCode.Stfld,
        CilCode.Stsfld
    };

    public override void Deserialize(CilInstruction instruction)
    {
        var fieldRef = (IFieldDescriptor)instruction.Operand!;
        var value = Context.Stack.Pop().TryCast(fieldRef.Signature!.FieldType);

        var hasInstance = instruction.OpCode.Code is CilCode.Stfld;

        if (hasInstance)
            _instance = Context.Stack.Pop();

        _value = value;
        _field = fieldRef;
    }

    public override RenderFragment Render()
    {
        var valueSpan = _value!.Render();

        var instanceSpan = _instance is null
            ? new TypeSignatureRenderer(Context.Renderer, ((ITypeDefOrRef)_field?.DeclaringType!).ToTypeSignature(),
                Context.Method, Context.Method.DeclaringType!).RenderMember()
            : _instance.Render();

        var fieldSpan = new FieldRenderer(Context.Renderer, _field!).RenderMember();

        var fldSpan = Context.Renderer.Concat(instanceSpan, Context.Renderer.Constants.Dot, fieldSpan);


        return Context.Renderer.Concat(fldSpan, Context.Renderer.Constants.Space, Context.Renderer.Constants.Equal,
            Context.Renderer.Constants.Space, valueSpan);
    }
}
