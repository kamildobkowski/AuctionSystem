using Identity.Domain.Entities;

namespace Identity.Domain.Services;

public interface IActivationCodeGenerator
{
	Task GenerateAndStoreCodeAsync(User user);
}