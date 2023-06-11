using System.Diagnostics.CodeAnalysis;
using System.Runtime.Versioning;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Signatures.Types;
using AsmResolver.PE.DotNet.Cil;
using Dekompiler.Components.CodeViewer;
using Dekompiler.Components.TreeView;
using Dekompiler.Core;
using Dekompiler.Core.Declaration;
using Dekompiler.Core.Results;
using Dekompiler.Core.Statements;
using Dekompiler.Core.Statements.Miscellaneous;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

// ReSharper disable RedundantCast

namespace Dekompiler.Pages;

public partial class Index : ComponentBase
{
#pragma warning disable CS8618
	[NotNull] private TreeView? _treeView;
	[NotNull] private CodeView? _codeView;
#pragma warning restore CS8618

	private static Dictionary<CilCode, Func<MethodContext, IStatement>> _handlers = Initialize();
	private List<AssemblyDefinition> _loadedAssemblies = new();

	private static Dictionary<CilCode, Func<MethodContext, IStatement>> Initialize()
	{
		var handlers = new Dictionary<CilCode, Func<MethodContext, IStatement>>();
		foreach (var type in typeof(Statement).Module.GetTypes())
		{
			if (typeof(CommentStatement) == type) continue;
			if (typeof(KeywordStatement) == type) continue;
			if (type.IsInterface || type.IsAbstract) continue;
			if (!typeof(Statement).IsAssignableFrom(type)) continue;

			var activatedRenderer = (Statement?)Activator.CreateInstance(type, MethodContext.Null);

			if (activatedRenderer is null) continue;

			foreach (var code in activatedRenderer.Codes)
			{
				handlers[code] = (ctx) => (Statement)Activator.CreateInstance(type, ctx)!;
			}
		}

		return handlers;
	}

	public async Task LoadAssembly(InputFileChangeEventArgs arg)
	{
		_loadedAssemblies.Clear();
		foreach (var file in arg.GetMultipleFiles(1337))
		{
			await using MemoryStream stream = new();
			await file.OpenReadStream(long.MaxValue).CopyToAsync(stream);
			try
			{
				var assembly = AssemblyDefinition.FromBytes(stream.ToArray());

				_loadedAssemblies.Add(assembly);
			}
			catch
			{
				// ignore.
			}
		}

		var tmp = new List<AssemblyDefinition>();

		var tree = new List<TreeViewNodeModel>();

		foreach (var assembly in _loadedAssemblies)
		{
			tree.Add(FormAssemblyTree(assembly));

			// load libraries.
			foreach (var module in assembly.Modules)
			{
				foreach (var assemblyReference in module.AssemblyReferences)
				{
					if (assemblyReference.Resolve() is null &&
					    _loadedAssemblies.FirstOrDefault(asm => asm.Name == assemblyReference.Name) is
						    { } assemblyDefinition)
					{
						module.MetadataResolver.AssemblyResolver.AddToCache(assemblyReference, assemblyDefinition);
					}
					/*else if (assemblyReference.Resolve() is { } resolvedAssemblyDefinition && !tmp.Contains(resolvedAssemblyDefinition))
					{
					    tree.Add(FormAssemblyTree(resolvedAssemblyDefinition));
					    tmp.Add(resolvedAssemblyDefinition);
					}*/
				}
			}
		}


		tree.Add(GetUnSupportedOpCodes());

		_treeView.SetNodes(tree);
	}

	private TreeViewNodeModel GetUnSupportedOpCodes()
	{
		var cilCodes = Enum.GetValues<CilCode>().OrderBy(e => e.ToString(), StringComparer.Ordinal);

		var unsupportedCodes = cilCodes.Where(code => !_handlers.ContainsKey(code)).Select(code =>
			new TreeViewNodeModel(Icons.EnumItem, () => Renderer.Span(code.ToString(), "enumfield"))).ToList();

		return new TreeViewNodeModel(Icons.Enum, () => Renderer.Span("UnSupported.CilOpCodes", "enum"),
			unsupportedCodes);
	}

