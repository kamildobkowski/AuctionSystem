namespace Auctions.Infrastructure.Database;

public static class FtsHelper
{
	public static string FormatFtsQuery(string filter)
	{
		var words = filter.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
		
		var formattedQuery = string.Join(" & ", words.Select(w => w + ":*"));

		return formattedQuery;
	}
}