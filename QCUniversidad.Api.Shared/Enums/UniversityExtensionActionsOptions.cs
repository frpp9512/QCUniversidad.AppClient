using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Enums;

public enum UniversityExtensionActionsOptions
{
    [Display(Name ="Alta", Description = "Alta participación en actividades de extensión universitaria.")]
    High,

    [Display(Name = "Medio", Description = "Media participación en actividades de extensión universitaria.")]
    Medium,

    [Display(Name = "Baja", Description = "Baja participación en actividades de extensión universitaria.")]
    Low
}