	private TreeViewNodeModel FormAssemblyTree(AssemblyDefinition assembly)
	{
		var assemblyTree = new List<TreeViewNodeModel>();

		foreach (var module in assembly.Modules)
		{
			assemblyTree.Add(FormModuleTree(module));
		}

		return new TreeViewNodeModel(Icons.Assembly, () => Renderer.Text(assembly.Name), assemblyTree);
	}

	private TreeViewNodeModel FormAssemblyReferenceTree(ModuleDefinition module)
	{
		var assemblyReferences = new List<TreeViewNodeModel>();

		foreach (var assemblyReference in module.AssemblyReferences)
		{
			assemblyReferences.Add(new TreeViewNodeModel(Icons.AssemblyReference,
				() => Renderer.Text(assemblyReference.Name)));
		}

		return new TreeViewNodeModel(Icons.AssemblyReference,
			() => Renderer.Text("Assembly References"), assemblyReferences);
	}

	private IEnumerable<TreeViewNodeModel> FormNamespaceTrees(ModuleDefinition module)
	{
		var tempTree = new Dictionary<string, List<TypeDefinition>>();

		foreach (var type in module.TopLevelTypes)
		{
			var fixedNamespace = string.IsNullOrEmpty(type.Namespace) ? "-" : (string)type.Namespace;

			if (!tempTree.ContainsKey(fixedNamespace)) tempTree[fixedNamespace] = new List<TypeDefinition>();

			tempTree[fixedNamespace].Add(type);
		}

		foreach (var (ns, types) in tempTree.OrderBy(pair => pair.Key, StringComparer.Ordinal))
		{
			var nsTree = new List<TreeViewNodeModel>();

			foreach (var type in types.OrderBy(type => (string?)type.Name, StringComparer.Ordinal))
			{
				nsTree.Add(FormTypeTree(type));
			}

			yield return new TreeViewNodeModel(Icons.Namespace, () => Renderer.Text(ns), nsTree);
		}
	}

	private TreeViewNodeModel FormModuleTree(ModuleDefinition module)
	{
		var moduleTree = new List<TreeViewNodeModel>();

		moduleTree.Add(FormAssemblyReferenceTree(module));

		moduleTree.AddRange(FormNamespaceTrees(module));

		return new TreeViewNodeModel(Icons.Module, () => Renderer.Span(module.Name, "ilmodule"), moduleTree);
	}

	private TreeViewNodeModel FormTypeTree(TypeDefinition type)
	{
		var typeTree = new List<TreeViewNodeModel>();

		var icon = type switch
		{
			{ IsInterface: true } => Icons.Interface,
			{ IsDelegate: true } => Icons.Delegate,
			{ IsEnum: true } => Icons.Enum,
			{ IsValueType: true } => Icons.ValueType,
			_ => Icons.Type
		};

		typeTree.AddRange(FormMethodTrees(type));
		typeTree.AddRange(FormPropertyTrees(type));
		typeTree.AddRange(FormEventTrees(type));
		typeTree.AddRange(FormFieldTrees(type));

		foreach (var nestedType in type.NestedTypes.OrderBy(nestedType => (string?)nestedType.Name,
			         StringComparer.Ordinal))
		{
			typeTree.Add(FormTypeTree(nestedType));
		}

		return new TreeViewNodeModel(icon, () => Decompile(type), () => RenderType(type), typeTree);
	}

	private RenderFragment RenderType(TypeDefinition type)
	{
		var typeSpan = new TypeRenderer(Renderer, type).RenderMember();

		if (type.GenericParameters.Count > 0)
		{
			var separator = Renderer.Concat(Renderer.Constants.Comma, Renderer.Constants.Space);

			var genericArgs = Renderer.Join(separator,
				type.GenericParameters.Select(arg => Renderer.Span(arg.Name, "genericparameter")).ToArray());

			return Renderer.Concat(typeSpan, Renderer.Constants.RightThan, genericArgs,
				Renderer.Constants.LeftThan);
		}

		return typeSpan;
	}

