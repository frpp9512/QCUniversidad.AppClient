using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Departments.Models;
using QCUniversidad.Api.Requests.Departments.Responses;

namespace QCUniversidad.Api.Requests.Departments.Handlers;

public class UpdateDepartmentRequestHandler(IDepartmentsManager departmentsManager, IMapper mapper) : IRequestHandler<UpdateDepartmentRequest, UpdateDepartmentRequestResponse>
{
    private readonly IDepartmentsManager _departmentsManager = departmentsManager;
    private readonly IMapper _mapper = mapper;

    public async Task<UpdateDepartmentRequestResponse> Handle(UpdateDepartmentRequest request, CancellationToken cancellationToken)
    {
        if (request.DepartmentToUpdate is null)
        {
            return new()
            {
                ErrorMessages = ["Should provide the department update data."],
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }

        try
        {
            DepartmentModel model = _mapper.Map<DepartmentModel>(request.DepartmentToUpdate);
            bool result = await _departmentsManager.UpdateDeparmentAsync(model);
            return new()
            {
                Updated = result,
                StatusCode = result ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.InternalServerError
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while updating a department. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
