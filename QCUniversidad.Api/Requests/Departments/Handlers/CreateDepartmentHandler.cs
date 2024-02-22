using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Departments.Responses;
using QCUniversidad.Api.Shared.Dtos.Department;

namespace QCUniversidad.Api.Requests.Departments.Handlers;

public class CreateDepartmentHandler(IDepartmentsManager departmentsManager, IMapper mapper) : IRequestHandler<CreateDepartmentRequest, CreateDepartmentResponse>
{
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateDepartmentResponse> Handle(CreateDepartmentRequest request, CancellationToken cancellationToken)
    {
        if (request.Department is null)
        {
            return new()
            {
                ErrorMessages = [$"You should provide the department data."],
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }

        try
        {
            DepartmentModel model = _mapper.Map<DepartmentModel>(request.Department);
            var createdDepartment = await _departmentsManager.CreateDepartmentAsync(model);
            return new()
            {
                CreatedEntity = _mapper.Map<DepartmentDto>(createdDepartment),
                ApiEntityEndpointAction = "GetById",
                CreatedId = createdDepartment.Id
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while creating a department. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
