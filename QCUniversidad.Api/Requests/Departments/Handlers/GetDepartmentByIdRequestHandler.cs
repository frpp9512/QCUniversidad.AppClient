using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Departments.Responses;
using QCUniversidad.Api.Shared.Dtos.Department;

namespace QCUniversidad.Api.Requests.Departments.Handlers;

public class GetDepartmentByIdRequestHandler(IDepartmentsManager departmentsManager, IMapper mapper) : IRequestHandler<GetDepartmentByIdRequest, GetDepartmentByIdRequestResponse>
{
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetDepartmentByIdRequestResponse> Handle(GetDepartmentByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            DepartmentModel department = await _departmentsManager.GetDepartmentAsync(request.DepartmentId);
            DepartmentDto dto = _mapper.Map<DepartmentDto>(department);
            return new()
            {
                RequestId = request.RequestId,
                DepartmentId = request.DepartmentId,
                Department = dto
            };
        }
        catch (DepartmentNotFoundException)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"The department with id '{request.DepartmentId}' was not found."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while fetching the department with id: {request.DepartmentId}. Error messages: {ex.Message}"]
            };
        }
    }
}
