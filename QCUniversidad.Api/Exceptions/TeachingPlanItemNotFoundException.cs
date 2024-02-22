using System.Runtime.Serialization;

namespace QCUniversidad.Api.Exceptions;

public class TeachingPlanItemNotFoundException : Exception
{
    public TeachingPlanItemNotFoundException()
    {
    }

    public TeachingPlanItemNotFoundException(string? message) : base(message)
    {
    }

    public TeachingPlanItemNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected TeachingPlanItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
