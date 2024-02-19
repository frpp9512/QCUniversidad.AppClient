using AutoMapper;
using Newtonsoft.Json;
using QCUniversidad.Api.Shared.Dtos.Department;
using QCUniversidad.Api.Shared.Dtos.Statistics;
using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Statistics;
using QCUniversidad.WebClient.Services.Contracts;
using System.Text;

namespace QCUniversidad.WebClient.Services.Data;

public class DepartmentsDataProvider(IApiCallerHttpClientFactory apiCallerFactory, IMapper mapper) : IDepartmentsDataProvider
{
    private readonly IApiCallerHttpClientFactory _apiCallerFactory = apiCallerFactory;
    private readonly IMapper _mapper = mapper;

    public async Task<IList<DepartmentModel>> GetDepartmentsAsync(int from = 0, int to = 0)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/department/listall?from={from}&to={to}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        List<DepartmentDto>? dtos = JsonConvert.DeserializeObject<List<DepartmentDto>>(contentText);
        List<DepartmentModel> models = dtos.Select(_mapper.Map<DepartmentModel>).ToList();
        return models;
    }

    public async Task<IList<DepartmentModel>> GetDepartmentsAsync(Guid facultyId)
    {
        if (facultyId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(facultyId));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/department/list?facultyId={facultyId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        List<DepartmentDto>? dtos = JsonConvert.DeserializeObject<List<DepartmentDto>>(contentText);
        List<DepartmentModel> models = dtos.Select(_mapper.Map<DepartmentModel>).ToList();
        return models;
    }

    public async Task<IList<DepartmentModel>> GetDepartmentsWithLoadAsync(Guid periodId)
    {
        if (periodId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(periodId));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/department/listallwithload?periodId={periodId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        List<DepartmentDto>? dtos = JsonConvert.DeserializeObject<List<DepartmentDto>>(contentText);
        List<DepartmentModel> models = dtos.Select(_mapper.Map<DepartmentModel>).ToList();
        return models;
    }

    public async Task<bool> ExistsDepartmentAsync(Guid departmentId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/department/exists?id={departmentId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        bool result = JsonConvert.DeserializeObject<bool>(contentText);
        return result;
    }

    public async Task<int> GetDepartmentsCountAsync()
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/department/countall");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        int total = int.Parse(await response.Content.ReadAsStringAsync());
        return total;
    }

    public async Task<int> GetDepartmentDisciplinesCount(Guid departmentId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/department/countdisciplines");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        int total = int.Parse(await response.Content.ReadAsStringAsync());
        return total;
    }

    public async Task<int> GetDepartmentsCountAsync(Guid facultyId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/deparment/count?facultyId={facultyId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        int total = int.Parse(await response.Content.ReadAsStringAsync());
        return total;
    }

    public async Task<DepartmentModel> GetDepartmentAsync(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(departmentId));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/department?id={departmentId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        DepartmentModel? dto = JsonConvert.DeserializeObject<DepartmentModel>(contentText);
        DepartmentModel model = _mapper.Map<DepartmentModel>(dto);
        return model;
    }

    public async Task<bool> CreateDepartmentAsync(DepartmentModel newDepartment)
    {
        if (newDepartment is null)
        {
            throw new ArgumentNullException(nameof(newDepartment));
        }

        NewDepartmentDto dto = _mapper.Map<NewDepartmentDto>(newDepartment);
        string serializedData = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PutAsync("/department", new StringContent(serializedData, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateDepartmentAsync(DepartmentModel department)
    {
        if (department is null)
        {
            throw new ArgumentNullException(nameof(department));
        }

        EditDepartmentDto dto = _mapper.Map<EditDepartmentDto>(department);
        string serializedData = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PostAsync("/department/update", new StringContent(serializedData, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteDepartmentAsync(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(departmentId));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.DeleteAsync($"/department?id={departmentId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<IList<StatisticItemModel>> GetDepartmentPeriodStatsAsync(Guid departmentId, Guid periodId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/department/periodstats?departmentId={departmentId}&periodId={periodId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<StatisticItemDto>? statisticsItems = JsonConvert.DeserializeObject<IList<StatisticItemDto>>(contentText);
        return statisticsItems?.Select(_mapper.Map<StatisticItemModel>).ToList() ?? [];
    }
}
