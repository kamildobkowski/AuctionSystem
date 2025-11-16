using Microsoft.AspNetCore.Http;
using Shared.Base.Cqrs.Commands;

namespace Auctions.Application.Contracts.Picture;

public sealed record AddPictureCommand(IFormFile Picture) : ICommand;