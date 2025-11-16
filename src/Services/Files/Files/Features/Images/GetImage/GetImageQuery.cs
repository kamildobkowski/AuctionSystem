using Shared.Base.Cqrs.Queries;

namespace Files.Features.Images.GetImage;

public sealed record GetImageQuery(Guid Id) : IQuery;