namespace QCUniversidad.Api.Shared.Dtos.Curriculum;

public record NewCurriculumDto
{
    public required string Denomination { get; set; }
    public string? Description { get; set; }
    public Guid CareerId { get; set; }
    public required Guid[] SelectedDisciplines { get; set; }
}