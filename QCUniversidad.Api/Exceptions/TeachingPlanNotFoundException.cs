using System.Runtime.Serialization;

namespace QCUniversidad.Api.Exceptions;

public class TeachingPlanNotFoundException : Exception
{
    public TeachingPlanNotFoundException()
    {
    }

    public TeachingPlanNotFoundException(string? message) : base(message)
    {
    }

    public TeachingPlanNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected TeachingPlanNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
