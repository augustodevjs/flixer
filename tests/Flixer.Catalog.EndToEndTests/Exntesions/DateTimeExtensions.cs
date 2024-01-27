namespace Flixer.Catalog.EndToEndTests.Exntesions;

internal static class DateTimeExtensions
{
    public static DateTime TrimMilisseconds(this DateTime dateTime)
    {
        return new DateTime(
            dateTime.Year,
            dateTime.Month,
            dateTime.Day,
            dateTime.Hour,
            dateTime.Minute,
            dateTime.Second,
            0,
            dateTime.Kind
        );
    }
}