	private IEnumerable<TreeViewNodeModel> FormMethodTrees(TypeDefinition type)
	{
		foreach (var method in type.Methods.OrderBy(method => (string?)method.Name, StringComparer.Ordinal)
			         .ThenBy(method => method.IsConstructor))
		{
			if (method.IsGetMethod || method.IsSetMethod) continue;

			if (method.IsAddMethod || method.IsFireMethod || method.IsRemoveMethod) continue;

			yield return FormMethodTree(method);
		}
	}

	private IEnumerable<TreeViewNodeModel> FormPropertyTrees(TypeDefinition type)
	{
		foreach (var property in type.Properties.OrderBy(property => (string?)property.Name, StringComparer.Ordinal))
		{
			yield return FormPropertyTree(property);
		}
	}

	private IEnumerable<TreeViewNodeModel> FormEventTrees(TypeDefinition type)
	{
		foreach (var @event in type.Events.OrderBy(@event => (string?)@event.Name, StringComparer.Ordinal))
		{
			yield return FormEventTree(@event);
		}
	}

	private IEnumerable<TreeViewNodeModel> FormFieldTrees(TypeDefinition type)
	{
		foreach (var field in type.Fields.OrderBy(field => (string?)field.Name, StringComparer.Ordinal)
			         .ThenBy(field => field.Constant is not null))
		{
			yield return FormFieldTree(field);
		}
	}

	private TreeViewNodeModel FormMethodTree(MethodDefinition method)
	{
		return new TreeViewNodeModel(Icons.Method, () => Decompile(method), () => RenderMethod(method));
	}

	private RenderFragment RenderMethod(MethodDefinition method)
	{
		var methodSpan = new MethodRenderer(Renderer, method).RenderMember();

		if (method.IsConstructor)
		{
			// :)
			var tmp = method.DeclaringType!.Name;
			if (method.IsStatic)
			{
				method.DeclaringType.Name = ".cctor";
			}

			methodSpan = RenderType(method.DeclaringType!);

			method.DeclaringType!.Name = tmp;
		}

		var returnType =
			Renderer.Concat(Renderer.Constants.Column, Renderer.Constants.Space,
				new TypeSignatureRenderer(Renderer, method.Signature!.ReturnType, method, method.DeclaringType!)
					.RenderMember());

		var parameterSpan = Renderer.Concat(Renderer.Constants.RightBracket, Renderer.Constants.LeftBracket);

		if (method.Parameters.Count > 0)
		{
			parameterSpan = RenderMethodParameters(method);
		}

		return Renderer.Concat(methodSpan, parameterSpan, returnType);
	}

	private RenderFragment RenderMethodParameters(MethodDefinition method)
	{
		var separator = Renderer.Concat(Renderer.Constants.Comma, Renderer.Constants.Space);

		var renderedParameters = new List<RenderFragment>();

		foreach (var parameter in method.Parameters)
		{
			if (parameter.MethodSignatureIndex is -1) continue;

			var parameterSpan =
				new TypeSignatureRenderer(Renderer, parameter.ParameterType, method, method.DeclaringType!)
					.RenderMember();

			if (parameter.Definition is { IsOut: true })
				parameterSpan = Renderer.Join(Renderer.Constants.Space, Renderer.Keyword("out"), parameterSpan);

			if (parameter.Definition is { IsIn: true })
				parameterSpan = Renderer.Join(Renderer.Constants.Space, Renderer.Keyword("in"), parameterSpan);

			renderedParameters.Add(parameterSpan);
		}

		var argTypes = Renderer.Join(separator, renderedParameters.ToArray());

		return Renderer.Concat(Renderer.Constants.RightBracket, argTypes, Renderer.Constants.LeftBracket);
	}

	private TreeViewNodeModel FormPropertyTree(PropertyDefinition property)
	{
		var propertyTree = new List<TreeViewNodeModel>();

		if (property.GetMethod is { } getMethod)
		{
			propertyTree.Add(FormMethodTree(getMethod));
		}

		if (property.SetMethod is { } setMethod)
		{
			propertyTree.Add(FormMethodTree(setMethod));
		}

		return new TreeViewNodeModel(Icons.Property, () => RenderProperty(property), propertyTree);
	}

