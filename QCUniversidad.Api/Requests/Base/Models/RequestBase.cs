using MediatR;

namespace QCUniversidad.Api.Requests.Base.Models;

public class RequestBase<TResponse> : IRequest<TResponse>
{
    public Guid RequestId { get; set; } = Guid.NewGuid();
}
