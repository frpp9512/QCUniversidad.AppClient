namespace QCUniversidad.Api.Extensions;

public static class DateTimeExtensions
{
    public static DateTime? SetKindUtc(this DateTime? dateTime) => dateTime.HasValue ? dateTime.Value.SetKindUtc() : null;
    public static DateTime SetKindUtc(this DateTime dateTime)
        => dateTime.Kind == DateTimeKind.Utc ? dateTime : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);

    public static DateTime? SetKindUtc(this DateTimeOffset? dateTimeOffset) => dateTimeOffset.HasValue ? dateTimeOffset.Value.SetKindUtc() : null;
    public static DateTime SetKindUtc(this DateTimeOffset dateTimeOffset)
        => dateTimeOffset.DateTime.Kind == DateTimeKind.Utc ? dateTimeOffset.DateTime : DateTime.SpecifyKind(dateTimeOffset.DateTime, DateTimeKind.Utc);
}