using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Enums;

public enum TeacherSyndicalAndPoliticalResposabilities
{
    [Display(Name = "Ninguna", Description = "No posee responsabilidades sindicales o políticas.", Prompt = "Ninguna")]
    None,

    [Display(Name = "Representante sindical de base", Description = "Representante de la actividad sindical de base en su sección.", Prompt = "Representante sindical de base")]
    SyndicalBaseRepresentative,

    [Display(Name = "Representante sindical general", Description = "Representante de la actividad sindical general del centro.", Prompt = "Representante sindical general")]
    SyndicalGeneralRepresentative,

    [Display(Name = "Secretario de comité de base UJC", Description = "Secretario del comité de base de la Unión de Jovenes Comunistas (UJC).", Prompt = "Secretario de comité de base UJC")]
    UJCBaseRepresentative,

    [Display(Name = "Secretario de comité UJC", Description = "Secretario del comité de la Unión de Jovenes Comunistas (UJC).", Prompt = "Secretario de comité UJC")]
    UJCComiteeRepresentative,

    [Display(Name = "Secretario de núcleo PCC", Description = "Secretario de núcleo del Partido Comunista de Cuba (PCC).", Prompt = "Secretario de núcleo PCC")]
    PCCBaseRepresentative,

    [Display(Name = "Secretario de comité PCC", Description = "Secretario del comité del Partido Comunista de Cuba (PCC).", Prompt = "Secretario de comité PCC")]
    PCCComiteeRepresentative
}