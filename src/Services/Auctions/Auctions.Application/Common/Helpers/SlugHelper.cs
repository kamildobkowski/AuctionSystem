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

		var text = title.Transliterate();
        
		text = text.ToLowerInvariant();
        
		text = Regex.Replace(text, @"[^a-z0-9\s-]", string.Empty);
        
		text = Regex.Replace(text, @"\s+", " ").Trim();
        
		text = Regex.Replace(text, @"\s", "-");
        
		return $"{text}-{id:N}";
	}

	public static bool Check(string slug, Guid id, string title)
	{
		if (string.IsNullOrWhiteSpace(slug))
		{
			return false;
		}

		var expectedSlug = Generate(id, title);

		return slug.Equals(expectedSlug, StringComparison.Ordinal);
	}
}