using System.Text;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Declaration;

public class PropertyDeclaration : IMemberDeclaration
{
    private RendererService _renderer;
    private PropertyDefinition _property;

    public PropertyDeclaration(RendererService renderer, PropertyDefinition property)
    {
        _property = property;
        _renderer = renderer;
    }

    public RenderFragment BuildDeclaration()
    {
        return _renderer.Join(_renderer.Constants.Space, RenderAccessibility(), RenderKind(), RenderProperty());
    }
    
    private RenderFragment RenderAccessibility()
    {
        var accessibility = () =>
        {
            return (_property.SetMethod ?? _property.GetMethod) switch
            {
                { IsPublic: true } => "public",
                { IsFamily: true } => "protected",
                { IsFamilyOrAssembly: true } => "internal",
                { IsFamilyAndAssembly: true } => "internal protected",
                { IsPrivate: true } => "private",
                _ => string.Empty,
            };
        };

        var modifier = () =>
        {
            var sb = new StringBuilder();

            if ((_property.SetMethod ?? _property.GetMethod) is { IsStatic: true }) sb.Append("static ");

            return sb.ToString().TrimEnd(' ');
        };

        var accessFragment = accessibility() is { Length: not 0 } s ? _renderer.Keyword(s) : null;
        var modifierFragment = modifier() is { Length: not 0 } s2 ? _renderer.Keyword(s2) : null;

        return _renderer.Join(_renderer.Constants.Space, accessFragment, modifierFragment);
    }

    private RenderFragment RenderKind()
    {
        return new TypeSignatureRenderer(_renderer, _property.Signature!.ReturnType, _property, _property.DeclaringType).RenderMember();
    }

    private RenderFragment RenderProperty()
    {
        var propertySpan = new PropertyRenderer(_renderer, _property).RenderMember();

        // indexer
        if (_property.Signature!.ParameterTypes.Count > 0)
        {
            propertySpan = _renderer.Keyword("this");
            
            var method = _property.GetMethod!;
            var separator = _renderer.Concat(_renderer.Constants.Comma, _renderer.Constants.Space);

            var renderedParameters = new List<RenderFragment>();

            foreach (var parameter in method.Parameters)
            {
                if (parameter.MethodSignatureIndex is -1) continue;

                var parameterSpan =
                    new TypeSignatureRenderer(_renderer, parameter.ParameterType, method, method.DeclaringType!)
                        .RenderMember();

                parameterSpan = _renderer.Join(_renderer.Constants.Space, parameterSpan, _renderer.Span(parameter.Name, "parameter"));
                
                renderedParameters.Add(parameterSpan);
            }

            var argTypes = _renderer.Join(separator, renderedParameters.ToArray());

            propertySpan =
                _renderer.Concat(propertySpan, _renderer.Constants.RightArrayBracket, argTypes, _renderer.Constants.LeftArrayBracket);
        }

        return propertySpan;
    }
}