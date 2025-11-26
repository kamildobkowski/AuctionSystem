using System.Text.RegularExpressions;
using AnyAscii;

namespace Auctions.Application.Common.Helpers;

public static class SlugHelper
{
	public static string Generate(Guid id, string title)
	{
		if (string.IsNullOrWhiteSpace(title))
		{
			return id.ToString();
		}

		var text = GetSlug(title);
        
		return $"{text}-{id:N}";
	}

	public static string? Check(Guid idFromRequest, string titleFromRequest, Guid id, string title)
	{
		var slug = $"{titleFromRequest}-{idFromRequest:N}";

		var expectedSlug = Generate(id, title);

		return slug.Equals(expectedSlug, StringComparison.Ordinal) ? null : GetSlug(expectedSlug);
	}

	private static string GetSlug(string title)
	{
		var text = title.Transliterate();
        
		text = text.ToLowerInvariant();
        
		text = Regex.Replace(text, @"[^a-z0-9\s-]", string.Empty);
        
		text = Regex.Replace(text, @"\s+", " ").Trim();
        
		text = Regex.Replace(text, @"\s", "-");
		return text;
	}
}