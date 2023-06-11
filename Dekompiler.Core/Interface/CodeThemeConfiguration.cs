// ReSharper disable StringLiteralTypo

namespace Dekompiler.Core.Interface;

public class CodeThemeConfiguration
{
    // https://github.com/dnSpy/dnSpy/blob/master/dnSpy/dnSpy/Themes/light.dntheme
    public static readonly CodeThemeConfiguration Default = new()
    {
        Colors = new Dictionary<string, CodeThemeColor>
        {
            { "defaulttext", new CodeThemeColor("#212121") },
            { "operator", new CodeThemeColor("#000000") },
            { "punctuation", new CodeThemeColor("#000000") },
            { "number", new CodeThemeColor("#5B2DA8") },
            { "comment", new CodeThemeColor("#008000") },
            { "keyword", new CodeThemeColor("#0000FF") },
            { "string", new CodeThemeColor("#A31515") },
            { "verbatimstring", new CodeThemeColor("#800000") },
            { "char", new CodeThemeColor("#A31515") },
            { "namespace", new CodeThemeColor("#9E7E00") },
            { "type", new CodeThemeColor("#2B91AF") },
            { "sealedtype", new CodeThemeColor("#2B91AF") },
            { "statictype", new CodeThemeColor("#1B5D70") },
            { "delegate", new CodeThemeColor("#6666FF") },
            { "enum", new CodeThemeColor("#336600") },
            { "interface", new CodeThemeColor("#1E667A") },
            { "valuetype", new CodeThemeColor("#009933") },
            { "module", new CodeThemeColor("#1B5D70") },
            { "genericparameter", new CodeThemeColor("#2B91AF") },
            { "instancemethod", new CodeThemeColor("#880000") },
            { "staticmethod", new CodeThemeColor("#880000") },
            { "extensionmethod", new CodeThemeColor("#960000", italic: true) },
            { "instancefield", new CodeThemeColor("#CC3399") },
            { "enumfield", new CodeThemeColor("#6F008A") },
            { "literalfield", new CodeThemeColor("#9900FF") },
            { "staticfield", new CodeThemeColor("#990099") },
            { "instanceevent", new CodeThemeColor("#990033") },
            { "staticevent", new CodeThemeColor("#660033") },
            { "instanceproperty", new CodeThemeColor("#996633") },
            { "staticproperty", new CodeThemeColor("#7A5229") },
            { "local", new CodeThemeColor("#000000") },
            { "parameter", new CodeThemeColor("#000000", bold: true) },
            { "preprocessorkeyword", new CodeThemeColor("#FF808080") },
            { "preprocessortext", new CodeThemeColor("#FF000000") },
            { "label", new CodeThemeColor("#663300") },
            { "opcode", new CodeThemeColor("#993366") },
            { "ildirective", new CodeThemeColor("#009900") },
            { "ilmodule", new CodeThemeColor("#6936AA") },
            { "excludedcode", new CodeThemeColor("#808080") }
        }
    };

    // https://github.com/dnSpy/dnSpy/blob/master/dnSpy/dnSpy/Themes/dark.dntheme
    public static readonly CodeThemeConfiguration Dark = new()
    {
        Colors = new Dictionary<string, CodeThemeColor>
        {
            { "defaulttext", new CodeThemeColor("#DCDCDC") },
            { "operator", new CodeThemeColor("#B4B4B4") },
            { "punctuation", new CodeThemeColor("#DCDCDC") },
            { "number", new CodeThemeColor("#B5CEA8") },
            { "comment", new CodeThemeColor("#57A64A") },
            { "keyword", new CodeThemeColor("#569CD6") },
            { "string", new CodeThemeColor("#D69D85") },
            { "verbatimstring", new CodeThemeColor("#D69D85") },
            { "char", new CodeThemeColor("#D69D85") },
            { "namespace", new CodeThemeColor("#FFD700") },
            { "type", new CodeThemeColor("#4EC9B0") },
            { "sealedtype", new CodeThemeColor("#4EC9B0") },
            { "statictype", new CodeThemeColor("#378D7B") },
            { "delegate", new CodeThemeColor("#33CCFF") },
            { "enum", new CodeThemeColor("#B8D7A3") },
            { "interface", new CodeThemeColor("#9B9B82") },
            { "valuetype", new CodeThemeColor("#009933") },
            { "module", new CodeThemeColor("#378D7B") },
            { "genericparameter", new CodeThemeColor("#4B8595") },
            { "instancemethod", new CodeThemeColor("#FF8000") },
            { "staticmethod", new CodeThemeColor("#E67300") },
            { "extensionmethod", new CodeThemeColor("#CC6600", italic: true) },
            { "instancefield", new CodeThemeColor("#AA70FF") },
            { "enumfield", new CodeThemeColor("#BD63C5") },
            { "literalfield", new CodeThemeColor("#C266FF") },
            { "staticfield", new CodeThemeColor("#8D8DC6") },
            { "instanceevent", new CodeThemeColor("#CC6699") },
            { "staticevent", new CodeThemeColor("#DB94B8") },
            { "instanceproperty", new CodeThemeColor("#248F8F") },
            { "staticproperty", new CodeThemeColor("#1F8E8E") },
            { "local", new CodeThemeColor("#DCDCDC") },
            { "parameter", new CodeThemeColor("#DCDCDC", bold: true) },
            { "preprocessorkeyword", new CodeThemeColor("#FF9B9B9B") },
            { "preprocessortext", new CodeThemeColor("#FFDCDCDC") },
            { "label", new CodeThemeColor("#806F4D") },
            { "opcode", new CodeThemeColor("#AD5C85") },
            { "ildirective", new CodeThemeColor("#9999FF") },
            { "ilmodule", new CodeThemeColor("#666699") },
            { "excludedcode", new CodeThemeColor("#9B9B4B4") }
        }
    };

    public CodeThemeConfiguration()
    {
        Colors = new Dictionary<string, CodeThemeColor>();
    }

    public Dictionary<string, CodeThemeColor> Colors
    {
        get;
        set;
    }

    public CodeThemeColor GetThemeColor(string codename)
    {
        return Colors[codename];
    }
}
