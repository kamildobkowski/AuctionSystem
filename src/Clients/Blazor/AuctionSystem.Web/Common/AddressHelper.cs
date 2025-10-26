using Maddalena;

namespace AuctionSystem.Web.Common;

public static class AddressHelper
{
	public static readonly string[] PhonePrefixes = [
		"+1",
		"+44",
		"+48",
		"+49",
		"+69",
		"+880"
	];

	public static readonly IReadOnlyDictionary<string, string> Countries = 
		Country
			.All
			.OrderBy(x => x.CommonName)
			.ToDictionary(x => x.CommonName, x => x.CountryCode.ToString());
}