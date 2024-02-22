using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Enums;

public enum UniversityExtensionActionsOptions
{
    [Display(Name = "Alta", Description = "Liderazgo, organización, participación y promoción de actividades comunitarias y extracurriculares.", Order = 4)]
    High,

    [Display(Name = "Media", Description = "Participación, apoyo y promoción de actividades comunitarias y extracurriculares.", Order = 3)]
    Medium,

    [Display(Name = "Baja", Description = "Promoción y apoyo de actividades comunitarias y extracurriculares.", Order = 2)]
    Low,

    [Display(Name = "Ninguna", Description = "No está involucrado en actividades de extensión universitaria. Esta opción solo debe tomarse en cuenta para profesores a tiempo parcial.", Order = 1)]
    None
}