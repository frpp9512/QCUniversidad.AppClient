using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Enums;

public enum SubjectTerminationMode
{
    [Display(Name = "Exámen final", Description = "La asignatura será evaluada mediante un exámen escrito.")]
    FinalExam,

    [Display(Name = "Trabajo de curso", Description = "La asignatura se evaluará mediante un trabajo investigativo.")]
    CourseWork,

    [Display(Name = "Recorrido", Description = "La asignatura se evaluará mediante el recorrido académico del estudiante en la asignatura.")]
    AcademicHistory
}
