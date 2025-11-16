namespace Files.Core.Configuration;

public sealed class ImageConfiguration
{
	public int MaxWidth { get; set; } = 1920;
	public int MaxHeight { get; set; }= 1920;
	public int Quality { get; set; }= 80;
}