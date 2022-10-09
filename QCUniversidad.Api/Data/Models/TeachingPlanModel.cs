using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Data.Models;

public class TeachingPlanModel
{
    /// <summary>
    /// Primary key value.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The period to plan.
    /// </summary>
    public PeriodModel Period { get; set; }

    /// <summary>
    /// The id of the period to plan.
    /// </summary>
    public Guid PeriodId { get; set; }

    /// <summary>
    /// The set of school activities planned.
    /// </summary>
    public IList<TeachingPlanItem> Items { get; set; }

    public int ItemsCount { get; set; }
    public int TotalHours { get; set; }
}