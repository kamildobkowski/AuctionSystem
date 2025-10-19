using Identity.Domain.ValueObjects;

namespace Identity.Domain.Entities;

public sealed class User
{
	public Guid Id { get; init; } = Guid.NewGuid();
	public string Email { get; private set; } = default!;
	public string PasswordHash { get; private set; } = default!;
	public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
	public DateTime ModifiedAt { get; private set; } = DateTime.UtcNow;
	public bool IsCompany { get; private set; }
	public bool IsAccountActivated { get; private set; }
	public CompanyDetails? CompanyDetails { get; private init; }
	public PersonalDetails? PersonalDetails { get; private init; }
	public string? RefreshTokenHash { get; private set; }
	public DateTime? RefreshTokenExpiresAt { get; private set; }

	public static User CreatePersonal(string email, string passwordHash,
		string firstName, string lastName, PhoneNumber? phoneNumber = null)
	{
		return new User
		{
			Email = email,
			PasswordHash = passwordHash,
			IsCompany = false,
			PersonalDetails = new PersonalDetails
			{
				PhoneNumber = phoneNumber,
				FirstName = firstName,
				LastName = lastName
			}
		};
	}

	public static User CreateCompany(string email, string passwordHash,
		string companyName, string nip, Address address, PhoneNumber phoneNumber)
	{
		return new User
		{
			Email = email,
			PasswordHash = passwordHash,
			IsCompany = true,
			CompanyDetails = new CompanyDetails
			{
				Address = address,
				Name = companyName,
				Nip = nip,
				PhoneNumber = phoneNumber
			}
		};
	}
	
	public void ActivateAccount()
	{
		IsAccountActivated = true;
		ModifiedAt = DateTime.UtcNow;
	}
	
	public void SetRefreshToken(string token, DateTime expiresAt)
	{
		RefreshTokenHash = token;
		RefreshTokenExpiresAt = expiresAt;
		ModifiedAt = DateTime.UtcNow;
	}
	
	public string GetName()
		=> IsCompany ? CompanyDetails!.Name : $"{PersonalDetails!.FirstName} {PersonalDetails.LastName}";
}