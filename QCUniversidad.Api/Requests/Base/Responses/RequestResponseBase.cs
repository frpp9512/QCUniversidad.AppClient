﻿using System.Net;

namespace QCUniversidad.Api.Requests.Base.Models;

public abstract record RequestResponseBase
{
    public required Guid RequestId { get; set; }
    public bool Success => !Error;
    public bool Error => ErrorMessages.Count > 0;
    public List<string> ErrorMessages { get; set; } = [];
    public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    public abstract object? GetPayload();
}