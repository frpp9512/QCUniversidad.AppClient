namespace QCUniversidad.Api.Shared.Dtos.LoadItem;

public record EditLoadItemDto : NewLoadItemDto
{
    public Guid Id { get; set; }
}
