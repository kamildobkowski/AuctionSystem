namespace Identity.Domain.ValueObjects;

public sealed class PhoneNumber
{
	public string Prefix { get; set; } = "+48";
	public string Number { get; set; } = default!;

	public override string ToString()
	{
		return Prefix + Number;
	}
}