	private RenderFragment RenderProperty(PropertyDefinition property)
	{
		var propertySpan = new PropertyRenderer(Renderer, property).RenderMember();

		var returnType =
			Renderer.Concat(Renderer.Constants.Column, Renderer.Constants.Space,
				new TypeSignatureRenderer(Renderer, property.Signature!.ReturnType, property, property.DeclaringType!)
					.RenderMember());

		// indexer
		if (property.Signature!.ParameterTypes.Count > 0)
		{
			var separator = Renderer.Concat(Renderer.Constants.Comma, Renderer.Constants.Space);

			var tmp = (TypeSignature parameter) =>
				new TypeSignatureRenderer(Renderer, parameter, property, property.DeclaringType).RenderMember();

			propertySpan = Renderer.Join(separator,
				property.Signature!.ParameterTypes.Select(parameter => tmp(parameter)).ToArray());

			propertySpan = Renderer.Concat(Renderer.Keyword("this"), Renderer.Constants.RightArrayBracket, propertySpan,
				Renderer.Constants.LeftArrayBracket);
		}

		return Renderer.Concat(propertySpan, returnType);
	}

	private TreeViewNodeModel FormEventTree(EventDefinition @event)
	{
		var eventTree = new List<TreeViewNodeModel>();

		if (@event.AddMethod is { } addMethod)
		{
			eventTree.Add(FormMethodTree(addMethod));
		}

		if (@event.FireMethod is { } fireMethod)
		{
			eventTree.Add(FormMethodTree(fireMethod));
		}

		if (@event.RemoveMethod is { } removeMethod)
		{
			eventTree.Add(FormMethodTree(removeMethod));
		}

		return new TreeViewNodeModel(Icons.Event, () => RenderEvent(@event), eventTree);
	}

	private RenderFragment RenderEvent(EventDefinition @event)
	{
		var eventSpan = new EventRenderer(Renderer, @event).RenderMember();

		var eventType =
			Renderer.Concat(Renderer.Constants.Column, Renderer.Constants.Space,
				new TypeSignatureRenderer(Renderer, @event.EventType!.ToTypeSignature(), @event, @event.DeclaringType)
					.RenderMember());

		return Renderer.Concat(eventSpan, eventType);
	}

	private TreeViewNodeModel FormFieldTree(FieldDefinition field)
	{
		var icon = field switch
		{
			{ DeclaringType: { BaseType: { FullName: "System.Enum" } } } => Icons.EnumItem,
			{ Constant: not null } => Icons.Constant,
			_ => Icons.Field
		};

		return new TreeViewNodeModel(icon, () => RenderField(field));
	}

	private RenderFragment RenderField(FieldDefinition field)
	{
		var propertySpan = new FieldRenderer(Renderer, field).RenderMember();

		var eventType =
			Renderer.Concat(Renderer.Constants.Column, Renderer.Constants.Space,
				new TypeSignatureRenderer(Renderer, field.Signature!.FieldType, field, field.DeclaringType)
					.RenderMember());

		return Renderer.Concat(propertySpan, eventType);
	}

	private void Decompile(IMemberDefinition member)
	{
		_codeView.Result = member switch
		{
			FieldDefinition fieldDefinition => DecompileField(fieldDefinition),
			EventDefinition eventDefinition => DecompileEvent(eventDefinition),
			MethodDefinition methodDefinition => DecompileMethod(methodDefinition),
			PropertyDefinition propertyDefinition => DecompileProperty(propertyDefinition),
			TypeDefinition typeDefinition => DecompileType(typeDefinition),
			_ => throw new ArgumentOutOfRangeException(nameof(member))
		};
	}

