using AutoMapper;
using Newtonsoft.Json;
using QCUniversidad.Api.Shared.Dtos.Career;
using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Planning;
using QCUniversidad.WebClient.Services.Contracts;
using System.Text;

namespace QCUniversidad.WebClient.Services.Data;

public class CareersDataProvider(IApiCallerHttpClientFactory apiCallerFactory, IMapper mapper) : ICareersDataProvider
{
    private readonly IApiCallerHttpClientFactory _apiCallerFactory = apiCallerFactory;
    private readonly IMapper _mapper = mapper;

    public async Task<IList<CareerModel>> GetCareersAsync(int from = 0, int to = 0)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/career/listall?from={from}&to={to}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        List<CareerDto>? dtos = JsonConvert.DeserializeObject<List<CareerDto>>(contentText);
        List<CareerModel> models = dtos.Select(_mapper.Map<CareerModel>).ToList();
        return models;
    }

    public async Task<IList<CareerModel>> GetCareersAsync(Guid facultyId)
    {
        if (facultyId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(facultyId));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/career/list?facultyId={facultyId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        List<CareerDto>? dtos = JsonConvert.DeserializeObject<List<CareerDto>>(contentText);
        List<CareerModel> models = dtos.Select(_mapper.Map<CareerModel>).ToList();
        return models;
    }

    public async Task<IList<CareerModel>> GetCareersForDepartmentAsync(Guid departmentId)
    {
        if (departmentId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(departmentId));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/career/listfordepartment?departmentId={departmentId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        List<CareerDto>? dtos = JsonConvert.DeserializeObject<List<CareerDto>>(contentText);
        List<CareerModel> models = dtos.Select(_mapper.Map<CareerModel>).ToList();
        return models;
    }

    public async Task<int> GetCareersCountAsync()
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/career/countall");
        if (response.IsSuccessStatusCode)
        {
            int total = int.Parse(await response.Content.ReadAsStringAsync());
            return total;
        }

        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<CareerModel> GetCareerAsync(Guid careerId)
    {
        if (careerId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(careerId));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/career?id={careerId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contextText = await response.Content.ReadAsStringAsync();
        CareerDto? dto = JsonConvert.DeserializeObject<CareerDto>(contextText);
        CareerModel model = _mapper.Map<CareerModel>(dto);
        return model;
    }

    public async Task<bool> ExistsCareerAsync(Guid id)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/career/exists?id={id}");
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }
        string contentText = await response.Content.ReadAsStringAsync();
        bool result = JsonConvert.DeserializeObject<bool>(contentText);
        return result;
    }

    public async Task<bool> CreateCareerAsync(CareerModel newCareer)
    {
        ArgumentNullException.ThrowIfNull(newCareer);

        NewCareerDto dto = _mapper.Map<NewCareerDto>(newCareer);
        string serializedData = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PutAsync("/career", new StringContent(serializedData, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateCareerAsync(CareerModel career)
    {
        ArgumentNullException.ThrowIfNull(career);

        EditCareerDto dto = _mapper.Map<EditCareerDto>(career);
        string serializedData = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PostAsync("/career/update", new StringContent(serializedData, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCareerAsync(Guid careerId)
    {
        if (careerId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(careerId));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.DeleteAsync($"/career?id={careerId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<CoursePeriodPlanningInfoModel> GetCoursePeriodPlanningInfoAsync(Guid courseId, Guid periodId)
    {
        if (courseId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(courseId));
        }

        if (periodId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(periodId));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/course/periodplanninginfo?id={courseId}&periodId={periodId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contextText = await response.Content.ReadAsStringAsync();
        CoursePeriodPlanningInfoDto? dto = JsonConvert.DeserializeObject<CoursePeriodPlanningInfoDto>(contextText);
        CoursePeriodPlanningInfoModel model = _mapper.Map<CoursePeriodPlanningInfoModel>(dto);
        return model;
    }
}
