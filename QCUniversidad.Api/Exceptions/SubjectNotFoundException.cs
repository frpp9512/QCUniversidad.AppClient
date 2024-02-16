using System.Runtime.Serialization;

namespace QCUniversidad.Api.Exceptions;

public class SubjectNotFoundException : Exception
{
    public SubjectNotFoundException()
    {
    }

    public SubjectNotFoundException(string? message) : base(message)
    {
    }

    public SubjectNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected SubjectNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
