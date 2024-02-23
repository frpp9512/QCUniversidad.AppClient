using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Departments.Responses;
using QCUniversidad.Api.Shared.Dtos.Department;

namespace QCUniversidad.Api.Requests.Departments.Handlers;

public class GetDepartmentsRangeRequestHandler(IDepartmentsManager departmentsManager, IMapper mapper) : IRequestHandler<GetDepartmentsRangeRequest, GetDepartmentsRangeRequestResponse>
{
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetDepartmentsRangeRequestResponse> Handle(GetDepartmentsRangeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            IList<DepartmentModel> departments = await _departmentsManager.GetDepartmentsAsync(request.From, request.To);
            List<DepartmentDto> dtos = departments.Select(_mapper.Map<DepartmentDto>).ToList();
            foreach (DepartmentDto? dto in dtos)
            {
                dto.DisciplinesCount = await _departmentsManager.GetDepartmentDisciplinesCount(dto.Id);
            }

            return new()
            {
                From = request.From,
                To = request.To,
                Departments = dtos
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [ $"Error while fetching the departments range from: {request.From} to: {request.To}. Error message: {ex.Message}" ],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
