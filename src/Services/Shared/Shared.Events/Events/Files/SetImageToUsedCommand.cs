using Shared.Base.Cqrs.Commands;

namespace Shared.Events.Events.Files;

public sealed class SetImageToUsedCommand : ICommand
{
	public const string Topic = "files.image.used.command.v1";

	public Guid ImageId { get; set; }
}