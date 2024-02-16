using MediatR;

namespace QCUniversidad.Api.Notifications.Models;

public class TeacherRecalculationRequested : INotification
{
    public Guid TeacherId { get; set; }
    public Guid PeriodId { get; set; }
}
