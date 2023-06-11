using AsmResolver;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using AsmResolver.PE.DotNet.Metadata.Tables.Rows;
using Dekompiler.Core.Statements;
using Dekompiler.Core.Statements.Constants;
using Dekompiler.Core.Statements.Miscellaneous;
using Dekompiler.Core.Statements.TypeSystem;

namespace Dekompiler.Core;

public static class Extensions
{
    public static IStatement TryCast(this IStatement statement, TypeSignature signature)
    {
        return signature switch
        {
            { FullName: "System.Boolean" } _ when statement is LdcI4Statement value =>
                new KeywordStatement(value.GetContext(), value.Value > 0 ? "true" : "false", signature),
            TypeDefOrRefSignature { } typeDefOrRef when statement is LdcI4Statement value =>
                ConstantField(typeDefOrRef, value),
            _ => statement
        };

        IStatement ConstantField(TypeDefOrRefSignature typeDefOrRef, LdcI4Statement value)
        {
            if (typeDefOrRef.Type.Resolve() is { IsEnum: true } enumType)
            {
                // var constant = BitConverter.GetBytes(value.Value);

                var fld = enumType.Fields.FirstOrDefault(enumItem =>
                    enumItem.Constant is { Value: not null } content &&
                    GetInt(content) == value.Value);

                if (fld is { } enumField)
                {
                    var fldStatement = new LdFldStatement(value.GetContext());

                    fldStatement.Deserialize(new CilInstruction(CilOpCodes.Ldsfld, enumField));

                    return fldStatement;
                }
                else
                {
                    return statement;
                }
            }
            else
            {
                return statement;
            }
        }
    }

    private static int GetInt(Constant constant)
    {
        var valueData = constant.Value!.Data;
        return constant.Type switch
        {
            ElementType.I1 or ElementType.U1 => valueData[0],
            ElementType.I2 => BitConverter.ToInt16(valueData),
            ElementType.U2 => BitConverter.ToUInt16(valueData),
            ElementType.I4 => BitConverter.ToInt32(valueData),
            ElementType.U4 => (int)BitConverter.ToUInt32(valueData),
            ElementType.I8 => (int)BitConverter.ToInt64(valueData),
            ElementType.U8 => (int)BitConverter.ToUInt64(valueData),
            ElementType.R4 => (int)BitConverter.ToSingle(valueData),
            ElementType.R8 => (int)BitConverter.ToDouble(valueData),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}