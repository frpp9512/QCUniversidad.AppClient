using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Enums;

public enum CoursesReceivedAndImprovementOptions
{
    [Display(Name = "Iniciante", Description = "Profesor con poco tiempo en el ejercicio docente. Se hace necesario más tiempo para autoprepararse y particiar en cursos de superación.")]
    Beginner,

    [Display(Name = "Promedio", Description = "Profesor ya con experiencia y preparación docente. Necesita un tiempo moderado para su autopreaparación y participación en cursos de superación.")]
    Average,

    [Display(Name = "Experimentado", Description = "Profesor con basta experiencia y alta preparación docente. El tiempo necesitado para la autopreparación y participación en curso de superación es mínimo.")]
    Experienced
}
