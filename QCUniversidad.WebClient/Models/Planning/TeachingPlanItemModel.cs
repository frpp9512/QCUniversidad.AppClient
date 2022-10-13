﻿using QCUniversidad.Api.Shared.Enums;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Models.Subjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QCUniversidad.WebClient.Models.Planning;

public class TeachingPlanItemModel
{
    public Guid Id { get; set; }
    public Guid SubjectId { get; set; }

    [Display(Name = "Asignatura", Description = "Asignatura planificada", Prompt = "Asignatura planificada")]
    public SubjectModel? Subject { get; set; }

    [Display(Name = "Actividad", Description = "Actividad planificada", Prompt = "Tipo de activdad")]
    public TeachingActivityType Type { get; set; }

    [Display(Name = "Cantidad de horas", Description = "Horas planificadas", Prompt = "Cantidad de horas planificadas")]
    [Range(0, 2287.2)]
    public double HoursPlanned { get; set; }

    [Display(Name = "Grupos", Description = "Cantidad de grupos involucrados en la actividad", Prompt = "Cantidad de grupos")]
    public uint GroupsAmount { get; set; }

    [NotMapped]
    [Display(Name = "Horas planificadas", Description = "Total de horas planificadas", Prompt = "Cantidad de horas planificadas")]
    public double TotalHoursPlanned => HoursPlanned * GroupsAmount;

    public Guid PeriodId { get; set; }
    public PeriodModel Period { get; set; }
}