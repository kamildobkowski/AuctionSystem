namespace Auctions.Domain.Entities.Base;

public abstract class Aggregate<TId>
{
	public TId Id { get; protected set; } = default!;
}