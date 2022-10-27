using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Data.Models;

public record NonTeachingLoadModel
{
    /// <summary>
    /// The primary key value.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The id of the period of the when the load is assigned.
    /// </summary>
    public Guid PeriodId { get; set; }

    /// <summary>
    /// The period of the when the load is assigned.
    /// </summary>
    public PeriodModel Period { get; set; }

    /// <summary>
    /// The id of the teacher whom the load is assigned to.
    /// </summary>
    public Guid TeacherId { get; set; }

    /// <summary>
    /// The teacher whom the load is assigned to.
    /// </summary>
    public TeacherModel Teacher { get; set; }

    /// <summary>
    /// The type of the load.
    /// </summary>
    public NonTeachingLoadType Type { get; set; }

    /// <summary>
    /// The value used for the calculation of the load.
    /// </summary>
    public string BaseValue { get; set; }

    /// <summary>
    /// The value of the load assigned to the teacher.
    /// </summary>
    public double Load { get; set; }

    /// <summary>
    /// The description of the load.
    /// </summary>
    public string Description { get; set; }
}
