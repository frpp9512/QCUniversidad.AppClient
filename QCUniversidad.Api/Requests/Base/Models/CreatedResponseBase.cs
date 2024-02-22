namespace QCUniversidad.Api.Requests.Base.Models;

public abstract record CreatedResponseBase<TId, TEntity> : ResponseBase
{
    public TId CreatedId { get; set; } = default!;
    public TEntity? CreatedEntity { get; set; }
    public string? ApiEntityEndpointAction { get; set; }

    public override object? GetPayload() => CreatedEntity;
}
