using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Notifications.Models;

namespace QCUniversidad.Api.Notifications.Handlers;

public class TeachersRecalculationOfDepartmentHandler(ITeachersLoadManager teachersLoadManager,
                                                      ILogger<TeachersRecalculationOfDepartmentHandler> logger) : INotificationHandler<TeachersRecalculationOfDepartmentRequested>
{
    private readonly ITeachersLoadManager _teachersLoadManager = teachersLoadManager;
    private readonly ILogger<TeachersRecalculationOfDepartmentHandler> _logger = logger;

    public async Task Handle(TeachersRecalculationOfDepartmentRequested notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Recalculation requested for teachers of department: {departmentId} in the period: {periodId}", notification.DepartmentId, notification.PeriodId);
        await _teachersLoadManager.RecalculateAllTeachersLoadOfDepartmentInPeriodAsync(notification.DepartmentId, notification.PeriodId);
    }
}
