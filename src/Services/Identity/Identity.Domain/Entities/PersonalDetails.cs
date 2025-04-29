using Identity.Domain.ValueObjects;

namespace Identity.Domain.Entities;

public sealed class PersonalDetails
{
	public Guid Id { get; set; }
	public string FirstName { get; set; } = default!;
	public string LastName { get; set; } = default!;
	public PhoneNumber? PhoneNumber { get; set; }
}