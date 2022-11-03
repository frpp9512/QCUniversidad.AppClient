using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Enums;

public enum TeacherLoadStatus
{
    [Display(Name = "Subutilizado", Description = "La carga asignada al profesor es muy inferior a su capacidad encontrándose subutilizado. Se recomienda realizar reajustes de carga para su mejor aprovechamiento.")]
    Underutilized,

    [Display(Name = "Aceptable", Description = "La carga del profesor es aceptable pero aún tiene capacidad márgen para asumir más carga.")]
    Acceptable,

    [Display(Name = "Balanceado", Description = "La carga y capacidad del profesor se encuentran balanceadas.")]
    Balanced,

    [Display(Name = "Sobrecargado", Description = "La carga asignada al profesor excede su capacidad causando que esté sobrecargado. Se recomienda realizar reajustes de carga para un lograr un valor aceptable o balanceado.")]
    Overloaded
}