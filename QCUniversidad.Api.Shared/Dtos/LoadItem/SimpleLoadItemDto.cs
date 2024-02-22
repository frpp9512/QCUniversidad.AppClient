using QCUniversidad.Api.Shared.Dtos.Teacher;

namespace QCUniversidad.Api.Shared.Dtos.LoadItem;

public record SimpleLoadItemDto : EditLoadItemDto
{
    public required TeacherDto Teacher { get; set; }
}
