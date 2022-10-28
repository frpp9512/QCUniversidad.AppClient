using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Shared.Enums;

public enum LoadViewItemType
{
    /// <summary>
    /// A load associated with the teaching activity of a specific subject.
    /// </summary>
    Teaching,

    /// <summary>
    /// A load associated with non teaching activities like self imporvement and class preparation.
    /// </summary>
    NonTeaching
}
