using System.Runtime.Serialization;

namespace QCUniversidad.Api.Exceptions;

public class NotCurrentSchoolYearDefined : Exception
{
    public NotCurrentSchoolYearDefined()
    {
    }

    public NotCurrentSchoolYearDefined(string? message) : base(message)
    {
    }

    public NotCurrentSchoolYearDefined(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected NotCurrentSchoolYearDefined(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
