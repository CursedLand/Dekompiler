using System.Text;
using AsmResolver.DotNet;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;
using Dekompiler.Core.Statements.Miscellaneous;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Declaration;

public class FieldDeclaration : IMemberDeclaration
{
    private RendererService _renderer;
    private FieldDefinition _field;

    public FieldDeclaration(RendererService renderer, FieldDefinition field)
    {
        _field = field;
        _renderer = renderer;
    }

    public RenderFragment BuildDeclaration()
    {
        if (_field is { DeclaringType: { BaseType: { FullName: "System.Enum" } } })
        {
            return _renderer.Concat(RenderField(), _renderer.Constants.Comma);
        }

        return _renderer.Join(_renderer.Constants.Space, RenderAccessibility(), RenderKind(), RenderField());
    }

    private RenderFragment RenderAccessibility()
    {
        var accessibility = () =>
        {
            return _field switch
            {
                { IsPublic: true } => "public",
                { IsFamily: true } => "protected",
                { IsFamilyOrAssembly: true } => "internal",
                { IsFamilyAndAssembly: true } => "internal protected",
                { IsPrivate: true, IsPrivateScope: true } => $"private protected",
                { IsPrivate: true } => "private",
                _ => string.Empty,
            };
        };

        var modifier = () =>
        {
            var sb = new StringBuilder();

            if (_field.IsStatic) sb.Append("static ");

            if (_field.IsInitOnly) sb.Append("readonly ");

            if (_field.Constant is { }) sb.Append("const ");

            return sb.ToString().TrimEnd(' ');
        };

        var accessFragment = accessibility() is { Length: not 0 } s ? _renderer.Keyword(s) : null;
        var modifierFragment = modifier() is { Length: not 0 } s2 ? _renderer.Keyword(s2) : null;

        return _renderer.Join(_renderer.Constants.Space, accessFragment, modifierFragment);
    }


    private RenderFragment RenderKind()
    {
        return new TypeSignatureRenderer(_renderer, _field.Signature!.FieldType, _field, _field.DeclaringType)
            .RenderMember();
    }

    private RenderFragment RenderField()
    {
        RenderFragment? constValue = null;

        if (_field.Constant is { Value: not null } constant)
        {
            var buffer = constant.Value.Data;
            constValue = constant.Type switch
            {
                ElementType.Boolean => _renderer.Keyword(BitConverter.ToBoolean(buffer, 0).ToString().ToLower()),
                ElementType.Char => _renderer.Span($"'{BitConverter.ToChar(buffer, 0)}'", "string"),
                
                ElementType.I1 => _renderer.Span(buffer[0].ToString(), "number"),
                ElementType.I2 => _renderer.Span(BitConverter.ToInt16(buffer, 0).ToString(), "number"),
                ElementType.I4 => _renderer.Span(BitConverter.ToInt32(buffer, 0).ToString(), "number"),
                ElementType.I8 => _renderer.Span(BitConverter.ToInt64(buffer, 0).ToString(), "number"),
                
                ElementType.U1 => _renderer.Span($"{buffer[0].ToString()}U", "number"),
                ElementType.U2 => _renderer.Span($"{BitConverter.ToUInt16(buffer, 0).ToString()}U", "number"),
                ElementType.U4 => _renderer.Span($"{BitConverter.ToUInt32(buffer, 0).ToString()}U", "number"),
                ElementType.U8 => _renderer.Span($"{BitConverter.ToUInt64(buffer, 0).ToString()}UL", "number"),
                
                ElementType.R4 => _renderer.Span($"{BitConverter.ToSingle(buffer, 0):F}", "number"),
                ElementType.R8 => _renderer.Span($"{BitConverter.ToDouble(buffer, 0):G}", "number"),
                
                ElementType.String => _renderer.Span($"\"{Encoding.UTF8.GetString(buffer)}\"", "string"),
                _ => null
            };

            constValue = _renderer.Concat(_renderer.Constants.Space, _renderer.Constants.Equal,
                _renderer.Constants.Space, constValue);
        }

        return _renderer.Concat(new FieldRenderer(_renderer, _field).RenderMember(), constValue);
    }
}