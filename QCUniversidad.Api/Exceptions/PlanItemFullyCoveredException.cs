using System.Runtime.Serialization;

namespace QCUniversidad.Api.Exceptions;

public class PlanItemFullyCoveredException : Exception
{
    public PlanItemFullyCoveredException()
    {
    }

    public PlanItemFullyCoveredException(string? message) : base(message)
    {
    }

    public PlanItemFullyCoveredException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected PlanItemFullyCoveredException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
