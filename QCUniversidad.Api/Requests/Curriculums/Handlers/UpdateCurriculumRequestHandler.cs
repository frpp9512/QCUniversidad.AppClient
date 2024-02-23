using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Curriculums.Models;
using QCUniversidad.Api.Requests.Curriculums.Responses;

namespace QCUniversidad.Api.Requests.Curriculums.Handlers;

public class UpdateCurriculumRequestHandler(ICurriculumsManager curriculumsManager, IMapper mapper) : IRequestHandler<UpdateCurriculumRequest, UpdateCurriculumRequestResponse>
{
    private readonly ICurriculumsManager _curriculumsManager = curriculumsManager;
    private readonly IMapper _mapper = mapper;

    public async Task<UpdateCurriculumRequestResponse> Handle(UpdateCurriculumRequest request, CancellationToken cancellationToken)
    {
        if (request.CurriculumToUpdate is null)
        {
            return new()
            {
                ErrorMessages = ["The curriculum cannot be null."],
                StatusCode = System.Net.HttpStatusCode.BadRequest
            };
        }

        try
        {
            CurriculumModel model = _mapper.Map<CurriculumModel>(request.CurriculumToUpdate);
            bool result = await _curriculumsManager.UpdateCurriculumAsync(model);
            return new()
            {
                Updated = result
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while updating the curriculum. Error message: {ex.Message}"]
            };
        }
    }
}
