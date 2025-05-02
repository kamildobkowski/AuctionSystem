using Shared.Events.EventBus;

namespace Shared.Events.Events;

public class EmailVerificationRequiredEvent : IEvent
{
	public static string Topic => "account_activation_required";

	public required string Email { get; set; } = default!;
	public required string Code { get; set; } = default!;
}