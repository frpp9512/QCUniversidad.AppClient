using QCUniversidad.Api.Shared.Dtos.Teacher;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;

namespace QCUniversidad.Api.Shared.Dtos.LoadItem;

public record LoadItemDto : EditLoadItemDto
{
    public required TeachingPlanItemSimpleDto PlanningItem { get; set; }
    public required TeacherDto Teacher { get; set; }
}
