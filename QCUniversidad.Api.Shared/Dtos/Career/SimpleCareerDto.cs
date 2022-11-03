using QCUniversidad.Api.Shared.Dtos.Faculty;

namespace QCUniversidad.Api.Shared.Dtos.Career;

public record SimpleCareerDto : EditCareerDto
{
    public FacultyDto? Faculty { get; set; }
}