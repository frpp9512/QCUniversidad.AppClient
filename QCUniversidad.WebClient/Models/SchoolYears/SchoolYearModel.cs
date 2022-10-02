using QCUniversidad.Api.Shared.Dtos.Period;
using QCUniversidad.Api.Shared.Enums;
using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Curriculums;
using QCUniversidad.WebClient.Models.Periods;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.SchoolYears;

/// <summary>
/// A set of periods where will be taught a set of subjects.
/// </summary>
public record SchoolYearModel
{
    /// <summary>
    /// Primary key value.
    /// </summary>
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Debe de especifica que año de la carrera comprende el año escolar.")]
    [Display(Name = "Año de la carrera", Description = "El año de la carrera al cual pertenece el presenta año escolar", Prompt = "Ej. '3' para 3er año")]
    [Range(1, 8, ErrorMessage = "El año de la carrera debe de ser mayor que 1 y no mayor que 8.")]
    public int CareerYear { get; set; } = 1;

    [Required(ErrorMessage = "Debe de especificar una denominación para el año escolar")]
    [Display(Name = "Denominación del año escolar", Description = "Como se va a denominar el año escolar.", Prompt = "Ej. '2022-2023'")]
    public string Denomination { get; set; }

    [Required(ErrorMessage = "Debe de especificar la fecha de inicio del año escolar")]
    [Display(Name = "Fecha de inicio", Description = "Fecha de inicio del año escolar", Prompt = "Ej. 3/9/2022")]
    public DateTimeOffset Starts { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Debe de especificar la fecha de culminación del año escolar")]
    [Display(Name = "Fecha de culminación", Description = "Fecha de culminación del año escolar", Prompt = "Ej. 12/7/2022")]
    public DateTimeOffset Ends { get; set; } = DateTime.Now.AddMonths(9);

    [Required(ErrorMessage = "Debe de especificar la modalidad de estudio del año escolar.")]
    [Display(Name = "Modalidad de estudio", Description = "La modalidad de estudio que cursan los estudiantes en el año escolar", Prompt = "Ej. 'Presencial'")]
    public TeachingModality TeachingModality { get; set; }

    public Guid CareerId { get; set; }

    public CareerModel? Career { get; set; }

    public Guid CurriculumId { get; set; }

    public CurriculumModel? Curriculum { get; set; }

    public IList<PeriodModel>? Periods { get; set; }
}