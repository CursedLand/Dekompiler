using System.Text;
using AsmResolver.DotNet;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Declaration;

public class TypeDeclaration : IMemberDeclaration
{
    private RendererService _renderer;
    private TypeDefinition _type;

    public TypeDeclaration(RendererService renderer, TypeDefinition type)
    {
        _type = type;
        _renderer = renderer;
    }

    public RenderFragment BuildDeclaration()
    {
        return _renderer.Join(_renderer.Constants.Space, RenderAccessibility(), RenderKind(), RenderType(),
            RenderInheritance());
    }

    private RenderFragment RenderInheritance()
    {
        var temp = new List<ITypeDefOrRef>();
        if (_type.BaseType is { FullName: not "System.Object", FullName: not "System.ValueType", FullName: not "System.Enum"  })
        {
            temp.Add(_type.BaseType);
        }
        
        temp.AddRange(_type.Interfaces.Select(i => i.Interface!));

        var separator = _renderer.Concat(_renderer.Constants.Comma, _renderer.Constants.Space);

        var interfaces = _renderer.Join(separator, temp.Select(t => new TypeRenderer(_renderer, t).RenderMember()).ToArray());

        if (temp.Count > 0)
        {
            return _renderer.Concat(_renderer.Constants.Column, _renderer.Constants.Space, interfaces);
        }

        return null!;
    }

    private RenderFragment RenderAccessibility()
    {
        var accessibility = () =>
        {
            return _type switch
            {
                { IsPublic: true } => "public",
                { IsNestedFamily: true } => "protected",
                { IsNestedFamilyOrAssembly: true } => "internal",
                { IsNestedFamilyAndAssembly: true } => "internal protected",
                { IsNestedPrivate: true } => "private",
                _ => string.Empty,
            };
        };

        var modifier = () =>
        {
            var sb = new StringBuilder();

            if (_type is { IsSealed: true, IsAbstract: false, IsEnum: false, IsDelegate: false, IsValueType: false }) sb.Append("sealed ");

            if (_type is { IsSealed: true, IsAbstract: true }) sb.Append("static ");

            return sb.ToString().TrimEnd(' ');
        };

        var accessFragment = accessibility() is { Length: not 0 } s ? _renderer.Keyword(s) : null;
        var modifierFragment = modifier() is { Length: not 0 } s2 ? _renderer.Keyword(s2) : null;

        return _renderer.Join(_renderer.Constants.Space, accessFragment, modifierFragment);
    }

    private RenderFragment RenderKind()
    {
        var typeKind = _type switch
        {
            { IsInterface: true } => "interface",
            { IsDelegate: true } => "delegate",
            { IsEnum: true } => "enum",
            { IsValueType: true } => "struct",
            _ => "class"
        };

        return _renderer.Keyword(typeKind);
    }

    private RenderFragment RenderType()
    {
        var typeSpan = new TypeRenderer(_renderer, _type).RenderMember();

        if (_type.GenericParameters.Count > 0)
        {
            var separator = _renderer.Concat(_renderer.Constants.Comma, _renderer.Constants.Space);

            var genericArgs = _renderer.Join(separator,
                _type.GenericParameters.Select(arg => _renderer.Span(arg.Name, "genericparameter")).ToArray());

            return _renderer.Concat(typeSpan, _renderer.Constants.RightThan, genericArgs,
                _renderer.Constants.LeftThan);
        }

        return typeSpan;
    }
}