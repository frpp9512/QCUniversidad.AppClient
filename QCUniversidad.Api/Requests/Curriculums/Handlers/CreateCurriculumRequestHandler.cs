using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Curriculums.Models;
using QCUniversidad.Api.Requests.Curriculums.Responses;
using QCUniversidad.Api.Shared.Dtos.Curriculum;

namespace QCUniversidad.Api.Requests.Curriculums.Handlers;

public class CreateCurriculumRequestHandler(ICurriculumsManager curriculumsManager, IMapper mapper) : IRequestHandler<CreateCurriculumRequest, CreateCurriculumRequestResponse>
{
    private readonly ICurriculumsManager _curriculumsManager = curriculumsManager;
    private readonly IMapper _mapper = mapper;

    public async Task<CreateCurriculumRequestResponse> Handle(CreateCurriculumRequest request, CancellationToken cancellationToken)
    {
        if (request.NewCurriculum is null)
        {
            return new()
            {
                RequestId = request.RequestId,
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
                RequestId = request.RequestId,
                CreatedEntity = _mapper.Map<CurriculumDto>(result),
                CreatedId = result.Id,
                ApiEntityEndpointAction = "GetById"
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                RequestId = request.RequestId,
                ErrorMessages = [$"Error while creating a curriculum. Error message: {ex.Message}"]
            };
        }
    }
}
