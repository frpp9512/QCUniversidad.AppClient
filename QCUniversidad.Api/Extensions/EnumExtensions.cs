using System.Net;

namespace QCUniversidad.Api.Extensions;

public static class EnumExtensions
{
    public static int GetStatusCode(this HttpStatusCode statusCode) => (int)statusCode;
}
