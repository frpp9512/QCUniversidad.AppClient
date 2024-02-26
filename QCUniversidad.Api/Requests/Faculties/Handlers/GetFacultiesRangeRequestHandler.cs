using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Faculties.Models;
using QCUniversidad.Api.Requests.Faculties.Responses;
using QCUniversidad.Api.Shared.Dtos.Faculty;

namespace QCUniversidad.Api.Requests.Faculties.Handlers;

public class GetFacultiesRangeRequestHandler(IFacultiesManager facultiesManager, IMapper mapper) : IRequestHandler<GetFacultiesRangeRequest, GetFacultiesRangeRequestResponse>
{
    private readonly IFacultiesManager _facultiesManager = facultiesManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetFacultiesRangeRequestResponse> Handle(GetFacultiesRangeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var faculties = await _facultiesManager.GetFacultiesAsync(request.From, request.To);
            List<FacultyDto> dtos = [];
            foreach (var faculty in faculties)
            {
                var dto = await GetFacultyDto(faculty);
                dtos.Add(dto);
            }

            return new()
            {
                RequestId = request.RequestId,
                From = request.From,
                To = request.To,
                Faculties = [.. dtos]
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while fetching the faculties range from {request.From} to {request.To}. Error message: {ex.Message}"],
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
