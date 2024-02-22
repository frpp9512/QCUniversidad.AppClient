using AutoMapper;
using Newtonsoft.Json;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;
using QCUniversidad.WebClient.Models.Planning;
using QCUniversidad.WebClient.Services.Contracts;
using System.Text;

namespace QCUniversidad.WebClient.Services.Data;

public class PlanningDataProvider(IApiCallerHttpClientFactory apiCallerFactory, IMapper mapper) : IPlanningDataProvider
{
    private readonly IApiCallerHttpClientFactory _apiCallerFactory = apiCallerFactory;
    private readonly IMapper _mapper = mapper;

    public async Task<bool> CreateTeachingPlanItemAsync(TeachingPlanItemModel item)
    {
        ArgumentNullException.ThrowIfNull(item);

        NewTeachingPlanItemDto dto = _mapper.Map<NewTeachingPlanItemDto>(item);
        string serializedDtos = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PutAsync("/period/addplanitem", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ExistsTeachingPlanItemAsync(Guid id)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/period/existsplanitem?id={id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        bool result = JsonConvert.DeserializeObject<bool>(contentText);
        return result;
    }

    public Task<int> GetTeachingPlanItemsCountAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<int> GetTeachingPlanItemsCountAsync(Guid periodId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/period/planitemscount?periodId={periodId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        int total = int.Parse(await response.Content.ReadAsStringAsync());
        return total;
    }

    public Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(int from = 0, int to = 0)
    {
        throw new NotImplementedException();
    }

    public async Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(Guid periodId, Guid? courseId = null, int from = 0, int to = 0)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/period/planitems?periodId={periodId}{(courseId is not null ? $"&courseId={courseId}" : "")}&from={from}&to={to}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<TeachingPlanItemSimpleDto>? periods = JsonConvert.DeserializeObject<IList<TeachingPlanItemSimpleDto>>(contentText);
        return periods?.Select(_mapper.Map<TeachingPlanItemModel>).ToList() ?? [];
    }

    public async Task<TeachingPlanItemModel> GetTeachingPlanItemAsync(Guid id)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/period/planitem?id={id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        TeachingPlanItemDto? teachingPlanItem = JsonConvert.DeserializeObject<TeachingPlanItemDto>(await response.Content.ReadAsStringAsync());
        return _mapper.Map<TeachingPlanItemModel>(teachingPlanItem);
    }

    public async Task<bool> UpdateTeachingPlanItemAsync(TeachingPlanItemModel period)
    {
        EditTeachingPlanItemDto dto = _mapper.Map<EditTeachingPlanItemDto>(period);
        string serializedDto = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PostAsync("/period/updateplanitem", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteTeachingPlanItemAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.DeleteAsync($"/period/deleteplanitem?id={id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsOfDepartmentOnPeriodAsync(Guid departmentId, Guid periodId, Guid? courseId = null, bool onlyLoadItems = false)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/department/planningitems?id={departmentId}&periodId={periodId}&onlyLoadItems={onlyLoadItems}{(courseId is not null ? $"&courseId={courseId}" : "")}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<TeachingPlanItemDto>? periods = JsonConvert.DeserializeObject<IList<TeachingPlanItemDto>>(contentText);
        return periods?.Select(_mapper.Map<TeachingPlanItemModel>).ToList() ?? [];
    }
}