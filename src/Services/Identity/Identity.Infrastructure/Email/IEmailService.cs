namespace Identity.Infrastructure.Email;

public interface IEmailService
{
	Task SendAccountConfirmationEmail(string email, string code);
}