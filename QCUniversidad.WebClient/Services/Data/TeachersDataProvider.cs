using AutoMapper;
using Newtonsoft.Json;
using QCUniversidad.Api.Shared.Dtos.LoadItem;
using QCUniversidad.Api.Shared.Dtos.Teacher;
using QCUniversidad.WebClient.Models.LoadDistribution;
using QCUniversidad.WebClient.Models.LoadItem;
using QCUniversidad.WebClient.Models.Teachers;
using QCUniversidad.WebClient.Services.Contracts;
using System.Text;

namespace QCUniversidad.WebClient.Services.Data;

public class TeachersDataProvider(IApiCallerHttpClientFactory apiCallerFactory, IMapper mapper) : ITeachersDataProvider
{
    private readonly IApiCallerHttpClientFactory _apiCallerFactory = apiCallerFactory;
    private readonly IMapper _mapper = mapper;

    public async Task<IList<TeacherModel>> GetTeachersAsync(int from, int to)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/teacher/list?from={from}&to={to}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<TeacherDto>? teachers = JsonConvert.DeserializeObject<IList<TeacherDto>>(contentText);
        return teachers?.Select(_mapper.Map<TeacherModel>).ToList() ?? [];
    }

    public async Task<int> GetTeachersCountAsync()
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/teacher/count");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        int total = int.Parse(await response.Content.ReadAsStringAsync());
        return total;
    }

    public async Task<bool> ExistsTeacherAsync(Guid id)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/teacher/exists?id={id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        bool result = JsonConvert.DeserializeObject<bool>(contentText);
        return result;
    }

    public async Task<bool> ExistsTeacherAsync(string personalId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/teacher/existsbypersonalid?personalId={personalId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        bool result = JsonConvert.DeserializeObject<bool>(contentText);
        return result;
    }

    public async Task<TeacherModel> GetTeacherAsync(Guid teacherId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/teacher?id={teacherId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        TeacherModel? discipline = JsonConvert.DeserializeObject<TeacherModel>(await response.Content.ReadAsStringAsync());
        return _mapper.Map<TeacherModel>(discipline);
    }

    public async Task<TeacherModel> GetTeacherAsync(string personalId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/teacher/bypersonalid?personalId={personalId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        TeacherModel? teacher = JsonConvert.DeserializeObject<TeacherModel>(await response.Content.ReadAsStringAsync());
        return _mapper.Map<TeacherModel>(teacher);
    }

    public async Task<TeacherModel> GetTeacherAsync(Guid teacherId, Guid periodId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/teacher/withload?id={teacherId}&periodId={periodId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        TeacherModel? discipline = JsonConvert.DeserializeObject<TeacherModel>(await response.Content.ReadAsStringAsync());
        return _mapper.Map<TeacherModel>(discipline);
    }

    public async Task<bool> CreateTeacherAsync(TeacherModel newTeacher)
    {
        ArgumentNullException.ThrowIfNull(newTeacher);

        NewTeacherDto dto = _mapper.Map<NewTeacherDto>(newTeacher);
        string serializedDtos = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PutAsync("/teacher", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateTeacherAsync(TeacherModel teacher)
    {
        ArgumentNullException.ThrowIfNull(teacher);

        EditTeacherDto dto = _mapper.Map<EditTeacherDto>(teacher);
        string serializedDto = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PostAsync("/teacher/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteTeacherAsync(Guid teacherId)
    {
        if (teacherId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(teacherId));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.DeleteAsync($"/teacher?id={teacherId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<IList<TeacherModel>> GetTeachersOfDepartmentAsync(Guid departmentId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/teacher/listofdepartment?departmentId={departmentId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<TeacherDto>? teachers = JsonConvert.DeserializeObject<IList<TeacherDto>>(contentText);
        return teachers?.Select(_mapper.Map<TeacherModel>).ToList() ?? [];
    }

    public async Task<IList<TeacherModel>> GetTeachersOfDepartmentNotAssignedToLoadItemAsync(Guid departmentId, Guid planItemId, Guid? disciplineId = null)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/teacher/listofdepartmentnotinplanitem?departmentId={departmentId}&planItemId={planItemId}{(disciplineId is not null ? $"&disciplineId={disciplineId}" : "")}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<TeacherDto>? teachers = JsonConvert.DeserializeObject<IList<TeacherDto>>(contentText);
        return teachers?.Select(_mapper.Map<TeacherModel>).ToList() ?? [];
    }

    public async Task<IList<LoadViewItemModel>> GetTeacherLoadItemsInPeriodAsync(Guid teacherId, Guid periodId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/teacher/loaditems?id={teacherId}&periodId={periodId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<LoadViewItemDto>? teachers = JsonConvert.DeserializeObject<IList<LoadViewItemDto>>(contentText);
        return teachers?.Select(_mapper.Map<LoadViewItemModel>).ToList() ?? [];
    }

    public async Task<bool> SetLoadItemAsync(CreateLoadItemModel newLoadItem)
    {
        NewLoadItemDto dto = _mapper.Map<NewLoadItemDto>(newLoadItem);
        string serializedDto = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PutAsync("/teacher/setload", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<IList<TeacherModel>> GetTeachersOfDepartmentForPeriodAsync(Guid departmentId, Guid periodId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/teacher/listofdepartmentforperiod?departmentId={departmentId}&periodId={periodId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<TeacherDto>? teachers = JsonConvert.DeserializeObject<IList<TeacherDto>>(contentText);
        return teachers?.Select(_mapper.Map<TeacherModel>).ToList() ?? [];
    }

    public async Task<IList<TeacherModel>> GetTeachersOfDepartmentForPeriodWithLoadItemsAsync(Guid departmentId, Guid periodId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/teacher/listofdepartmentforperiodwithloaditems?departmentId={departmentId}&periodId={periodId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<TeacherDto>? teachers = JsonConvert.DeserializeObject<IList<TeacherDto>>(contentText);
        return teachers?.Select(_mapper.Map<TeacherModel>).ToList() ?? [];
    }

    public async Task<bool> DeleteLoadItemAsync(Guid loadItemId)
    {
        if (loadItemId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(loadItemId));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.DeleteAsync($"/teacher/deleteload?loadItemId={loadItemId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<IList<TeacherModel>> GetSupportTeachersAsync(Guid departmentId, Guid periodId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/teacher/listsupport?departmentId={departmentId}&periodId={periodId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<TeacherDto>? teachers = JsonConvert.DeserializeObject<IList<TeacherDto>>(contentText);
        return teachers?.Select(_mapper.Map<TeacherModel>).ToList() ?? [];
    }

    public async Task<bool> SetNonTeachingLoadAsync(SetNonTeachingLoadModel model)
    {
        SetNonTeachingLoadDto dto = _mapper.Map<SetNonTeachingLoadDto>(model);
        string serializedDto = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PostAsync("/teacher/setnonteachingload", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<IList<BirthdayTeacherModel>> GetBirthdayTeachersForCurrentWeekAsync(Guid departmentId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/statistics/weekbirthdaysfordepartment?departmentId={departmentId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<BirthdayTeacherDto>? teachers = JsonConvert.DeserializeObject<IList<BirthdayTeacherDto>>(contentText);
        return teachers?.Select(_mapper.Map<BirthdayTeacherModel>).ToList() ?? [];
    }

    public async Task<IList<BirthdayTeacherModel>> GetBirthdayTeachersForCurrentMonthAsync(Guid departmentId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/statistics/monthbirthdaysfordepartment?departmentId={departmentId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<BirthdayTeacherDto>? teachers = JsonConvert.DeserializeObject<IList<BirthdayTeacherDto>>(contentText);
        return teachers?.Select(_mapper.Map<BirthdayTeacherModel>).ToList() ?? [];
    }

    public async Task<IList<BirthdayTeacherModel>> GetBirthdayTeachersForCurrentWeekAsync(Guid scopeId, string scope)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/statistics/weekbirthdaysforscope?scope={scope}&scopeId={scopeId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<BirthdayTeacherDto>? teachers = JsonConvert.DeserializeObject<IList<BirthdayTeacherDto>>(contentText);
        return teachers?.Select(_mapper.Map<BirthdayTeacherModel>).ToList() ?? [];
    }

    public async Task<IList<BirthdayTeacherModel>> GetBirthdayTeachersForCurrentMonthAsync(Guid scopeId, string scope)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/statistics/monthbirthdaysforscope?scope={scope}&scopeId={scopeId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<BirthdayTeacherDto>? teachers = JsonConvert.DeserializeObject<IList<BirthdayTeacherDto>>(contentText);
        return teachers?.Select(_mapper.Map<BirthdayTeacherModel>).ToList() ?? [];
    }
}
