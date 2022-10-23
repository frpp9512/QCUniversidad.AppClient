using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    [Display(Name = "Asistente", Description = "Profesor asistente", Prompt = "Profesor asistente")]
    Assistant
}