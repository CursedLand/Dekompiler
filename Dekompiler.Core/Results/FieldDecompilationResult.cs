using AsmResolver.DotNet;
using Dekompiler.Core.Declaration;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Results;

public class FieldDecompilationResult : IDecompilationResult
{
	private FieldDefinition _field;
	private readonly RendererService _service;

	public FieldDecompilationResult(Icons.IconInfo icon, RenderFragment header, IMemberDeclaration declaration,
		FieldDefinition field, RendererService service)
	{
		Icon = icon;
		Header = header;
		Declaration = declaration;
		_field = field;
		_service = service;
	}

	public Icons.IconInfo Icon { get; }
	public RenderFragment Header { get; }
	public IMemberDefinition Member => _field;
	public IMemberDeclaration Declaration { get; }

	public RenderFragment RenderResult()
	{
		return _service.Concat(
			_service.Span($"// Token: 0x{_field.MetadataToken.ToInt32():x8} RID: {_field.MetadataToken.Rid}",
				"comment"), IDecompilationResult.NewLine(), Declaration.BuildDeclaration());
	}
}