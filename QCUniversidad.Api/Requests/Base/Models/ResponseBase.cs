namespace QCUniversidad.Api.Requests.Base.Models;

public record ResponseBase
{
    public bool Success => !Error;
    public bool Error => ErrorMessages.Count > 0;
    public List<string> ErrorMessages { get; set; } = [];
}
