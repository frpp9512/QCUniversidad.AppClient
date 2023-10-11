namespace QCUniversidad.Api.Shared.Dtos.Period;

public record EditPeriodDto : NewPeriodDto
{
    public Guid Id { get; set; }
}
