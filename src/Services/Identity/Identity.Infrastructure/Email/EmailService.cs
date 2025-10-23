namespace Identity.Infrastructure.Email;

public class EmailService() : IEmailService
{
	public Task SendAccountConfirmationEmail(string email, string code)
	{
		return Task.CompletedTask;
	}
}