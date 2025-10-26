using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuctionSystem.ExternalServices.Helpers;

internal static class JsonHelper
{
	private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
	{
		PropertyNameCaseInsensitive = true,
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
		WriteIndented = false,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};

	static JsonHelper()
	{
		Options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
	}

	// Optional convenience helpers
	public static T? Deserialize<T>(string json) =>
		System.Text.Json.JsonSerializer.Deserialize<T>(json, Options);

	public static string Serialize<T>(T value) =>
		System.Text.Json.JsonSerializer.Serialize(value, Options);
}