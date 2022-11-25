namespace QCUniversidad.Api.Shared.Dtos.TeachingPlan;

public record EditTeachingPlanItemDto : NewTeachingPlanItemDto
{
    public Guid Id { get; set; }
}
