using QCUniversidad.WebClient.Models.Courses;
using QCUniversidad.WebClient.Models.Periods;
using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.SchoolYears;

public record SchoolYearModel
{
    public Guid Id { get; set; }

    [Required]
    [Display(Name = "Nombre", Prompt = "Nombre del año escolar", Description = "El nombre del año escolar")]
    public string Name { get; set; }

    [Display(Name = "Descripción", Prompt = "Descripción del año escolar", Description = "Descripción de año escolar")]
    public string? Description { get; set; }

    [Display(Name = "Año actual", Prompt = "Año escolar actual", Description = "Define si es el año escolar actual.")]
    public bool Current { get; set; }

    public IList<CourseModel>? Courses { get; set; }
    public int? CoursesCount { get; set; }

    public IList<PeriodModel>? Periods { get; set; }
    public int? PeriodsCount { get; set; }
}