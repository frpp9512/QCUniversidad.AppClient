using Microsoft.AspNetCore.Mvc;
using QCUniversidad.Api.Extensions;
using QCUniversidad.Api.Requests.Base.Models;

namespace QCUniversidad.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    private protected IActionResult GetResponseResult(RequestResponseBase response)
        => response.Error
            ? StatusCode(response.StatusCode.GetStatusCode(), response.ErrorMessages)
            : StatusCode(response.StatusCode.GetStatusCode(), response.GetPayload());

    private protected IActionResult GetCreatedResponseResult<TId, TEntity>(CreatedRequestResponseBase<TId, TEntity> response)
        => response.Error
            ? StatusCode(response.StatusCode.GetStatusCode(), response.Error)
            : (IActionResult)Created(Url.Action(response.ApiEntityEndpointAction, response.CreatedId), response.GetPayload());
}
