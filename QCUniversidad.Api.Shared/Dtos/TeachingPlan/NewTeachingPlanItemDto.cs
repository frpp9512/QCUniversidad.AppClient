using QCUniversidad.Api.Shared.Enums;

namespace QCUniversidad.Api.Shared.Dtos.TeachingPlan
{
    public record NewTeachingPlanItemDto
    {
        public Guid SubjectId { get; set; }
        public Guid CourseId { get; set; }
        public TeachingActivityType Type { get; set; }
        public double HoursPlanned { get; set; }
        public uint GroupsAmount { get; set; }
        public double TotalHoursPlanned { get; set; }
        public bool IsNotLoadGenerator { get; set; }
        public Guid PeriodId { get; set; }
    }
}
