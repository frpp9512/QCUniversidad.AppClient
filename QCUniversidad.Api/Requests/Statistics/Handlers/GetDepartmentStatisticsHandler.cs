using MediatR;
using Microsoft.Extensions.Options;
using QCUniversidad.Api.ConfigurationModels;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Statistics.Models;
using QCUniversidad.Api.Requests.Statistics.Responses;
using QCUniversidad.Api.Shared.Dtos.Statistics;

namespace QCUniversidad.Api.Requests.Statistics.Handlers;

public class GetDepartmentStatisticsHandler(IPeriodsManager periodsManager, ITeachersManager teachersManager, IDepartmentsManager departmentsManager, IOptions<CalculationOptions> calcOptions) : IRequestHandler<GetDepartmentStatisticsRequest, GetDepartmentStatisticsResponse>
{
    private readonly IPeriodsManager _periodsManager = periodsManager;
    private readonly ITeachersManager _teachersManager = teachersManager;
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;
    private readonly CalculationOptions _calculationOptions = calcOptions.Value;

    public async Task<GetDepartmentStatisticsResponse> Handle(GetDepartmentStatisticsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            List<StatisticItemDto> stats = [];
            PeriodModel period = await _periodsManager.GetPeriodAsync(request.PeriodId);
            double timeFund = period.TimeFund;

            stats.Add(new()
            {
                Name = "Fondo de tiempo del período",
                Mu = "h",
                Value = timeFund
            });

            int teachersCount = await _teachersManager.GetTeachersCountAsync();
            stats.Add(new()
            {
                Name = "Cantidad de profesores",
                Mu = "U",
                Value = teachersCount
            });

            double salary = teachersCount * _calculationOptions.AverageMonthlySalary * period.MonthsCount;
            stats.Add(new()
            {
                Name = "Salario promedio",
                Mu = "CUP",
                Value = salary
            });

            double depCapacity = timeFund * teachersCount;
            stats.Add(new()
            {
                Name = "Capacidad del departamento",
                Mu = "h-profesor/período",
                Value = depCapacity
            });

            double depLoad = await _departmentsManager.GetDepartmentTotalLoadInPeriodAsync(request.PeriodId, request.DepartmentId);
            stats.Add(new()
            {
                Name = "Carga del departamento",
                Mu = "h-profesor/período",
                Value = depLoad
            });

            double depLoadPercent = Math.Round(depLoad / depCapacity * 100, 2);
            stats.Add(new()
            {
                Name = "Porciento de carga",
                Mu = "Porciento (%)",
                Value = depLoadPercent
            });

            double diff = depLoad - depCapacity;
            double personalRequiriement = Math.Floor(diff / (_calculationOptions.MonthTimeFund * period.MonthsCount));
            stats.Add(new()
            {
                Name = "Ajustes de personal",
                Mu = "U",
                Value = personalRequiriement
            });

            double salaryImpact = personalRequiriement * _calculationOptions.AverageMonthlySalary * period.MonthsCount;
            stats.Add(new()
            {
                Name = "Imacto económico luego de ajustes de personal",
                Mu = "CUP",
                Value = salaryImpact
            });

            double rap = await _departmentsManager.CalculateRAPAsync(request.DepartmentId);
            stats.Add(new()
            {
                Name = "Relación alumno-profesor",
                Mu = "",
                Value = rap
            });

            return new()
            {
                StatisticItems = stats
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while fetching the statistics for department {request.DepartmentId} in the period {request.PeriodId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
