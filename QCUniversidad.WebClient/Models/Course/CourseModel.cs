using QCUniversidad.Api.Shared.Dtos.Period;
using QCUniversidad.Api.Shared.Enums;
using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Curriculums;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Models.SchoolYears;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Courses;

/// <summary>
/// A set of periods where will be taught a set of subjects.
/// </summary>
public record CourseModel
{
    /// <summary>
    /// Primary key value.
    /// </summary>
    public Guid Id { get; set; }

    public Guid SchoolYearId { get; set; }

    public SchoolYearModel? SchoolYear { get; set; }

    [Required(ErrorMessage = "Debe de especifica que año de la carrera comprende el curso.")]
    [Display(Name = "Año de la carrera", Description = "El año de la carrera al cual pertenece el presenta curso", Prompt = "Ej. '3' para 3er año")]
    [Range(1, 8, ErrorMessage = "El año de la carrera debe de ser mayor que 1 y no mayor que 8.")]
    public int CareerYear { get; set; } = 1;

    [Required(ErrorMessage = "Debe de especificar una denominación para el curso")]
    [Display(Name = "Denominación del curso", Description = "Como se va a denominar el curso.", Prompt = "Ej. '2022-2023'")]
    public string Denomination { get; set; }

    [Required(ErrorMessage = "Debe de especificar la modalidad de estudio del curso.")]
    [Display(Name = "Modalidad de estudio", Description = "La modalidad de estudio que cursan los estudiantes en el curso", Prompt = "Ej. 'Presencial'")]
    public TeachingModality TeachingModality { get; set; }

    [Required(ErrorMessage = "Debe de especificar la matrícula del estudio del curso.")]
    [Display(Name = "Matrícula de estudiantes", Description = "La cantodad de estudiantes que participan en el curso.", Prompt = "Matrícula")]
    public uint Enrolment { get; set; }

    public Guid CareerId { get; set; }

    public CareerModel? Career { get; set; }

    public Guid CurriculumId { get; set; }

    public CurriculumModel? Curriculum { get; set; }
}