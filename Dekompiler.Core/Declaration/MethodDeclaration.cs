using System.Text;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Dekompiler.Core.Declaration;

public class MethodDeclaration : IMemberDeclaration
{

    private RendererService _renderer;
    private MethodDefinition _method;
    
    public MethodDeclaration(RendererService renderer, MethodDefinition method)
    {
        _renderer = renderer;
        _method = method;
    }
    
    public RenderFragment BuildDeclaration()
    {
        return _renderer.Join(_renderer.Constants.Space, RenderAccessibility(), _method.IsConstructor ? null : RenderType(), RenderMethod());
    }

    private RenderFragment RenderAccessibility()
    {
        var accessibility = () =>
        {
            return _method switch
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

            if (_method is { IsStatic: true }) sb.Append("static ");

            return sb.ToString().TrimEnd(' ');
        };

        var accessFragment = accessibility() is { Length: not 0 } s ? _renderer.Keyword(s) : null;
        var modifierFragment = modifier() is { Length: not 0 } s2 ? _renderer.Keyword(s2) : null;

        return _renderer.Join(_renderer.Constants.Space, accessFragment, modifierFragment);
    }
    
    private RenderFragment RenderType()
    {
        return new TypeSignatureRenderer(_renderer, _method.Signature!.ReturnType, _method, _method.DeclaringType).RenderMember();
    }

    private RenderFragment RenderMethod()
    {
        var methodSpan = new MethodRenderer(_renderer, _method).RenderMember();

        if (_method.IsConstructor)
        {
            methodSpan = RenderType(_method.DeclaringType!);
        }

        var parameterSpan = _renderer.Concat(_renderer.Constants.RightBracket, _renderer.Constants.LeftBracket);

        if (_method.Parameters.Count > 0)
        {
            parameterSpan = RenderMethodParameters(_method);
        }
        
        if (_method.GenericParameters.Count > 0)
        {
            var separator = _renderer.Concat(_renderer.Constants.Comma, _renderer.Constants.Space);

            var genericArgs = _renderer.Join(separator,
                _method.GenericParameters.Select(arg => _renderer.Span(arg.Name, "genericparameter")).ToArray());

            parameterSpan = _renderer.Concat(_renderer.Concat(_renderer.Constants.RightThan, genericArgs,
                _renderer.Constants.LeftThan), parameterSpan);
        }

        return _renderer.Concat(methodSpan, parameterSpan);
    }

    private RenderFragment RenderMethodParameters(MethodDefinition method)
    {
        var separator = _renderer.Concat(_renderer.Constants.Comma, _renderer.Constants.Space);

        var renderedParameters = new List<RenderFragment>();

        foreach (var parameter in method.Parameters)
        {
            if (parameter.MethodSignatureIndex is -1) continue;

            var getUnderlyingSignature = parameter.Definition is { IsOut: true } || parameter.Definition is { IsIn: true };
            var isRef = parameter.ParameterType is ByReferenceTypeSignature && !getUnderlyingSignature;
            

            var parameterSpan =
                new TypeSignatureRenderer(_renderer,
                        (getUnderlyingSignature || isRef) && parameter.ParameterType is TypeSpecificationSignature spec
                            ? spec.BaseType
                            : parameter.ParameterType, method, method.DeclaringType!)
                    .RenderMember();

            parameterSpan = _renderer.Join(_renderer.Constants.Space, parameterSpan, _renderer.Span(parameter.Name, "parameter"));

            if (isRef)
                parameterSpan = _renderer.Join(_renderer.Constants.Space, _renderer.Keyword("ref"), parameterSpan);
            
            if (parameter.Definition is { IsOut: true })
                parameterSpan = _renderer.Join(_renderer.Constants.Space, _renderer.Keyword("out"), parameterSpan);

            if (parameter.Definition is { IsIn: true })
                parameterSpan = _renderer.Join(_renderer.Constants.Space, _renderer.Keyword("in"), parameterSpan);

            renderedParameters.Add(parameterSpan);
        }

        var argTypes = _renderer.Join(separator, renderedParameters.ToArray());

        return _renderer.Concat(_renderer.Constants.RightBracket, argTypes, _renderer.Constants.LeftBracket);
    }
    
    private RenderFragment RenderType(TypeDefinition type)
    {
        var typeSpan = new TypeRenderer(_renderer, type).RenderMember();

        if (type.GenericParameters.Count > 0)
        {
            var separator = _renderer.Concat(_renderer.Constants.Comma, _renderer.Constants.Space);

            var genericArgs = _renderer.Join(separator,
                type.GenericParameters.Select(arg => _renderer.Span(arg.Name, "genericparameter")).ToArray());

            return _renderer.Concat(typeSpan, _renderer.Constants.RightThan, genericArgs,
                _renderer.Constants.LeftThan);
        }

        return typeSpan;
    }
}

