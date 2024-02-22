using System.Runtime.Serialization;

namespace QCUniversidad.Api.Exceptions;

public class SchoolYearNotFoundException : Exception
{
    public SchoolYearNotFoundException()
    {
    }

    public SchoolYearNotFoundException(string? message) : base(message)
    {
    }

    public SchoolYearNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected SchoolYearNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
