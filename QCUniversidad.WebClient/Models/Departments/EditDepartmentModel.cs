using QCUniversidad.WebClient.Models.Careers;
using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.Departments;

public class EditDepartmentModel
{
    public Guid Id { get; set; }

    [Required]
    [Display(Name = "Nombre", Prompt = "Nombre del departamento", Description = "El nombre del departamento")]
    public string? Name { get; set; }

    [Display(Name = "Descripción", Prompt = "Descripción del departamento", Description = "La descripción del departamento.")]
    public string? Description { get; set; }

    [Display(Name = "Centro de estudio", Prompt = "Es centro de estudio", Description = "El departamento es un centro de estudio e investigación.")]
    public bool IsStudyCenter { get; set; }

    [Required]
    [Display(Name = "Identificador interno", Prompt = "Identificador interno", Description = "El identificador usado para la gestión interna de recursos humanos.")]
    public string? InternalId { get; set; }
    public IList<CareerModel>? Careers { get; set; }

    [Display(Name = "Carreras que gestiona", Prompt = "Carreras que gestiona", Description = "Carreras que gestiona el departamento.")]
    public Guid[]? SelectedCareers { get; set; }
    public Guid FacultyId { get; set; }
    public string? FacultyName { get; set; }
}