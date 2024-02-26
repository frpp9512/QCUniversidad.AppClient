using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Exceptions;
using QCUniversidad.Api.Requests.Faculties.Models;
using QCUniversidad.Api.Requests.Faculties.Responses;
using QCUniversidad.Api.Shared.Dtos.Faculty;

namespace QCUniversidad.Api.Requests.Faculties.Handlers;

public class GetFacultyIdRequestHandler(IFacultiesManager facultiesManager, IMapper mapper) : IRequestHandler<GetFacultyByIdRequest, GetFacultyByIdRequestResponse>
{
    private readonly IFacultiesManager _facultiesManager = facultiesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetFacultyByIdRequestResponse> Handle(GetFacultyByIdRequest request, CancellationToken cancellationToken)
    {
        try
        {
            FacultyModel result = await _facultiesManager.GetFacultyAsync(request.FacultyId);
            FacultyDto dto = await GetFacultyDto(result);
            return new()
            {
                RequestId = request.RequestId,
                Faculty = dto,
                FacultyId = request.FacultyId
            };
        }
        catch (FacultyNotFoundException)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"The faculty with id {request.FacultyId} do not exist."],
                StatusCode = System.Net.HttpStatusCode.NotFound
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while fetching the faculty with id {request.RequestId}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }

    private async Task<FacultyDto> GetFacultyDto(FacultyModel model)
    {
        FacultyDto dto = _mapper.Map<FacultyDto>(model);
        dto.CareersCount = await _facultiesManager.GetFacultyCareerCountAsync(model.Id);
        dto.DepartmentCount = await _facultiesManager.GetFacultyDepartmentCountAsync(model.Id);
        return dto;
    }
}
