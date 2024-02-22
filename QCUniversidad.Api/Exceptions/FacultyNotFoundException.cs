using System.Runtime.Serialization;

namespace QCUniversidad.Api.Exceptions;

public class FacultyNotFoundException : Exception
{
    public FacultyNotFoundException()
    {
    }

    public FacultyNotFoundException(string? message) : base(message)
    {
    }

    public FacultyNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected FacultyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
