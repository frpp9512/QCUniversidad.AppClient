namespace QCUniversidad.Api.Extensions;

public static class DateTimeExtensions
{
    public static DateTime? SetKindUtc(this DateTime? dateTime)
    {
        return dateTime.HasValue ? dateTime.Value.SetKindUtc() : null;
    }

    public static DateTime SetKindUtc(this DateTime dateTime)
    {
        return dateTime.Kind == DateTimeKind.Utc ? dateTime : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }

    public static DateTime? SetKindUtc(this DateTimeOffset? dateTimeOffset)
    {
        return dateTimeOffset.HasValue ? dateTimeOffset.Value.SetKindUtc() : null;
    }

    public static DateTime SetKindUtc(this DateTimeOffset dateTimeOffset)
    {
        return dateTimeOffset.DateTime.Kind == DateTimeKind.Utc ? dateTimeOffset.DateTime : DateTime.SpecifyKind(dateTimeOffset.DateTime, DateTimeKind.Utc);
    }
}