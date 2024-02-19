using AutoMapper;
using Newtonsoft.Json;
using QCUniversidad.Api.Shared.Dtos.Discipline;
using QCUniversidad.WebClient.Models.Disciplines;
using QCUniversidad.WebClient.Services.Contracts;
using System.Text;

namespace QCUniversidad.WebClient.Services.Data;

public class DisciplinesDataProvider(IApiCallerHttpClientFactory apiCallerFactory, IMapper mapper) : IDisciplinesDataProvider
{
    private readonly IApiCallerHttpClientFactory _apiCallerFactory = apiCallerFactory;
    private readonly IMapper _mapper = mapper;

    public async Task<IList<DisciplineModel>> GetDisciplinesAsync(int from = 0, int to = 0)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/discipline/list?from={from}&to={to}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<PopulatedDisciplineDto>? disciplines = JsonConvert.DeserializeObject<IList<PopulatedDisciplineDto>>(contentText);
        return disciplines?.Select(_mapper.Map<DisciplineModel>).ToList() ?? [];
    }

    public async Task<IList<DisciplineModel>> GetDisciplinesAsync(Guid departmentId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/discipline/listofdepartment?departmentId={departmentId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<PopulatedDisciplineDto>? disciplines = JsonConvert.DeserializeObject<IList<PopulatedDisciplineDto>>(contentText);
        return disciplines?.Select(_mapper.Map<DisciplineModel>).ToList() ?? [];
    }

    public async Task<int> GetDisciplinesCountAsync()
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/discipline/count");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        int total = int.Parse(await response.Content.ReadAsStringAsync());
        return total;
    }

    public async Task<DisciplineModel> GetDisciplineAsync(Guid disciplineId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/discipline?id={disciplineId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        DisciplineModel? discipline = JsonConvert.DeserializeObject<DisciplineModel>(await response.Content.ReadAsStringAsync());
        return _mapper.Map<DisciplineModel>(discipline);
    }

    public async Task<DisciplineModel> GetDisciplineAsync(string name)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/discipline/byname?name={name}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        DisciplineModel? discipline = JsonConvert.DeserializeObject<DisciplineModel>(await response.Content.ReadAsStringAsync());
        return _mapper.Map<DisciplineModel>(discipline);
    }

    public async Task<bool> ExistsDisciplineAsync(Guid id)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/discipline/exists?id={id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        bool result = JsonConvert.DeserializeObject<bool>(contentText);
        return result;
    }

    public async Task<bool> ExistsDisciplineAsync(string name)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/discipline/existsbyname?name={name}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        bool result = JsonConvert.DeserializeObject<bool>(contentText);
        return result;
    }

    public async Task<bool> CreateDisciplineAsync(DisciplineModel newDiscipline)
    {
        ArgumentNullException.ThrowIfNull(newDiscipline);

        NewDisciplineDto dto = _mapper.Map<NewDisciplineDto>(newDiscipline);
        string serializedDtos = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PutAsync("/discipline", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateDisciplineAsync(DisciplineModel discipline)
    {
        ArgumentNullException.ThrowIfNull(discipline);

        EditDisciplineDto dto = _mapper.Map<EditDisciplineDto>(discipline);
        string serializedDto = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PostAsync("/discipline/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteDisciplineAsync(Guid disciplineId)
    {
        if (disciplineId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(disciplineId));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.DeleteAsync($"/discipline?id={disciplineId}");
        return response.IsSuccessStatusCode;
    }
}
