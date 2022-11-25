using QCUniversidad.Api.Shared.Enums;
using QCUniversidad.WebClient.Models.Courses;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Models.Subjects;
using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.Planning;

public class EditTeachingPlanItemModel
{
    public Guid Id { get; set; }

    [Required]
    [Display(Name = "Asignatura", Description = "Asignatura planificada", Prompt = "Asignatura planificada")]
    public Guid SubjectId { get; set; }

    [Required]
    [Display(Name = "Actividad", Description = "Actividad planificada", Prompt = "Tipo de activdad")]
    public TeachingActivityType Type { get; set; }

    [Required]
    [Display(Name = "Cantidad de horas", Description = "Horas planificadas", Prompt = "Cantidad de horas planificadas")]
    [Range(0, 2287.2)]
    public double HoursPlanned { get; set; }

    [Display(Name = "Grupos", Description = "Cantidad de grupos involucrados en la actividad", Prompt = "Cantidad de grupos")]
    public uint GroupsAmount { get; set; }

    [Display(Name = "Práctica profesional o trabajo de culminación de curso o carrera", Description = "Define si la actividad es una actividad que da cierre al año o carrera, o en su defecto es una activdad de práctica profesional.", Prompt = "Práctica profesional o trabajo de culminación de curso o carrera")]
    public bool IsNotLoadGenerator { get; set; }

    [Required]
    public Guid PeriodId { get; set; }

    public PeriodModel? Period { get; set; }

    [Required]
    public Guid CourseId { get; set; }
    public CourseModel? Course { get; set; }

    public IList<SubjectModel>? Subjects { get; set; }
}
