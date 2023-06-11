using System.Text;
using AsmResolver.DotNet;
using Dekompiler.Core.TypeSystem;
using Microsoft.AspNetCore.Components;

namespace Dekompiler.Core.Declaration;

public class EventDeclaration : IMemberDeclaration
{
	private RendererService _renderer;
	private EventDefinition _event;

	public EventDeclaration(RendererService renderer, EventDefinition @event)
	{
		_event = @event;
		_renderer = renderer;
	}

	public RenderFragment BuildDeclaration()
	{
		return _renderer.Join(_renderer.Constants.Space, RenderAccessibility(), _renderer.Keyword("event"),
			RenderKind(), RenderProperty());
	}

	private RenderFragment RenderAccessibility()
	{
		var accessibility = () =>
		{
			return (_event.AddMethod ?? _event.FireMethod ?? _event.RemoveMethod) switch
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

			if ((_event.AddMethod ?? _event.FireMethod ?? _event.RemoveMethod) is { IsStatic: true })
				sb.Append("static ");

			return sb.ToString().TrimEnd(' ');
		};

		var accessFragment = accessibility() is { Length: not 0 } s ? _renderer.Keyword(s) : null;
		var modifierFragment = modifier() is { Length: not 0 } s2 ? _renderer.Keyword(s2) : null;

		return _renderer.Join(_renderer.Constants.Space, accessFragment, modifierFragment);
	}

	private RenderFragment RenderKind()
	{
		return new TypeSignatureRenderer(_renderer, _event.EventType!.ToTypeSignature(), _event, _event.DeclaringType)
			.RenderMember();
	}

	private RenderFragment RenderProperty()
	{
		return new EventRenderer(_renderer, _event).RenderMember();
	}
}