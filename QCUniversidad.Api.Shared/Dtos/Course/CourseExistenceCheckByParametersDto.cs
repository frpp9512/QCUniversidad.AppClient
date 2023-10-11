using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Shared.Dtos.Course;

public record CourseExistenceCheckByParametersDto
{
    public Guid CareerId { get; set; }
    public int CareerYear { get; set; }
    public TeachingModality Modality { get; set; }
}
