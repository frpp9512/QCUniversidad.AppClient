using AutoMapper;
using MediatR;
using QCUniversidad.Api.Contracts;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Requests.Curriculums.Models;
using QCUniversidad.Api.Requests.Curriculums.Responses;
using QCUniversidad.Api.Shared.Dtos.Curriculum;

namespace QCUniversidad.Api.Requests.Curriculums.Handlers;

public class GetCurriculumsRangeRequestHandler(ICurriculumsManager curriculumsManager, IMapper mapper) : IRequestHandler<GetCurriculumsRangeRequest, GetCurriculumsRangeRequestResponse>
{
    private readonly ICurriculumsManager _curriculumsManager = curriculumsManager;
    private readonly IMapper _mapper = mapper;

    public async Task<GetCurriculumsRangeRequestResponse> Handle(GetCurriculumsRangeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            IList<CurriculumModel> curriculums = await _curriculumsManager.GetCurriculumsAsync(request.From, request.To);
            var dtos = curriculums.Select(_mapper.Map<CurriculumDto>).ToList();
            return new()
            {
                Curriculums = dtos,
                From = request.From,
                To = request.To
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                ErrorMessages = [$"Error while fetching the curriculums in the range from: {request.From} to {request.To}. Error message: {ex.Message}"],
                StatusCode = System.Net.HttpStatusCode.InternalServerError
            };
        }
    }
}
