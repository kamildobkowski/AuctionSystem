using Auctions.Application.Contracts.AuctionDetails;
using Shared.Base.Result;

namespace Auctions.Application.AuctionDetails.Services;

public interface ISellerDataService
{
	Task<Result<SellerDataModel>> GetSellerData(Guid sellerId);
}