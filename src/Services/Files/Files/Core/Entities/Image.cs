namespace Files.Core.Entities;

public class Image
{
	public Guid Id { get; init; }

	public string FileName { get; private set; } = null!;
	
	public bool IsUsed { get; set; }

	public Guid CreatedById { get; private set; }

	public DateTime Created { get; init; }

	public DateTime LastModified { get; private set; }

	private Image() { }
	
	public Image(string extension, Guid userId)
	{
		Id = Guid.NewGuid();
		FileName = GenerateFileName(Id, extension);
		CreatedById = userId;
		Created = DateTime.UtcNow;
		LastModified = DateTime.UtcNow;
	}
	
	private static string GenerateFileName(Guid id, string extension)
	{
		if (!extension.StartsWith('.'))
			extension = string.Concat('.', extension);
		
		return string.Concat(id.ToString("N"), extension);
	}
}