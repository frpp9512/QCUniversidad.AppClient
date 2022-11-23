using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Faculties;
using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.Departments;

public class DepartmentModel
{
    public Guid Id { get; set; }

    [Required]
    [Display(Name = "Nombre", Prompt = "Nombre del departamento", Description = "El nombre del departamento")]
    public string Name { get; set; }

    [Display(Name = "Descripción", Prompt = "Descripción del departamento", Description = "La descripción del departamento.")]
    public string? Description { get; set; }

    [Display(Name = "Centro de estudio", Prompt = "Es centro de estudio", Description = "El departamento es un centro de estudio e investigación.")]
    public bool IsStudyCenter { get; set; }

    [Display(Name = "Identificador interno", Prompt = "Identificador interno", Description = "El identificador usado para la gestión interna de recursos humanos.")]
    public string? InternalId { get; set; }
    public int DisciplinesCount { get; set; }
    public Guid FacultyId { get; set; }
    public FacultyModel? Faculty { get; set; }
    public Guid[]? SelectedCareers { get; set; }
    public CareerModel[]? Careers { get; set; }
    public double? TotalTimeFund { get; set; }
    public double? Load { get; set; }
    public double? LoadPercent { get; set; }
    public double? LoadCovered { get; set; }
    public double? LoadCoveredPercent { get; set; }
}