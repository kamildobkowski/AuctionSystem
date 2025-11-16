using MassTransit;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Result;
using Shared.Events.Common;
using Shared.Events.Events.Files;

namespace Files.Features.Images.SetImageToUsed;

public sealed class SetImageToUsedCommandConsumer(ICommandHandler<SetImageToUsedCommand, NullResult> handler) 
	: IConsumer<SetImageToUsedCommand>
{
	public async Task Consume(ConsumeContext<SetImageToUsedCommand> context)
	{
		var result = await handler.HandleAsync(context.Message);
		if (!result.IsSuccess)
			throw new EventProcessingFailedException(result.ErrorResult!);
	}
}