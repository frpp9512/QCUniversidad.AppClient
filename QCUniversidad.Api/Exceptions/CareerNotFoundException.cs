using System.Runtime.Serialization;

namespace QCUniversidad.Api.Exceptions;

public class CareerNotFoundException : Exception
{
    public CareerNotFoundException()
    {
    }

    public CareerNotFoundException(string? message) : base(message)
    {
    }

    public CareerNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected CareerNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
