using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Notifications.Models;

namespace QCUniversidad.Api.Notifications.Handlers;

public class TeacherRecalculationHandler(ITeachersLoadManager teachersLoadManager,
                                         ILogger<TeacherRecalculationHandler> logger) : INotificationHandler<TeacherRecalculationRequested>
{
    private readonly ITeachersLoadManager _teachersLoadManager = teachersLoadManager;
    private readonly ILogger<TeacherRecalculationHandler> _logger = logger;

    public async Task Handle(TeacherRecalculationRequested notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Requested recalculation for teacher: {teacherId} in the period: {periodId}", notification.TeacherId, notification.PeriodId);
        await _teachersLoadManager.RecalculateAutogenerateTeachingLoadItemsAsync(notification.TeacherId, notification.PeriodId);
    }
}
