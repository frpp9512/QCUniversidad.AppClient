﻿using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Enums;

public enum TeacherContractType
{
    /// <summary>
    /// A full time teacher
    /// </summary>
    [Display(Name = "Tiempo completo", Description = "Profesor a tiempo completo", Prompt = "Tiempo completo")]
    FullTime,

    /// <summary>
    /// A part-time teacher
    /// </summary>
    [Display(Name = "Tiempo parcial", Description = "Profesor a tiempo parcial", Prompt = "Tiempo parcial")]
    PartTime,

    /// <summary>
    /// An assistant teacher
    /// </summary>
    [Display(Name = "Colaborador", Description = "Profesor colaborador", Prompt = "Profesor colaborador")]
    Collaborator
}