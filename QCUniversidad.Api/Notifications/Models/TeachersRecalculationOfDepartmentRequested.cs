using MediatR;

namespace QCUniversidad.Api.Notifications.Models;

public class TeachersRecalculationOfDepartmentRequested : INotification
{
    public Guid DepartmentId { get; set; }
    public Guid PeriodId { get; set; }
}
