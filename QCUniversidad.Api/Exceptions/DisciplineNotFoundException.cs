using System.Runtime.Serialization;

namespace QCUniversidad.Api.Exceptions;

public class DisciplineNotFoundException : Exception
{
    public DisciplineNotFoundException()
    {
    }

    public DisciplineNotFoundException(string? message) : base(message)
    {
    }

    public DisciplineNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DisciplineNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
