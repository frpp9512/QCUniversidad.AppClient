using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.Faculties;

public class FacultyModel
{
    public Guid Id { get; set; }

    [Required]
    [Display(Name = "Nombre", Prompt = "Nombre de la facultad", Description = "El nombre de la facultad")]
    public required string Name { get; set; }

    [Required]
    [Display(Name = "Sede", Prompt = "Sede universitaria", Description = "El nombre de la sede universitaria a la cual pertenece la facultad.")]
    public required string Campus { get; set; }

    [Required]
    [Display(Name = "Identificador interno", Prompt = "Identificador interno", Description = "El identificador usado para la gestión interna de recursos humanos.")]
    public required string InternalId { get; set; }

    public int DepartmentCount { get; set; } = 0;
    public int CareersCount { get; set; } = 0;
}
