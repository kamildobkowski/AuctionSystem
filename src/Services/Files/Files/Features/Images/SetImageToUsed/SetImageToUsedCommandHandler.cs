using Files.Core.Persistence;
using Microsoft.EntityFrameworkCore;
using Shared.Base.Cqrs.Commands;
using Shared.Base.Errors;
using Shared.Base.Result;
using Shared.Events.Events.Files;

namespace Files.Features.Images.SetImageToUsed;

public sealed class SetImageToUsedCommandHandler(ImagesDbContext dbContext) 
	: ICommandHandler<SetImageToUsedCommand, NullResult>
{
	public async Task<ICommandResult<NullResult>> HandleAsync(
		SetImageToUsedCommand command, CancellationToken cancellationToken = default)
	{
		var image = await dbContext.Images.FirstOrDefaultAsync(x => x.Id == command.ImageId, cancellationToken);
		if (image is null)
			return CommandResult.Failure<NullResult>(ErrorResult.NotFoundError);
		image.IsUsed = true;
		await dbContext.SaveChangesAsync(cancellationToken);
		return CommandResult.Success(NullResult.Instance);
	}
}