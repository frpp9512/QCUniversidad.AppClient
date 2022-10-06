using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata;
using QCUniversidad.Api.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.Data.Models;

public class TeachingPlanItem
{
    /// <summary>
    /// The primary key identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The id of the subject planned
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// The subject planned.
    /// </summary>
    public SubjectModel? Subject { get; set; }

    /// <summary>
    /// The type of school activity performed.
    /// </summary>
    public TeachingActivityType Type { get; set; }

    /// <summary>
    /// The amount of hours planned for the activity.
    /// </summary>
    [Range(0, 2287.2)]
    public double HoursPlanned { get; set; }

    /// <summary>
    /// The amount of groups planned to recieve the school activity.
    /// </summary>
    public uint GroupsAmount { get; set; }

    /// <summary>
    /// The total hours planned in function of amount of groups.
    /// </summary>
    public double TotalHoursPlanned => HoursPlanned * GroupsAmount;

    /// <summary>
    /// The id of the teaching plan.
    /// </summary>
    public Guid TeachingPlanId { get; set; }

    /// <summary>
    /// The teaching plan.
    /// </summary>
    public TeachingPlanModel TeachingPlan { get; set; }
}