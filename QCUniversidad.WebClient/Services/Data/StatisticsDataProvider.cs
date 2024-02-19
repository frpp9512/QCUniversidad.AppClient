using AutoMapper;
using Newtonsoft.Json;
using QCUniversidad.Api.Shared.Dtos.Statistics;
using QCUniversidad.WebClient.Models.Statistics;
using QCUniversidad.WebClient.Services.Contracts;

namespace QCUniversidad.WebClient.Services.Data;

public class StatisticsDataProvider(IApiCallerHttpClientFactory apiCallerFactory, IMapper mapper) : IStatisticsDataProvider
{
    private readonly IApiCallerHttpClientFactory _apiCallerFactory = apiCallerFactory;
    private readonly IMapper _mapper = mapper;

    public async Task<IList<StatisticItemModel>> GetGlobalStatisticsAsync()
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/statistics/globalstatistics");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<StatisticItemDto>? statisticsItems = JsonConvert.DeserializeObject<IList<StatisticItemDto>>(contentText);
        return statisticsItems?.Select(_mapper.Map<StatisticItemModel>).ToList() ?? [];
    }

    public async Task<IList<StatisticItemModel>> GetGlobalStatisticsForDepartmentAsync(Guid departmentId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/statistics/departmentstatistics?departmentId={departmentId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<StatisticItemDto>? statisticsItems = JsonConvert.DeserializeObject<IList<StatisticItemDto>>(contentText);
        return statisticsItems?.Select(_mapper.Map<StatisticItemModel>).ToList() ?? [];
    }
}
