using System.Runtime.Serialization;

namespace QCUniversidad.Api.Exceptions;

public class DatabaseOperationException : Exception
{
    public DatabaseOperationException()
    {
    }

    public DatabaseOperationException(string? message) : base(message)
    {
    }

    public DatabaseOperationException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected DatabaseOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