	private IDecompilationResult DecompileType(TypeDefinition type)
	{
		var icon = type switch
		{
			{ IsInterface: true } => Icons.Interface,
			{ IsDelegate: true } => Icons.Delegate,
			{ IsEnum: true } => Icons.Enum,
			{ IsValueType: true } => Icons.ValueType,
			_ => Icons.Type
		};

		var result = new TypeDecompilationResult(icon, RenderType(type), new TypeDeclaration(Renderer, type), type,
			Renderer);

		foreach (var method in type.Methods)
		{
			if (method.IsGetMethod || method.IsSetMethod) continue;

			if (method.IsAddMethod || method.IsFireMethod || method.IsRemoveMethod) continue;

			result.MethodResults.Add((MethodDecompilationResult)DecompileMethod(method));
		}

		foreach (var property in type.Properties)
		{
			result.PropertyResults.Add((PropertyDecompilationResult)DecompileProperty(property));
		}

		foreach (var @event in type.Events)
		{
			result.EventResults.Add((EventDecompilationResult)DecompileEvent(@event));
		}

		foreach (var field in type.Fields)
		{
			result.FieldResults.Add((FieldDecompilationResult)DecompileField(field));
		}

		foreach (var nestedType in type.NestedTypes)
		{
			result.NestedTypeResults.Add((TypeDecompilationResult)DecompileType(nestedType));
		}


		return result;
	}

	private IDecompilationResult DecompileProperty(PropertyDefinition property)
	{
		var getResult =
			(MethodDecompilationResult?)(property.GetMethod is { } getMethod ? DecompileMethod(getMethod) : null);
		var setResult =
			(MethodDecompilationResult?)(property.SetMethod is { } setMethod ? DecompileMethod(setMethod) : null);

		return new PropertyDecompilationResult(Icons.Property, RenderProperty(property),
			new PropertyDeclaration(Renderer, property), getResult, setResult, Renderer, property);
	}

	private IDecompilationResult DecompileMethod(MethodDefinition method)
	{
		var ctx = new MethodContext(method, Renderer);

		try
		{
			if (ctx.Method.CilMethodBody is { } cilMethodBody)
			{
				if (cilMethodBody.Instructions.Any(instruction => !_handlers.ContainsKey(instruction.OpCode.Code)))
				{
					ctx.CompleteStatements.Add(
						new CommentStatement(ctx, "This method contains unsupported CilOpCodes."));
					var usedCodes = cilMethodBody.Instructions.Select(instruction => instruction.OpCode.Code)
						.Distinct();
					ctx.CompleteStatements.Add(new CommentStatement(ctx,
						string.Join(", ", usedCodes.Where(code => !_handlers.ContainsKey(code)))));
				}
				else
				{
					cilMethodBody.Instructions.ExpandMacros();

					foreach (var instr in cilMethodBody.Instructions)
					{
						var statement = _handlers[instr.OpCode.Code](ctx);

						statement.Deserialize(instr);
						statement.InterpretStack();
					}
				}
			}
			else
			{
				ctx.Empty = true;
			}
		}
		catch (Exception ex)
		{
			ctx.CompleteStatements.Add(new CommentStatement(ctx, $"--- ---{ex.Message}--- ---"));
			ctx.CompleteStatements.Add(new CommentStatement(ctx, $"--- ---{ex.StackTrace}--- ---"));
		}

		IMemberDeclaration declaration = method switch
		{
			{ IsAddMethod: true } => new AddDeclaration(Renderer),
			{ IsRemoveMethod: true } => new RemoveDeclaration(Renderer),
			{ IsGetMethod: true } => new GetDeclaration(Renderer),
			{ IsSetMethod: true } => new SetDeclaration(Renderer),
			_ => new MethodDeclaration(Renderer, method)
		};

		return new MethodDecompilationResult(Icons.Method, RenderMethod(method),
			declaration, ctx);
	}

	private IDecompilationResult DecompileEvent(EventDefinition @event)
	{
		var getResult =
			(MethodDecompilationResult?)(@event.AddMethod is { } addMethod ? DecompileMethod(addMethod) : null);
		var setResult =
			(MethodDecompilationResult?)(@event.RemoveMethod is { } removeMethod
				? DecompileMethod(removeMethod)
				: null);

		return new EventDecompilationResult(Icons.Property, RenderEvent(@event),
			new EventDeclaration(Renderer, @event), getResult, setResult, Renderer, @event);
	}

	private IDecompilationResult DecompileField(FieldDefinition field)
	{
		return new FieldDecompilationResult(Icons.Field, RenderField(field), new FieldDeclaration(Renderer, field),
			field, Renderer);
	}
}