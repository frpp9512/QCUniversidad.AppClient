using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Departments.Responses;
using QCUniversidad.Api.Shared.Dtos.Department;

namespace QCUniversidad.Api.Requests.Departments.Handlers;

public class GetDepartmentsOfFacultyRequestHandler(IDepartmentsManager departmentsManager, IMapper mapper) : IRequestHandler<GetDepartmentsOfFacultyRequest, GetDepartmentsOfFacultyRequestResponse>
{
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetDepartmentsOfFacultyRequestResponse> Handle(GetDepartmentsOfFacultyRequest request, CancellationToken cancellationToken)
    {
        try
        {
            IList<DepartmentModel> departments = await _departmentsManager.GetDepartmentsAsync(request.FacultyId);
            List<DepartmentDto> dtos = departments.Select(_mapper.Map<DepartmentDto>).ToList();
            foreach (DepartmentDto? dto in dtos)
            {
                dto.DisciplinesCount = await _departmentsManager.GetDepartmentDisciplinesCount(dto.Id);
            }

            return new()
            {
                FacultyId = request.FacultyId,
                Departments = dtos
            };
        }
        catch (FacultyNotFoundException)
        {
            return new()
            {
                ErrorMessages = [$"The faculty with id {request.FacultyId} do not exists."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while fetching the departments of faculty {request.FacultyId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
