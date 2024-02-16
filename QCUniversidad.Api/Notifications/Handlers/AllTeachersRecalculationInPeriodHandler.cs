using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Notifications.Models;

namespace QCUniversidad.Api.Notifications.Handlers;

public class AllTeachersRecalculationInPeriodHandler(ITeachersLoadManager teachersLoadManager,
                                                     ILogger<AllTeachersRecalculationInPeriodHandler> logger) : INotificationHandler<AllTeachersRecalculationRequested>
{
    private readonly ITeachersLoadManager _teachersLoadManager = teachersLoadManager;
    private readonly ILogger<AllTeachersRecalculationInPeriodHandler> _logger = logger;

    public async Task Handle(AllTeachersRecalculationRequested notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Recalculation requested for all the teachers in period: {periodId}", notification.PeriodId);
        await _teachersLoadManager.RecalculateAllTeachersLoadInPeriodAsync(notification.PeriodId);
    }
}
