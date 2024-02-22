using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Enums;

public enum TeacherAdministrativeResponsibilities
{
    [Display(Name = "Ninguna", Description = "No tiene responsabilidades administrativas.", Prompt = "Ninguna", Order = 1)]
    None,

    [Display(Name = "Director de CUM", Description = "Director de un centro universitario municipal (CUM).", Prompt = "Director de CUM", Order = 6)]
    CUMDirector,

    [Display(Name = "Director de FUM", Description = "Director de una facultad universitaria municipal (FUM).", Prompt = "Director de FUM", Order = 5)]
    FUMDirector,

    [Display(Name = "Jefe de departamento", Description = "Jefe de departamento.", Prompt = "Jefe de departamento", Order = 4)]
    HeadOfDepartment,

    [Display(Name = "Coordinador de carrera", Description = "Coordinador de carrera.", Prompt = "Coordinador de carrera", Order = 2)]
    CareerCoordinator,

    [Display(Name = "Jefe de disciplina", Description = "Jefe de disciplina.", Prompt = "Jefe de disciplina", Order = 3)]
    HeadOfDiscipline,

    [Display(Name = "Rector", Description = "Rector.", Prompt = "Rector", Order = 12)]
    Rector,

    [Display(Name = "Vicerrector", Description = "Vicerrector.", Prompt = "Vicerrector", Order = 11)]
    ViceRector,

    [Display(Name = "Decano", Description = "Decano.", Prompt = "Decano", Order = 8)]
    Dean,

    [Display(Name = "Vicedecano", Description = "Vicedecano.", Prompt = "Vicedecano", Order = 7)]
    ViceDean,

    [Display(Name = "Director", Description = "Director.", Prompt = "Director", Order = 9)]
    Director,

    [Display(Name = "Director docente", Description = "Director docente.", Prompt = "Director docente", Order = 10)]
    TeachingDirector,
}