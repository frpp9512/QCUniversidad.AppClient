using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Enums;

public enum EducationalWorkType
{
    [Display(Name = "Profesor principal de año", Description = "Encargado de coordinar y controlar todas las actividades docentes y extradocentes de los estudiantes de curso regular diurno (CRD).")]
    HeadTeacherOfTheYear,

    [Display(Name = "Profesor guía", Description = "Encargado de coordinar y controlar todas las actividades docentes de los estudiantes de un grupo de curso regular diurno (CRD).")]
    GuideTeacher,

    [Display(Name = "Coordinador de año", Description = "Encargado de realizar todas las actividades docentes de los estudiantes de curso por encuentro (CPE).")]
    YearCoordinator,

    [Display(Name = "Profesor", Description = "Realiza atención directa con los estudiantes en función de las actividades docentes.")]
    Teacher,

    [Display(Name = "Coordinador de maestría", Description = "Encargado de coordinar y controlar las actividades docentes y extradocentes de los cursos de maestrías.")]
    MastersCoordinator,

    [Display(Name = "Coordinador de doctorado", Description = "Encargado de coordinar y controlar las actividades docentes y extradocentes de los cursos de doctorado.")]
    DoctorateCoordinator
}