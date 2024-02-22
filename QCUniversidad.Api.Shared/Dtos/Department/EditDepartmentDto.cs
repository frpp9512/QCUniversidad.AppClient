namespace QCUniversidad.Api.Shared.Dtos.Department;

public record EditDepartmentDto : NewDepartmentDto
{
    public Guid Id { get; set; }
}