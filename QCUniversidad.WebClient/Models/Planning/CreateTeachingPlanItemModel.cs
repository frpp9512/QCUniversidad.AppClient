using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using QCUniversidad.WebClient.Models.Subjects;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Models.Courses;
using Microsoft.CodeAnalysis.Diagnostics;

namespace QCUniversidad.WebClient.Models.Planning;

public class CreateTeachingPlanItemModel
{
    [Required]
    [Display(Name = "Curso", Description = "Curso a planificar", Prompt = "Curso a planfificar")]
    public Guid CourseId { get; set; }

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

    [Required]
    public Guid PeriodId { get; set; }
    public PeriodModel? Period { get; set; }

    public IList<CourseModel>? Courses { get; set; }
}