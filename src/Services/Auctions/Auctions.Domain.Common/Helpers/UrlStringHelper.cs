using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Auctions.Domain.Common.Helpers;

public static class UrlStringHelper
{
	private static readonly Regex InvalidChars = new("[^a-z0-9\\s-]", RegexOptions.Compiled);
	private static readonly Regex CollapseDashes = new("[\\s-]+", RegexOptions.Compiled);
	private static readonly Regex TrimDashes = new("(^-+|-+$)", RegexOptions.Compiled);

	public static string Encode(string input)
	{
		if (string.IsNullOrWhiteSpace(input)) return string.Empty;
		
		var normalized = input.Normalize(NormalizationForm.FormD);
		var sb = new StringBuilder(normalized.Length);
		foreach (var c in normalized.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
			sb.Append(c);

		var clean = sb.ToString().Normalize(NormalizationForm.FormC).ToLowerInvariant();
		
		clean = InvalidChars.Replace(clean, "");
		
		clean = CollapseDashes.Replace(clean, "-");
		
		clean = TrimDashes.Replace(clean, "");

		return clean;
	}
	
	public static string GenerateUrl(string title, Guid id)
	{
		return string.Concat(Encode(title), ' ', id.ToString("N"));
	}
}