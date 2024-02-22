using QCUniversidad.WebClient.Models.Departments;
using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.Disciplines;

public record DisciplineModel
{
    public Guid Id { get; set; }

    [Required]
    [Display(Name = "Nombre", Prompt = "Nombre de la disciplina", Description = "El nombre de la disciplina")]
    public string? Name { get; set; }

    [Display(Name = "Disciplina", Prompt = "Descripción de la disciplina", Description = "La descripción de la disciplina")]
    public string? Description { get; set; }
    public int? TeachersCount { get; set; }
    public int? SubjectsCount { get; set; }
    public Guid DepartmentId { get; set; }
    public DepartmentModel? Department { get; set; }
    public DisciplineImportAction? ImportAction { get; set; }
}
