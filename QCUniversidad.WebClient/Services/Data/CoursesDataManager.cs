using AutoMapper;
using Newtonsoft.Json;
using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.WebClient.Models.Course;
using QCUniversidad.WebClient.Services.Contracts;
using System.Text;

namespace QCUniversidad.WebClient.Services.Data;

public class CoursesDataManager(IApiCallerHttpClientFactory apiCallerFactory, IMapper mapper) : ICoursesDataProvider
{
    private readonly IApiCallerHttpClientFactory _apiCallerFactory = apiCallerFactory;
    private readonly IMapper _mapper = mapper;

    public async Task<Guid> CreateCourseAsync(CourseModel course)
    {
        ArgumentNullException.ThrowIfNull(course);

        NewCourseDto dto = _mapper.Map<NewCourseDto>(course);
        string serializedDtos = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PutAsync("/course", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Error while creating the the course.");
        }

        string responseText = await response.Content.ReadAsStringAsync();
        var created = JsonConvert.DeserializeObject<CourseDto>(responseText);
        return created?.Id ?? throw new Exception("Error while creating the course. The deserialization of the response failed.");
    }

    public async Task<bool> ExistsCourseAsync(Guid id)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/course/exists?id={id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        bool result = JsonConvert.DeserializeObject<bool>(contentText);
        return result;
    }

    public async Task<bool> CheckCourseExistenceByCareerYearAndModality(Guid careerId, int careerYear, int modality)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/course/existsbycareeryearandmodality?careerId={careerId}&careerYear={careerYear}&modality={modality}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        bool result = JsonConvert.DeserializeObject<bool>(contentText);
        return result;
    }

    public async Task<int> GetCoursesCountAsync()
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/course/count");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        int total = int.Parse(await response.Content.ReadAsStringAsync());
        return total;
    }

    public async Task<IList<CourseModel>> GetCoursesAsync(int from = 0, int to = 0)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/course/list?from={from}&to={to}");
        string responseText = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<CourseDto>? courses = JsonConvert.DeserializeObject<IList<CourseDto>>(contentText);
        return courses?.Select(_mapper.Map<CourseModel>).ToList() ?? [];
    }

    public async Task<IList<CourseModel>> GetCoursesAsync(Guid schoolYearId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/course/listbyschoolyear?schoolYearId={schoolYearId}");
        string responseText = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<CourseDto>? courses = JsonConvert.DeserializeObject<IList<CourseDto>>(contentText);
        return courses?.Select(_mapper.Map<CourseModel>).ToList() ?? [];
    }

    public async Task<IList<CourseModel>> GetCoursesAsync(Guid schoolYearId, Guid facultyId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/course/listbyschoolyearandfaculty?schoolYearId={schoolYearId}&facultyId={facultyId}");
        string responseText = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<CourseDto>? courses = JsonConvert.DeserializeObject<IList<CourseDto>>(contentText);
        return courses?.Select(_mapper.Map<CourseModel>).ToList() ?? [];
    }

    public async Task<IList<CourseModel>> GetCoursesAsync(Guid careerId, Guid schoolYearId, Guid facultyId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/course/listbycareerschoolyearandfaculty?careerId={careerId}&schoolYearId={schoolYearId}&facultyId={facultyId}");
        string responseText = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<CourseDto>? courses = JsonConvert.DeserializeObject<IList<CourseDto>>(contentText);
        return courses?.Select(_mapper.Map<CourseModel>).ToList() ?? [];
    }

    public async Task<CourseModel> GetCourseAsync(Guid id)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/course?id={id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        CourseDto? discipline = JsonConvert.DeserializeObject<CourseDto>(await response.Content.ReadAsStringAsync());
        return _mapper.Map<CourseModel>(discipline);
    }

    public async Task<bool> UpdateCourseAsync(CourseModel course)
    {
        ArgumentNullException.ThrowIfNull(course);

        EditCourseDto dto = _mapper.Map<EditCourseDto>(course);
        string serializedDto = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PostAsync("/course/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteCourseAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.DeleteAsync($"/course?id={id}");
        return response.IsSuccessStatusCode;
    }

    public async Task<IList<CourseModel>> GetCoursesForDepartment(Guid departmentId, Guid? schoolYearId = null)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/course/listfordepartment?departmentId={departmentId}{(schoolYearId is not null ? $"&schoolYearId={schoolYearId}" : "")}");
        string responseText = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<CourseDto>? courses = JsonConvert.DeserializeObject<IList<CourseDto>>(contentText);
        return courses?.Select(_mapper.Map<CourseModel>).ToList() ?? [];
    }
}
