using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Enums;

public enum TeacherAdministrativeResponsibilities
{
    [Display(Name = "Ninguna", Description = "No tiene responsabilidades administrativas.", Prompt = "Ninguna")]
    None,

    [Display(Name = "Director de CUM", Description = "Director de un centro universitario municipal (CUM).", Prompt = "Director de CUM")]
    CUMDirector,

    [Display(Name = "Director de FUM", Description = "Director de una facultad universitaria municipal (FUM).", Prompt = "Director de FUM")]
    FUMDirector,

    [Display(Name = "Jefe de departamento", Description = "Jefe de departamento.", Prompt = "Jefe de departamento")]
    HeadOfDepartment,

    [Display(Name = "Coordinador de carrera", Description = "Coordinador de carrera.", Prompt = "Coordinador de carrera")]
    CareerCoordinator,

    [Display(Name = "Jefe de disciplina", Description = "Jefe de disciplina.", Prompt = "Jefe de disciplina")]
    HeadOfDiscipline,
}