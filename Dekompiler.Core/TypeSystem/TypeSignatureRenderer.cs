using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures;
using AsmResolver.DotNet.Signatures.Types;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.TypeSystem;

public class TypeSignatureRenderer
{
    private readonly ITypeDefOrRef? _declaringType;
    private readonly IMemberDescriptor _member;
    private readonly RendererService _service;
    private TypeSignature _signature;
    private bool _declare;

    public TypeSignatureRenderer(RendererService service, TypeSignature signature, IMemberDescriptor member,
        ITypeDefOrRef? declaringType = null, bool declare = true)
    {
        _service = service;
        _member = member;
        _declaringType = declaringType;
        _signature = signature;
        _declare = declare;
    }

    public RenderFragment RenderMember()
    {
        var renderedSignature = _signature switch
        {
            PointerTypeSignature pointerType => RenderPointerTypeSignature(pointerType),
            CorLibTypeSignature corLibType => RenderCorLibTypeSignature(corLibType),
            ArrayTypeSignature arrayType => RenderArrayTypeSignature(arrayType),
            BoxedTypeSignature boxedType => RenderBoxedTypeSignature(boxedType),
            ByReferenceTypeSignature byReferenceType => RenderByReferenceTypeSignature(byReferenceType),
            SzArrayTypeSignature szArrayType => RenderSzArrayTypeSignature(szArrayType),
            GenericInstanceTypeSignature genericInstanceType => RenderGenericInstanceTypeSignature(genericInstanceType),
            GenericParameterSignature genericParameter => RenderGenericParameterSignature(genericParameter),
            TypeDefOrRefSignature typeDefOrRef => RenderTypeDefOrRefSignature(typeDefOrRef),
            TypeSpecificationSignature typeSpecification => RenderTypeSpecificationSignature(typeSpecification),
            /*
                PinnedTypeSignature pinnedType => ...
                ArrayBaseTypeSignature arrayBaseType => ...
                CustomModifierTypeSignature customModifierType => ...
                FunctionPointerTypeSignature functionPointerType => ...,
                SentinelTypeSignature sentinelType => ...,
            */
            _ => throw new ArgumentOutOfRangeException(nameof(_signature))
        };

        // nested signatures.
        if (_signature.DeclaringType is not null && _declare)
        {
            renderedSignature =
                _service.Concat(
                    new TypeSignatureRenderer(_service, _signature.DeclaringType.ToTypeSignature(), _member,
                        _declaringType).RenderMember(), _service.Constants.Dot, renderedSignature);
        }

        return renderedSignature;
    }


    private RenderFragment RenderBoxedTypeSignature(BoxedTypeSignature boxedType)
    {
        throw new NotImplementedException();
    }

    private RenderFragment RenderByReferenceTypeSignature(ByReferenceTypeSignature byReferenceType)
    {
        var baseType = new TypeSignatureRenderer(_service, byReferenceType.BaseType, _member, _declaringType, false)
            .RenderMember();

        return _service.Concat(baseType, _service.Text("&"));
    }

    private RenderFragment RenderArrayTypeSignature(ArrayTypeSignature arrayType)
    {
        throw new NotImplementedException();
    }

    private RenderFragment RenderTypeSpecificationSignature(TypeSpecificationSignature typeSpecification)
    {
        var baseType = new TypeSignatureRenderer(_service, typeSpecification.BaseType, _member, _declaringType, false)
            .RenderMember();
        return baseType;
    }

    private RenderFragment RenderSzArrayTypeSignature(SzArrayTypeSignature szArrayType)
    {
        var baseType = new TypeSignatureRenderer(_service, szArrayType.BaseType, _member, _declaringType, false)
            .RenderMember();

        return _service.Concat(baseType, _service.Text("[]"));
    }

    private RenderFragment RenderCorLibTypeSignature(CorLibTypeSignature corLibType)
    {
        if (corLibType.FullName is "System.TypedReference" or "System.IntPtr" or "System.UIntPtr")
        {
            return new TypeRenderer(_service, corLibType.ToTypeDefOrRef()).RenderMember();
        }

        var keyword = corLibType.FullName switch
        {
            "System.Boolean" => "bool",
            "System.Byte" => "byte",
            "System.SByte" => "sbyte",
            "System.Char" => "char",
            "System.Decimal" => "decimal",
            "System.Double" => "double",
            "System.Single" => "float",
            "System.Int32" => "int",
            "System.UInt32" => "uint",
            "System.Int64" => "long",
            "System.UInt64" => "ulong",
            "System.Int16" => "short",
            "System.UInt16" => "ushort",
            "System.String" => "string",
            "System.Object" => "object",
            "System.Void" => "void",
            _ => throw new ArgumentOutOfRangeException()
        };

        return _service.Keyword(keyword);
    }

    private RenderFragment RenderTypeDefOrRefSignature(TypeDefOrRefSignature typeDefOrRef)
    {
        var memberSpan = new TypeRenderer(_service, typeDefOrRef.Resolve()!)
            .RenderMember();

        return memberSpan;
    }


    private RenderFragment RenderGenericParameterSignature(GenericParameterSignature genericParameter)
    {

        var name = genericParameter.ParameterType switch
        {
            GenericParameterType.Type => (_declaringType is null
                ? (TypeDefinition)_member.Resolve()!
                : _declaringType.Resolve())!.GenericParameters[genericParameter.Index].Name,

            GenericParameterType.Method => ((MethodDefinition)_member.Resolve()!)
                .GenericParameters[genericParameter.Index].Name,

            _ => throw new ArgumentOutOfRangeException()
        };

        return _service.Span(name, "genericparameter");
    }

    private RenderFragment RenderGenericInstanceTypeSignature(GenericInstanceTypeSignature genericInstanceType)
    {
        var genericType = new TypeRenderer(_service, genericInstanceType.GenericType).RenderMember();

        var separator = _service.Concat(_service.Constants.Comma, _service.Constants.Space);

        var genericArgs = _service.Join(separator,
            genericInstanceType.TypeArguments
                .Select(arg => new TypeSignatureRenderer(_service, arg, _member, _declaringType).RenderMember())
                .ToArray());

        return _service.Concat(genericType, _service.Constants.RightThan, genericArgs, _service.Constants.LeftThan);
    }

    private RenderFragment RenderPointerTypeSignature(PointerTypeSignature pointerType)
    {
        var baseType = new TypeSignatureRenderer(_service, pointerType.BaseType, _member, _declaringType, false)
            .RenderMember();

        return _service.Concat(baseType, _service.Constants.Star);
    }
}