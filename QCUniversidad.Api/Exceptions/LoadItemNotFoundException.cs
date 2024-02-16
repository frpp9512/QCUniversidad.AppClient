using System.Runtime.Serialization;

namespace QCUniversidad.Api.Exceptions;

public class LoadItemNotFoundException : Exception
{
    public LoadItemNotFoundException()
    {
    }

    public LoadItemNotFoundException(string? message) : base(message)
    {
    }

    public LoadItemNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected LoadItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
