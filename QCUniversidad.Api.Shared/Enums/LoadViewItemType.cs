using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.Api.Shared.Enums;

public enum LoadViewItemType
{
    /// <summary>
    /// A load associated with the teaching activity of a specific subject.
    /// </summary>
    [Display(Name = "Carga directa")]
    Teaching,

    /// <summary>
    /// A load associated with non teaching activities like self imporvement and class preparation.
    /// </summary>
    [Display(Name = "Carga indirecta")]
    NonTeaching
}
