using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Shared.Dtos.Teacher;

public record NewTeacherDto
{
    public string? Fullname { get; set; }
    public string? PersonalId { get; set; }
    public string? Position { get; set; }
    public TeacherCategory Category { get; set; }
    public TeacherContractType ContractType { get; set; }
    public double SpecificTimeFund { get; set; }
    public string? Email { get; set; }
    public bool ServiceProvider { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid[]? SelectedDisciplines { get; set; }
}
