using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.Disciplines;

public record EditDisciplineModel : CreateDisciplineModel
{
    public new Guid Id { get; set; }

    [Required]
    [Display(Name = "Nombre", Prompt = "Nombre de la disciplina", Description = "El nombre de la disciplina")]
    public new required string Name { get; set; }

    [Display(Name = "Disciplina", Prompt = "Descripción de la disciplina", Description = "La descripción de la disciplina")]
    public new string? Description { get; set; }
    public new Guid DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
}
