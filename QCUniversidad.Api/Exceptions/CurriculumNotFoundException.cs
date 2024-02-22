using System.Runtime.Serialization;

namespace QCUniversidad.Api.Exceptions;

public class CurriculumNotFoundException : Exception
{
    public CurriculumNotFoundException()
    {
    }

    public CurriculumNotFoundException(string? message) : base(message)
    {
    }

    public CurriculumNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected CurriculumNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
