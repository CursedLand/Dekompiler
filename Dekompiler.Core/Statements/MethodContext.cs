using AsmResolver.DotNet;
using AsmResolver.DotNet.Code.Cil;

namespace Dekompiler.Core.Statements;

public class MethodContext
{
    private readonly UniqueNameGenerator _generator = new();
    private readonly Dictionary<CilLocalVariable, string> _generatorCache = new();

#pragma warning disable CS8618
    private MethodContext()
    {
    }
#pragma warning restore CS8618

    public MethodContext(MethodDefinition methodDefinition, RendererService renderer)
    {
        Method = methodDefinition;
        Renderer = renderer;
        InitializedVariable = methodDefinition.CilMethodBody?.LocalVariables.ToDictionary(loc => loc, _ => false) ?? new();
        Stack = new Stack<IStatement>();
        CompleteStatements = new List<IStatement>();
    }

    public Dictionary<CilLocalVariable, bool> InitializedVariable { get; }

    public RendererService Renderer { get; }

    public Stack<IStatement> Stack { get; }

    public List<IStatement> CompleteStatements { get; }

    public MethodDefinition Method { get; }

    public static MethodContext Null => new();

    public bool Empty { get; set; }

    public string GenerateName(CilLocalVariable variable)
    {
        var type = (variable.VariableType.Resolve()?.Name ?? variable.VariableType.Name)!.Value;

        var index = type.LastIndexOf('`');
        var name = index is not -1
            ? type.Substring(0, index)
            : type;

        if (_generatorCache.TryGetValue(variable, out var generateName))
            return generateName;
        return _generatorCache[variable] = _generator.GenerateUniqueName(name);
    }
}

public class UniqueNameGenerator
{
    private readonly Dictionary<string, int> nameCache;

    public UniqueNameGenerator()
    {
        nameCache = new Dictionary<string, int>();
    }

    public string GenerateUniqueName(string name)
    {
        if (nameCache.ContainsKey(name))
        {
            var count = nameCache[name] + 1;
            nameCache[name] = count;
            name = $"{name}{count}";
        }
        else
        {
            nameCache.Add(name, 0);
        }

        name = CamelCase(name);
        return name;
    }

    private string CamelCase(string word)
    {
        if (string.IsNullOrEmpty(word))
            return word;

        return char.ToLowerInvariant(word[0]) + word.Substring(1);
    }
}