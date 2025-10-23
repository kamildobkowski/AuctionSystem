namespace Shared.Events.Events.Users;

public class PersonalUserCreatedEvent
{
	public static string Topic { get; } = "users.personaluser.created.v1";

	public Guid UserId { get; set; }

	public string Email { get; set; } = null!;

	public string FirstName { get; set; } = null!;

	public string LastName { get; set; } = null!;
}