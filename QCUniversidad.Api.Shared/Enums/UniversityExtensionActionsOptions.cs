using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Enums;

public enum UniversityExtensionActionsOptions
{
    [Display(Name = "Alta", Description = "Liderazgo, organización, participación y promoción de actividades comunitarias y extracurriculares.")]
    High,

    [Display(Name = "Media", Description = "Participación, apoyo y promoción de actividades comunitarias y extracurriculares.")]
    Medium,

    [Display(Name = "Baja", Description = "Promoción y apoyo de actividades comunitarias y extracurriculares.")]
    Low
}
