namespace Shared.Events.Events.Email;

public class SendEmailEvent
{
	public static string Topic { get; } = "email.send";
	public Guid Id { get; set; } = Guid.NewGuid();
	public required string Template { get; set; }
	public required List<string> Recipients { get; set; } = [];
	public required string Title { get; set; }
	public required Dictionary<string, object> Parameters { get; set; } = [];
}