using AutoMapper;
using Newtonsoft.Json;
using QCUniversidad.Api.Shared.Dtos.SchoolYear;
using QCUniversidad.WebClient.Models.SchoolYears;
using QCUniversidad.WebClient.Services.Contracts;
using System.Text;

namespace QCUniversidad.WebClient.Services.Data;

public class SchoolYearsDataProvider(IApiCallerHttpClientFactory apiCallerFactory, IMapper mapper) : ISchoolYearDataProvider
{
    private readonly IApiCallerHttpClientFactory _apiCallerFactory = apiCallerFactory;
    private readonly IMapper _mapper = mapper;

    public async Task<SchoolYearModel> GetCurrentSchoolYear()
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/schoolyear/current");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        SchoolYearDto? schoolYear = JsonConvert.DeserializeObject<SchoolYearDto>(await response.Content.ReadAsStringAsync());
        return _mapper.Map<SchoolYearModel>(schoolYear);
    }

    public async Task<SchoolYearModel> GetSchoolYearAsync(Guid id)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/schoolyear?id={id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        SchoolYearDto? schoolYear = JsonConvert.DeserializeObject<SchoolYearDto>(await response.Content.ReadAsStringAsync());
        return _mapper.Map<SchoolYearModel>(schoolYear);
    }

    public async Task<IList<SchoolYearModel>> GetSchoolYearsAsync(int from = 0, int to = 0)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/schoolyear/list?from={from}&to={to}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<SchoolYearDto>? schoolYears = JsonConvert.DeserializeObject<IList<SchoolYearDto>>(contentText);
        return schoolYears?.Select(_mapper.Map<SchoolYearModel>).ToList() ?? [];
    }

    public async Task<int> GetSchoolYearTotalAsync()
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/schoolyear/count");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        int total = int.Parse(await response.Content.ReadAsStringAsync());
        return total;
    }

    public async Task<bool> ExistSchoolYearAsync(Guid id)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/schoolyear/exists?id={id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        bool result = JsonConvert.DeserializeObject<bool>(contentText);
        return result;
    }

    public async Task<bool> CreateSchoolYearAsync(SchoolYearModel schoolYear)
    {
        ArgumentNullException.ThrowIfNull(schoolYear);

        NewSchoolYearDto dto = _mapper.Map<NewSchoolYearDto>(schoolYear);
        string serializedDtos = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PutAsync("/schoolyear", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateSchoolYearAsync(SchoolYearModel schoolYear)
    {
        ArgumentNullException.ThrowIfNull(schoolYear);

        EditSchoolYearDto dto = _mapper.Map<EditSchoolYearDto>(schoolYear);
        string serializedDto = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PostAsync("/schoolYear/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteSchoolYearAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.DeleteAsync($"/schoolyear?id={id}");
        return response.IsSuccessStatusCode;
    }
}
