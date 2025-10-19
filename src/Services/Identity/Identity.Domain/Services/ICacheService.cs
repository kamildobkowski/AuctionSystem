namespace Identity.Domain.Services;

public interface ICacheService
{
	Task<bool> IsEmailTaken(string email);
	Task SetEmailTaken(string email);
	Task<bool> IsActivationCodeTaken(string code);
	Task SetActivationCode(string email, string code);
}