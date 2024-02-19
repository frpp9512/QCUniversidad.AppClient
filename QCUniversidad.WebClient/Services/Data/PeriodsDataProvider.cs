using AutoMapper;
using Newtonsoft.Json;
using QCUniversidad.Api.Shared.Dtos.Period;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Services.Contracts;
using System.Text;

namespace QCUniversidad.WebClient.Services.Data;

public class PeriodsDataProvider(IApiCallerHttpClientFactory apiCallerFactory, IMapper mapper) : IPeriodsDataProvider
{
    private readonly IApiCallerHttpClientFactory _apiCallerFactory = apiCallerFactory;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> CreatePeriodAsync(PeriodModel period)
    {
        ArgumentNullException.ThrowIfNull(period);

        NewPeriodDto dto = _mapper.Map<NewPeriodDto>(period);
        string serializedDtos = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PutAsync("/period", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ExistsPeriodAsync(Guid id)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/period/exists?id={id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        bool result = JsonConvert.DeserializeObject<bool>(contentText);
        return result;
    }

    public async Task<int> GetPeriodsCountAsync()
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/period/count");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        int total = int.Parse(await response.Content.ReadAsStringAsync());
        return total;
    }

    public async Task<IList<PeriodModel>> GetPeriodsAsync(int from, int to)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/period/list?from={from}&to={to}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<PeriodDto>? periods = JsonConvert.DeserializeObject<IList<PeriodDto>>(contentText);
        return periods?.Select(_mapper.Map<PeriodModel>).ToList() ?? [];
    }

    public async Task<IList<PeriodModel>> GetPeriodsAsync(Guid? schoolYearId = null)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        string query = schoolYearId is null ? "/period/list" : $"/period/listbyschoolyear?schoolYearId={schoolYearId}";
        HttpResponseMessage response = await client.GetAsync($"/period/listbyschoolyear{(schoolYearId is null ? "" : $"?schoolYearId={schoolYearId}")}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<PeriodDto>? periods = JsonConvert.DeserializeObject<IList<PeriodDto>>(contentText);
        return periods?.Select(_mapper.Map<PeriodModel>).ToList() ?? [];
    }

    public async Task<PeriodModel> GetPeriodAsync(Guid id)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/period?id={id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        PeriodDto? discipline = JsonConvert.DeserializeObject<PeriodDto>(await response.Content.ReadAsStringAsync());
        return _mapper.Map<PeriodModel>(discipline);
    }

    public async Task<bool> UpdatePeriodAsync(PeriodModel period)
    {
        ArgumentNullException.ThrowIfNull(period);

        EditPeriodDto dto = _mapper.Map<EditPeriodDto>(period);
        string serializedDto = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PostAsync("/period/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePeriodAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.DeleteAsync($"/period?id={id}");
        return response.IsSuccessStatusCode;
    }
}
