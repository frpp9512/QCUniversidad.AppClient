using System.Runtime.Serialization;

namespace QCUniversidad.Api.Exceptions;

public class PeriodSubjectNotFoundException : Exception
{
    public PeriodSubjectNotFoundException()
    {
    }

    public PeriodSubjectNotFoundException(string? message) : base(message)
    {
    }

    public PeriodSubjectNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected PeriodSubjectNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
