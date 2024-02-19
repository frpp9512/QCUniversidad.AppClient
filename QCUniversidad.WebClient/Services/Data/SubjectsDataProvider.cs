using AutoMapper;
using Newtonsoft.Json;
using QCUniversidad.Api.Shared.Dtos.Subject;
using QCUniversidad.WebClient.Models.Subjects;
using QCUniversidad.WebClient.Services.Contracts;
using System.Text;

namespace QCUniversidad.WebClient.Services.Data;

public class SubjectsDataProvider(IApiCallerHttpClientFactory apiCallerFactory, IMapper mapper) : ISubjectsDataProvider
{
    private readonly IApiCallerHttpClientFactory _apiCallerFactory = apiCallerFactory;
    private readonly IMapper _mapper = mapper;

    public async Task<IList<SubjectModel>> GetSubjectsAsync(int from, int to)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/subject/list?from={from}&to={to}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<SubjectDto>? teachers = JsonConvert.DeserializeObject<IList<SubjectDto>>(contentText);
        return teachers?.Select(_mapper.Map<SubjectModel>).ToList() ?? [];
    }

    public async Task<IList<SubjectModel>> GetSubjectsForDisciplineAsync(Guid disciplineId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/subject/listfordiscipline?disciplineId={disciplineId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<SubjectDto>? teachers = JsonConvert.DeserializeObject<IList<SubjectDto>>(contentText);
        return teachers?.Select(_mapper.Map<SubjectModel>).ToList() ?? [];
    }

    public async Task<IList<SubjectModel>> GetSubjectsForCourseAsync(Guid courseId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/subject/getforcourse?courseId={courseId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<SubjectDto>? teachers = JsonConvert.DeserializeObject<IList<SubjectDto>>(contentText);
        return teachers?.Select(_mapper.Map<SubjectModel>).ToList() ?? [];
    }

    public async Task<IList<SubjectModel>> GetSubjectsForCourseInPeriodAsync(Guid courseId, Guid periodId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/subject/getforcourseinperiod?courseId={courseId}&periodId={periodId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<SubjectDto>? teachers = JsonConvert.DeserializeObject<IList<SubjectDto>>(contentText);
        return teachers?.Select(_mapper.Map<SubjectModel>).ToList() ?? [];
    }

    public async Task<IList<SubjectModel>> GetSubjectsForCourseNotAssignedInPeriodAsync(Guid courseId, Guid periodId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/subject/getforcoursenotassignedtoperiod?courseId={courseId}&periodId={periodId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<SubjectDto>? teachers = JsonConvert.DeserializeObject<IList<SubjectDto>>(contentText);
        return teachers?.Select(_mapper.Map<SubjectModel>).ToList() ?? [];
    }

    public async Task<int> GetSubjectsCountAsync()
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/subject/count");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        int total = int.Parse(await response.Content.ReadAsStringAsync());
        return total;
    }

    public async Task<bool> ExistsSubjectAsync(Guid id)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/subject/exists?id={id}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        bool result = JsonConvert.DeserializeObject<bool>(contentText);
        return result;
    }

    public async Task<bool> ExistsSubjectAsync(string name)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/subject/existsbyname?name={name}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        bool result = JsonConvert.DeserializeObject<bool>(contentText);
        return result;
    }

    public async Task<SubjectModel> GetSubjectAsync(Guid subjectId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/subject?id={subjectId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        SubjectModel? subject = JsonConvert.DeserializeObject<SubjectModel>(await response.Content.ReadAsStringAsync());
        return _mapper.Map<SubjectModel>(subject);
    }

    public async Task<SubjectModel> GetSubjectAsync(string name)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/subject/byname?name={name}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        SubjectModel? subject = JsonConvert.DeserializeObject<SubjectModel>(await response.Content.ReadAsStringAsync());
        return _mapper.Map<SubjectModel>(subject);
    }

    public async Task<bool> CreateSubjectAsync(SubjectModel newSubject)
    {
        ArgumentNullException.ThrowIfNull(newSubject);

        NewSubjectDto dto = _mapper.Map<NewSubjectDto>(newSubject);
        string serializedDtos = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PutAsync("/subject", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateSubjectAsync(SubjectModel subject)
    {
        ArgumentNullException.ThrowIfNull(subject);

        EditSubjectDto dto = _mapper.Map<EditSubjectDto>(subject);
        string serializedDto = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PostAsync("/subject/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteSubjectAsync(Guid subjectId)
    {
        if (subjectId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(subjectId));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.DeleteAsync($"/subject?id={subjectId}");
        return response.IsSuccessStatusCode;
    }

    public async Task<IList<PeriodSubjectModel>> GetPeriodSubjectsForCourseAsync(Guid periodId, Guid courseId)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/subject/periodsubjects?periodId={periodId}&courseId={courseId}");
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        string contentText = await response.Content.ReadAsStringAsync();
        IList<PeriodSubjectDto>? teachers = JsonConvert.DeserializeObject<IList<PeriodSubjectDto>>(contentText);
        return teachers?.Select(_mapper.Map<PeriodSubjectModel>).ToList() ?? [];
    }

    public async Task<bool> CreatePeriodSubjectAsync(PeriodSubjectModel newPeriodSubject)
    {
        ArgumentNullException.ThrowIfNull(newPeriodSubject);

        NewPeriodSubjectDto dto = _mapper.Map<NewPeriodSubjectDto>(newPeriodSubject);
        string serializedDtos = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PutAsync("/subject/periodsubject", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<PeriodSubjectModel> GetPeriodSubjectAsync(Guid id)
    {
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.GetAsync($"/subject/periodsubject?id={id}");
        if (response.IsSuccessStatusCode)
        {
            PeriodSubjectDto? subject = JsonConvert.DeserializeObject<PeriodSubjectDto>(await response.Content.ReadAsStringAsync());
            return _mapper.Map<PeriodSubjectModel>(subject);
        }

        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> UpdatePeriodSubjectAsync(PeriodSubjectModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        EditPeriodSubjectDto dto = _mapper.Map<EditPeriodSubjectDto>(model);
        string serializedDto = JsonConvert.SerializeObject(dto);
        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.PostAsync("/subject/updateperiodsubject", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeletePeriodSubjectAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(id));
        }

        HttpClient client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        HttpResponseMessage response = await client.DeleteAsync($"/subject/deleteperiodsubject?id={id}");
        return response.IsSuccessStatusCode;
    }
}
