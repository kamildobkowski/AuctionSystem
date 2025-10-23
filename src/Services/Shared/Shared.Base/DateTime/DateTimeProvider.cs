namespace Shared.Base.DateTime;

public static class DateTimeProvider
{
	public static System.DateTime Now => System.DateTime.UtcNow;
	
	public static DateOnly Today => DateOnly.FromDateTime(Now);
}