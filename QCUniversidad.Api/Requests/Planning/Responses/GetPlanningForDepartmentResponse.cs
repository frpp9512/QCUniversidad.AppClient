using QCUniversidad.Api.Requests.Base.Models;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;

namespace QCUniversidad.Api.Requests.Planning.Responses;

public record GetPlanningForDepartmentResponse : RequestResponseBase
{
    public List<TeachingPlanItemDto>? PlanningItems { get; set; }

    public override object? GetPayload() => PlanningItems;
}
