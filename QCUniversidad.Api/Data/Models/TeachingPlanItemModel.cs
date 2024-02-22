using QCUniversidad.Api.Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace QCUniversidad.Api.Data.Models;

public record TeachingPlanItemModel
{
    public Guid Id { get; set; }
    public Guid SubjectId { get; set; }

    public SubjectModel? Subject { get; set; }

    public TeachingActivityType Type { get; set; }

    public double HoursPlanned { get; set; }

    public uint GroupsAmount { get; set; }

    [NotMapped]
    public double TotalHoursPlanned { get; set; }

    public bool FromPostgraduateCourse { get; set; }

    public bool IsNotLoadGenerator { get; set; }

    public Guid PeriodId { get; set; }
    public required PeriodModel Period { get; set; }

    public Guid CourseId { get; set; }
    public required CourseModel Course { get; set; }

    public required IList<LoadItemModel> LoadItems { get; set; }
}