using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Enums;

public enum ParticipationInProjectsOptions
{
    [Display(Name = "Participa", Description = "Tiene paricipación en proyectos de investigación en cualquiera de sus roles y modalidades.")]
    Participate,

    [Display(Name = "No participa", Description = "No participa en proyectos de investigación.")]
    DoNotParticipate
}
