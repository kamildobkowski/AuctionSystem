using Shared.Events.EventBus;

namespace Shared.Events.Events;

public class PersonalUserCreatedEvent : IEvent
{
	public Guid UserId { get; set; }
	public string FirstName { get; set; } = default!;
	public string LastName { get; set; } = default!;
	public string Email { get; set; } = default!;
	
	public static string Topic { get; } = "personal_user_created";
}