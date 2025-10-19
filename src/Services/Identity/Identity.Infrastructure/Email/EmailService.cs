using Shared.Events.EventBus;

namespace Identity.Infrastructure.Email;

public class EmailService(IEventBus eventBus) : IEmailService
{
	public Task SendAccountConfirmationEmail(string email, string code)
	{
		return Task.CompletedTask;
	}
}