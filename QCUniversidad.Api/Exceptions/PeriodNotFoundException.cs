using System.Runtime.Serialization;

namespace QCUniversidad.Api.Exceptions;

public class PeriodNotFoundException : Exception
{
    public PeriodNotFoundException()
    {
    }

    public PeriodNotFoundException(string? message) : base(message)
    {
    }

    public PeriodNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected PeriodNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
