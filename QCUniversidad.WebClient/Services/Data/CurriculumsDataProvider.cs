using AutoMapper;
using Newtonsoft.Json;
using QCUniversidad.Api.Shared.Dtos.Curriculum;
using QCUniversidad.WebClient.Models.Curriculums;
using QCUniversidad.WebClient.Services.Contracts;
using System.Text;

namespace QCUniversidad.WebClient.Services.Data;

public class CurriculumsDataProvider(IApiCallerHttpClientFactory apiCallerFactory, IMapper mapper) : ICurriculumsDataProvider
{
    private readonly IApiCallerHttpClientFactory _apiCallerFactory = apiCallerFactory;
    private readonly IMapper _mapper = mapper;

    public async Task<IList<CurriculumModel>> GetCurriculumsAsync(int from, int to)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/curriculum/list?from={from}&to={to}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<CurriculumDto>? curriculums = JsonConvert.DeserializeObject<IList<CurriculumDto>>(contentText);
        return curriculums?.Select(_mapper.Map<CurriculumModel>).ToList() ?? [];
    }

    public async Task<IList<CurriculumModel>> GetCurriculumsForCareerAsync(Guid careerId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/curriculum/listforcareer?careerId={careerId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<CurriculumDto>? curriculums = JsonConvert.DeserializeObject<IList<CurriculumDto>>(contentText);
        return curriculums?.Select(_mapper.Map<CurriculumModel>).ToList() ?? [];
    }

    public async Task<int> GetCurriculumsCountAsync()
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/curriculum/count");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        int total = int.Parse(await response.Content.ReadAsStringAsync());
        return total;
    }

    public async Task<bool> ExistsCurriculumAsync(Guid id)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/curriculum/exists?id={id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        bool result = JsonConvert.DeserializeObject<bool>(contentText);
        return result;
    }

    public async Task<CurriculumModel> GetCurriculumAsync(Guid curriculumId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/curriculum?id={curriculumId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        CurriculumModel? discipline = JsonConvert.DeserializeObject<CurriculumModel>(await response.Content.ReadAsStringAsync());
        return _mapper.Map<CurriculumModel>(discipline);
    }

    public async Task<bool> CreateCurriculumAsync(CurriculumModel newCurriculum)
    {
        ArgumentNullException.ThrowIfNull(newCurriculum);

        NewCurriculumDto dto = _mapper.Map<NewCurriculumDto>(newCurriculum);
        string serializedDtos = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PutAsync("/curriculum", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCurriculumAsync(CurriculumModel curriculum)
    {
        ArgumentNullException.ThrowIfNull(curriculum);

        EditCurriculumDto dto = _mapper.Map<EditCurriculumDto>(curriculum);
        string serializedDto = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PostAsync("/curriculum/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCurriculumAsync(Guid curriculumId)
    {
        if (curriculumId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(curriculumId));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.DeleteAsync($"/curriculum?id={curriculumId}");
        return response.IsSuccessStatusCode;
    }
}
