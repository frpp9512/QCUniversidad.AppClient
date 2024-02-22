using MediatR;

namespace QCUniversidad.Api.Notifications.Models;

public class AllTeachersRecalculationRequested : INotification
{
    public Guid PeriodId { get; set; }
}
