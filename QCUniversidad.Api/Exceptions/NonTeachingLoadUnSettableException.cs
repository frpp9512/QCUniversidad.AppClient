using System.Runtime.Serialization;

namespace QCUniversidad.Api.Exceptions;

public class NonTeachingLoadUnSettableException : Exception
{
    public NonTeachingLoadUnSettableException()
    {
    }

    public NonTeachingLoadUnSettableException(string? message) : base(message)
    {
    }

    public NonTeachingLoadUnSettableException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NonTeachingLoadUnSettableException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
