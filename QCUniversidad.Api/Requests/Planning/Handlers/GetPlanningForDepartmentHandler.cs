using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Planning.Models;
using QCUniversidad.Api.Requests.Planning.Responses;
using QCUniversidad.Api.Shared.Dtos.Teacher;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;

namespace QCUniversidad.Api.Requests.Planning.Handlers;

public class GetPlanningForDepartmentHandler(IPlanningManager planningManager,
                                             IPeriodsManager periodsManager,
                                             ITeachersLoadManager teachersLoadManager,
                                             IMapper mapper) : IRequestHandler<GetPlanningForDepartmentRequest, GetPlanningForDepartmentResponse>
{
    private readonly IPlanningManager _planningManager = planningManager;
    private readonly IPeriodsManager _periodsManager = periodsManager;
    private readonly ITeachersLoadManager _teachersLoadManager = teachersLoadManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetPlanningForDepartmentResponse> Handle(GetPlanningForDepartmentRequest request, CancellationToken cancellationToken)
    {
        try
        {
            IList<TeachingPlanItemModel> result = await _planningManager.GetTeachingPlanItemsOfDepartmentOnPeriod(request.DepartmentId, request.PeriodId, request.CourseId, request.OnlyLoadItems);
            double periodTimeFund = await _periodsManager.GetPeriodTimeFund(request.PeriodId);
            var dtos = result.Select(_mapper.Map<TeachingPlanItemDto>).ToList();
            foreach (TeachingPlanItemDto? dto in dtos)
            {
                if (dto.LoadItems is null)
                {
                    continue;
                }

                foreach (Shared.Dtos.LoadItem.SimpleLoadItemDto loadItem in dto.LoadItems)
                {
                    if (loadItem.Teacher is null)
                    {
                        continue;
                    }

                    double load = await _teachersLoadManager.GetTeacherLoadInPeriodAsync(loadItem.Teacher.Id, request.PeriodId);
                    loadItem.Teacher.Load = new TeacherLoadDto
                    {
                        TeacherId = loadItem.Teacher.Id,
                        TimeFund = periodTimeFund,
                        Load = load,
                        LoadPercent = Math.Round(load / periodTimeFund * 100, 2),
                        PeriodId = request.PeriodId
                    };
                }
            }

            return new()
            {
                PlanningItems = dtos
            };
        }
        catch (ArgumentException ex)
        {
            return new()
            {
                ErrorMessages = [$"Error with the request arguments. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error fetching the department {request.DepartmentId} planning for period {request.PeriodId} and course {request.CourseId} and with only load items {request.OnlyLoadItems}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
