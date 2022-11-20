using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Enums;

public enum TeacherAdquiredResponsabilities
{
    [Display(Name = "Ninguna", Description = "No posee responsabilidades adquiridas.", Prompt = "Ninguna")]
    None,

    [Display(Name = "Representante de proceso a nivel de área", Description = "Representante algún proceso del área donde pertenece.", Prompt = "Representante de proceso a nivel de área")]
    AreaProcessRepresentative,

    [Display(Name = "Representante de proceso a nivel de centro", Description = "Representante algún proceso del centro donde pertenece.", Prompt = "Representante de proceso a nivel de centro")]
    InstitutionProcessRepresentative,
}