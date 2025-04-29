using Identity.Domain.ValueObjects;

namespace Identity.Domain.Entities;

public sealed class User
{
	public Guid Id { get; init; } = default!;
	public string Email { get; private set; } = default!;
	public string PasswordHash { get; private set; } = default!;
	public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
	public DateTime ModifiedAt { get; private set; } = DateTime.UtcNow;
	public bool IsCompany { get; private set; } = false;
	public CompanyDetails? CompanyDetails { get; private set; }
	public PersonalDetails? PersonalDetails { get; set; }

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
	
	
}