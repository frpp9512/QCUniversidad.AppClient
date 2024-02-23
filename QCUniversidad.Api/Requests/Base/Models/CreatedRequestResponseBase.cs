namespace QCUniversidad.Api.Requests.Base.Models;

public abstract record CreatedRequestResponseBase<TId, TEntity> : RequestResponseBase
{
    public TId CreatedId { get; set; } = default!;
    public TEntity? CreatedEntity { get; set; }
    public string? ApiEntityEndpointAction { get; set; }

    public override object? GetPayload() => CreatedEntity;
}
