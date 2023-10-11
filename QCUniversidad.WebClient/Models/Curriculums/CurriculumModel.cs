using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Disciplines;
using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.Curriculums;

public class CurriculumModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Debe de especificar una denominación para el curriculum.")]
    [Display(Name = "Denominación", Prompt = "Ej. Plan E", Description = "La denominación que recibe el curriculum.")]
    public required string Denomination { get; set; }

    [Display(Name = "Descripción", Prompt = "Ej. Plan de estudio acelerado para ingeniería industrial", Description = "La denominación que recibe el curriculum.")]
    public string? Description { get; set; }
    public Guid CareerId { get; set; }
    public CareerModel? Career { get; set; }
    public IList<DisciplineModel>? CurriculumDisciplines { get; set; }
    public Guid[]? SelectedDisciplines { get; set; }
}