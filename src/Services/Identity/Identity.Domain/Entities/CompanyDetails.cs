using Identity.Domain.ValueObjects;

namespace Identity.Domain.Entities;

public sealed class CompanyDetails
{
	public Guid Id { get; set; }
	public string Name { get; set; } = default!;
	public Address Address { get; set; } = default!;
	public string Nip { get; set; } = default!;
	public PhoneNumber PhoneNumber { get; set; } = default!;
}