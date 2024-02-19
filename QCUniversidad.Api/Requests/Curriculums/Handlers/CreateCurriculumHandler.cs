using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Curriculums.Models;
using QCUniversidad.Api.Requests.Curriculums.Responses;
using QCUniversidad.Api.Shared.Dtos.Curriculum;

namespace QCUniversidad.Api.Requests.Curriculums.Handlers;

public class CreateCurriculumHandler(ICurriculumsManager curriculumsManager, IMapper mapper) : IRequestHandler<CreateCurriculumRequest, CreateCurriculumResponse>
{
    private readonly ICurriculumsManager _curriculumsManager = curriculumsManager;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateCurriculumResponse> Handle(CreateCurriculumRequest request, CancellationToken cancellationToken)
    {
        if (request.NewCurriculum is null)
        {
            return new()
            {
                ErrorMessages = ["Must provide the curriculum data."],
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }

        try
        {
            CurriculumModel model = _mapper.Map<CurriculumModel>(request.NewCurriculum);
            var result = await _curriculumsManager.CreateCurriculumAsync(_mapper.Map<CurriculumModel>(request.NewCurriculum));
            return new()
            {
                CreatedEntity = _mapper.Map<CurriculumDto>(result),
                CreatedId = result.Id,
                ApiEntityEndpointAction = "GetById"
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while creating a curriculum. Error message: {ex.Message}"]
            };
        }
    }
}
