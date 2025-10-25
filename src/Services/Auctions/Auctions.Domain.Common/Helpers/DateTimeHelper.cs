namespace Auctions.Domain.Common.Helpers;

public static class DateTimeHelper
{
	public static DateTime Now => DateTime.UtcNow;
	
	public static DateTime NextHour => RoundToNextHour(Now);
	
	public static DateTime Next10Minutes => RoundToNext10Minutes(Now);
	
	public static DateTime RoundToNextHour(DateTime dateTime)
	{
		var rounded = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0, DateTimeKind.Utc);
		return rounded.AddHours(1);
	}
	
	public static DateTime RoundToNext10Minutes(DateTime dateTime)
	{
		var minutesToAdd = 10 - (dateTime.Minute % 10);
		var rounded = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0, DateTimeKind.Utc);
		return rounded.AddMinutes(minutesToAdd);
	}
}