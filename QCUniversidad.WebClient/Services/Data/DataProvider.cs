using AutoMapper;
using Newtonsoft.Json;
using QCUniversidad.Api.Shared.Dtos.Career;
using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.Api.Shared.Dtos.Curriculum;
using QCUniversidad.Api.Shared.Dtos.Department;
using QCUniversidad.Api.Shared.Dtos.Discipline;
using QCUniversidad.Api.Shared.Dtos.Faculty;
using QCUniversidad.Api.Shared.Dtos.LoadItem;
using QCUniversidad.Api.Shared.Dtos.Period;
using QCUniversidad.Api.Shared.Dtos.SchoolYear;
using QCUniversidad.Api.Shared.Dtos.Statistics;
using QCUniversidad.Api.Shared.Dtos.Subject;
using QCUniversidad.Api.Shared.Dtos.Teacher;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;
using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Courses;
using QCUniversidad.WebClient.Models.Curriculums;
using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Disciplines;
using QCUniversidad.WebClient.Models.Faculties;
using QCUniversidad.WebClient.Models.LoadDistribution;
using QCUniversidad.WebClient.Models.LoadItem;
using QCUniversidad.WebClient.Models.Periods;
using QCUniversidad.WebClient.Models.Planning;
using QCUniversidad.WebClient.Models.SchoolYears;
using QCUniversidad.WebClient.Models.Statistics;
using QCUniversidad.WebClient.Models.Subjects;
using QCUniversidad.WebClient.Models.Teachers;
using QCUniversidad.WebClient.Services.Platform;
using System.Text;

namespace QCUniversidad.WebClient.Services.Data;

public class DataProvider : IDataProvider
{
    private readonly IApiCallerHttpClientFactory _apiCallerFactory;
    private readonly IMapper _mapper;

    public DataProvider(IApiCallerHttpClientFactory apiCallerFactory, IMapper mapper)
    {
        _apiCallerFactory = apiCallerFactory;
        _mapper = mapper;
    }

    #region Faculties

    public async Task<IList<FacultyModel>> GetFacultiesAsync(int from = 0, int to = 0)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/faculty/list?from={from}&to={to}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var faculties = JsonConvert.DeserializeObject<IList<FacultyDto>>(contentText);
            return faculties?.Select(f => _mapper.Map<FacultyModel>(f)).ToList() ?? new List<FacultyModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> ExistFacultyAsync(Guid id)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/faculty/exists?id={id}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(contentText);
            return result;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> CreateFacultyAsync(FacultyModel facultyModel)
    {
        if (facultyModel is not null)
        {
            var dto = _mapper.Map<FacultyDto>(facultyModel);
            var serializedDtos = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PutAsync("/faculty", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(facultyModel));
    }

    public async Task<int> GetFacultiesTotalAsync()
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/faculty/count");
        if (response.IsSuccessStatusCode)
        {
            var total = int.Parse(await response.Content.ReadAsStringAsync());
            return total;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<FacultyModel> GetFacultyAsync(Guid id)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/faculty?id={id}");
        if (response.IsSuccessStatusCode)
        {
            var faculty = JsonConvert.DeserializeObject<FacultyDto>(await response.Content.ReadAsStringAsync());
            return _mapper.Map<FacultyModel>(faculty);
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> UpdateFacultyAsync(FacultyModel facultyModel)
    {
        if (facultyModel is not null)
        {
            var dto = _mapper.Map<FacultyDto>(facultyModel);
            var serializedDto = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PostAsync("/faculty/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(facultyModel));
    }

    public async Task<bool> DeleteFacultyAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.DeleteAsync($"/faculty?id={id}");
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(id));
    }

    #endregion

    #region Deparments

    public async Task<IList<DepartmentModel>> GetDepartmentsAsync(int from = 0, int to = 0)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/department/listall?from={from}&to={to}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var dtos = JsonConvert.DeserializeObject<List<DepartmentDto>>(contentText);
            var models = dtos.Select(dto => _mapper.Map<DepartmentModel>(dto)).ToList();
            return models;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<IList<DepartmentModel>> GetDepartmentsAsync(Guid facultyId)
    {
        if (facultyId != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.GetAsync($"/department/list?facultyId={facultyId}");
            if (response.IsSuccessStatusCode)
            {
                var contentText = await response.Content.ReadAsStringAsync();
                var dtos = JsonConvert.DeserializeObject<List<DepartmentDto>>(contentText);
                var models = dtos.Select(dto => _mapper.Map<DepartmentModel>(dto)).ToList();
                return models;
            }
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }
        throw new ArgumentNullException(nameof(facultyId));
    }

    public async Task<IList<DepartmentModel>> GetDepartmentsWithLoadAsync(Guid periodId)
    {
        if (periodId != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.GetAsync($"/department/listallwithload?periodId={periodId}");
            if (response.IsSuccessStatusCode)
            {
                var contentText = await response.Content.ReadAsStringAsync();
                var dtos = JsonConvert.DeserializeObject<List<DepartmentDto>>(contentText);
                var models = dtos.Select(dto => _mapper.Map<DepartmentModel>(dto)).ToList();
                return models;
            }
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }
        throw new ArgumentNullException(nameof(periodId));
    }

    public async Task<bool> ExistsDepartmentAsync(Guid departmentId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/department/exists?id={departmentId}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(contentText);
            return result;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<int> GetDepartmentsCountAsync()
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/department/countall");
        if (response.IsSuccessStatusCode)
        {
            var total = int.Parse(await response.Content.ReadAsStringAsync());
            return total;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<int> GetDepartmentDisciplinesCount(Guid departmentId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/department/countdisciplines");
        if (response.IsSuccessStatusCode)
        {
            var total = int.Parse(await response.Content.ReadAsStringAsync());
            return total;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<int> GetDepartmentsCountAsync(Guid facultyId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/deparment/count?facultyId={facultyId}");
        if (response.IsSuccessStatusCode)
        {
            var total = int.Parse(await response.Content.ReadAsStringAsync());
            return total;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<DepartmentModel> GetDepartmentAsync(Guid departmentId)
    {
        if (departmentId != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.GetAsync($"/department?id={departmentId}");
            if (response.IsSuccessStatusCode)
            {
                var contentText = await response.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<DepartmentModel>(contentText);
                var model = _mapper.Map<DepartmentModel>(dto);
                return model;
            }
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }
        throw new ArgumentNullException(nameof(departmentId));
    }

    public async Task<bool> CreateDepartmentAsync(DepartmentModel newDepartment)
    {
        if (newDepartment is not null)
        {
            var dto = _mapper.Map<NewDepartmentDto>(newDepartment);
            var serializedData = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PutAsync("/department", new StringContent(serializedData, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(newDepartment));
    }

    public async Task<bool> UpdateDepartmentAsync(DepartmentModel department)
    {
        if (department is not null)
        {
            var dto = _mapper.Map<EditDepartmentDto>(department);
            var serializedData = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PostAsync("/department/update", new StringContent(serializedData, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(department));
    }

    public async Task<bool> DeleteDepartmentAsync(Guid departmentId)
    {
        if (departmentId != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.DeleteAsync($"/department?id={departmentId}");
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(departmentId));
    }

    #endregion

    #region Careers

    public async Task<IList<CareerModel>> GetCareersAsync(int from = 0, int to = 0)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/career/listall?from={from}&to={to}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var dtos = JsonConvert.DeserializeObject<List<CareerDto>>(contentText);
            var models = dtos.Select(dto => _mapper.Map<CareerModel>(dto)).ToList();
            return models;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<IList<CareerModel>> GetCareersAsync(Guid facultyId)
    {
        if (facultyId != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.GetAsync($"/career/list?facultyId={facultyId}");
            if (response.IsSuccessStatusCode)
            {
                var contentText = await response.Content.ReadAsStringAsync();
                var dtos = JsonConvert.DeserializeObject<List<CareerDto>>(contentText);
                var models = dtos.Select(dto => _mapper.Map<CareerModel>(dto)).ToList();
                return models;
            }
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }
        throw new ArgumentNullException(nameof(facultyId));
    }

    public async Task<int> GetCareersCountAsync()
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/career/countall");
        if (response.IsSuccessStatusCode)
        {
            var total = int.Parse(await response.Content.ReadAsStringAsync());
            return total;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<CareerModel> GetCareerAsync(Guid careerId)
    {
        if (careerId != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.GetAsync($"/career?id={careerId}");
            if (response.IsSuccessStatusCode)
            {
                var contextText = await response.Content.ReadAsStringAsync();
                var dto = JsonConvert.DeserializeObject<CareerDto>(contextText);
                var model = _mapper.Map<CareerModel>(dto);
                return model;
            }
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }
        throw new ArgumentNullException(nameof(careerId));
    }

    public async Task<bool> ExistsCareerAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.GetAsync($"/career/exists?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var contentText = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<bool>(contentText);
                return result;
            }
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<bool> CreateCareerAsync(CareerModel newCareer)
    {
        if (newCareer is not null)
        {
            var dto = _mapper.Map<NewCareerDto>(newCareer);
            var serializedData = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PutAsync("/career", new StringContent(serializedData, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(newCareer));
    }

    public async Task<bool> UpdateCareerAsync(CareerModel career)
    {
        if (career is not null)
        {
            var dto = _mapper.Map<EditCareerDto>(career);
            var serializedData = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PostAsync("/career/update", new StringContent(serializedData, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(career));
    }

    public async Task<bool> DeleteCareerAsync(Guid careerId)
    {
        if (careerId != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.DeleteAsync($"/career?id={careerId}");
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(careerId));
    }

    #endregion

    #region Disciplines

    public async Task<IList<DisciplineModel>> GetDisciplinesAsync(int from = 0, int to = 0)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/discipline/list?from={from}&to={to}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var disciplines = JsonConvert.DeserializeObject<IList<PopulatedDisciplineDto>>(contentText);
            return disciplines?.Select(f => _mapper.Map<DisciplineModel>(f)).ToList() ?? new List<DisciplineModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<int> GetDisciplinesCountAsync()
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/discipline/count");
        if (response.IsSuccessStatusCode)
        {
            var total = int.Parse(await response.Content.ReadAsStringAsync());
            return total;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<DisciplineModel> GetDisciplineAsync(Guid disciplineId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/discipline?id={disciplineId}");
        if (response.IsSuccessStatusCode)
        {
            var discipline = JsonConvert.DeserializeObject<DisciplineModel>(await response.Content.ReadAsStringAsync());
            return _mapper.Map<DisciplineModel>(discipline);
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> ExistsDisciplineAsync(Guid id)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/discipline/exists?id={id}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(contentText);
            return result;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> CreateDisciplineAsync(DisciplineModel newDiscipline)
    {
        if (newDiscipline is not null)
        {
            var dto = _mapper.Map<NewDisciplineDto>(newDiscipline);
            var serializedDtos = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PutAsync("/discipline", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(newDiscipline));
    }

    public async Task<bool> UpdateDisciplineAsync(DisciplineModel discipline)
    {
        if (discipline is not null)
        {
            var dto = _mapper.Map<EditDisciplineDto>(discipline);
            var serializedDto = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PostAsync("/discipline/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(discipline));
    }

    public async Task<bool> DeleteDisciplineAsync(Guid disciplineId)
    {
        if (disciplineId != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.DeleteAsync($"/discipline?id={disciplineId}");
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(disciplineId));
    }

    #endregion

    #region Teachers

    public async Task<IList<TeacherModel>> GetTeachersAsync(int from, int to)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/teacher/list?from={from}&to={to}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var teachers = JsonConvert.DeserializeObject<IList<TeacherDto>>(contentText);
            return teachers?.Select(f => _mapper.Map<TeacherModel>(f)).ToList() ?? new List<TeacherModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<int> GetTeachersCountAsync()
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/teacher/count");
        if (response.IsSuccessStatusCode)
        {
            var total = int.Parse(await response.Content.ReadAsStringAsync());
            return total;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> ExistsTeacherAsync(Guid id)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/teacher/exists?id={id}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(contentText);
            return result;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<TeacherModel> GetTeacherAsync(Guid teacherId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/teacher?id={teacherId}");
        if (response.IsSuccessStatusCode)
        {
            var discipline = JsonConvert.DeserializeObject<TeacherModel>(await response.Content.ReadAsStringAsync());
            return _mapper.Map<TeacherModel>(discipline);
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<TeacherModel> GetTeacherAsync(Guid teacherId, Guid periodId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/teacher/withload?id={teacherId}&periodId={periodId}");
        if (response.IsSuccessStatusCode)
        {
            var discipline = JsonConvert.DeserializeObject<TeacherModel>(await response.Content.ReadAsStringAsync());
            return _mapper.Map<TeacherModel>(discipline);
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> CreateTeacherAsync(TeacherModel newTeacher)
    {
        if (newTeacher is not null)
        {
            var dto = _mapper.Map<NewTeacherDto>(newTeacher);
            var serializedDtos = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PutAsync("/teacher", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(newTeacher));
    }

    public async Task<bool> UpdateTeacherAsync(TeacherModel teacher)
    {
        if (teacher is not null)
        {
            var dto = _mapper.Map<EditTeacherDto>(teacher);
            var serializedDto = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PostAsync("/teacher/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(teacher));
    }

    public async Task<bool> DeleteTeacherAsync(Guid teacherId)
    {
        if (teacherId != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.DeleteAsync($"/teacher?id={teacherId}");
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(teacherId));
    }

    public async Task<IList<TeacherModel>> GetTeachersOfDepartmentAsync(Guid departmentId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/teacher/listofdepartment?departmentId={departmentId}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var teachers = JsonConvert.DeserializeObject<IList<TeacherDto>>(contentText);
            return teachers?.Select(f => _mapper.Map<TeacherModel>(f)).ToList() ?? new List<TeacherModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<IList<TeacherModel>> GetTeachersOfDepartmentNotAssignedToLoadItemAsync(Guid departmentId, Guid planItemId, Guid? disciplineId = null)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/teacher/listofdepartmentnotinplanitem?departmentId={departmentId}&planItemId={planItemId}{(disciplineId is not null ? $"&disciplineId={disciplineId}" : "")}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var teachers = JsonConvert.DeserializeObject<IList<TeacherDto>>(contentText);
            return teachers?.Select(f => _mapper.Map<TeacherModel>(f)).ToList() ?? new List<TeacherModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<IList<LoadViewItemModel>> GetTeacherLoadItemsInPeriodAsync(Guid teacherId, Guid periodId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/teacher/loaditems?id={teacherId}&periodId={periodId}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var teachers = JsonConvert.DeserializeObject<IList<LoadViewItemDto>>(contentText);
            return teachers?.Select(f => _mapper.Map<LoadViewItemModel>(f)).ToList() ?? new List<LoadViewItemModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> SetLoadItemAsync(CreateLoadItemModel newLoadItem)
    {
        var dto = _mapper.Map<NewLoadItemDto>(newLoadItem);
        var serializedDto = JsonConvert.SerializeObject(dto);
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.PutAsync("/teacher/setload", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    public async Task<IList<TeacherModel>> GetTeachersOfDepartmentForPeriodAsync(Guid departmentId, Guid periodId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/teacher/listofdepartmentforperiod?departmentId={departmentId}&periodId={periodId}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var teachers = JsonConvert.DeserializeObject<IList<TeacherDto>>(contentText);
            return teachers?.Select(f => _mapper.Map<TeacherModel>(f)).ToList() ?? new List<TeacherModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> DeleteLoadItemAsync(Guid loadItemId)
    {
        if (loadItemId != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.DeleteAsync($"/teacher/deleteload?loadItemId={loadItemId}");
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(loadItemId));
    }

    public async Task<IList<TeacherModel>> GetSupportTeachersAsync(Guid departmentId, Guid periodId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/teacher/listsupport?departmentId={departmentId}&periodId={periodId}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var teachers = JsonConvert.DeserializeObject<IList<TeacherDto>>(contentText);
            return teachers?.Select(f => _mapper.Map<TeacherModel>(f)).ToList() ?? new List<TeacherModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> SetNonTeachingLoadAsync(SetNonTeachingLoadModel model)
    {
        var dto = _mapper.Map<SetNonTeachingLoadDto>(model);
        var serializedDto = JsonConvert.SerializeObject(dto);
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.PostAsync("/teacher/setnonteachingload", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
        return response.IsSuccessStatusCode;
    }

    #endregion

    #region Subjects

    public async Task<IList<SubjectModel>> GetSubjectsAsync(int from, int to)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/subject/list?from={from}&to={to}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var teachers = JsonConvert.DeserializeObject<IList<SubjectDto>>(contentText);
            return teachers?.Select(f => _mapper.Map<SubjectModel>(f)).ToList() ?? new List<SubjectModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<IList<SubjectModel>> GetSubjectsForCourseAsync(Guid courseId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/subject/getforcourse?courseId={courseId}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var teachers = JsonConvert.DeserializeObject<IList<SubjectDto>>(contentText);
            return teachers?.Select(f => _mapper.Map<SubjectModel>(f)).ToList() ?? new List<SubjectModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<IList<SubjectModel>> GetSubjectsForCourseInPeriodAsync(Guid courseId, Guid periodId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/subject/getforcourseinperiod?courseId={courseId}&periodId={periodId}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var teachers = JsonConvert.DeserializeObject<IList<SubjectDto>>(contentText);
            return teachers?.Select(f => _mapper.Map<SubjectModel>(f)).ToList() ?? new List<SubjectModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<IList<SubjectModel>> GetSubjectsForCourseNotAssignedInPeriodAsync(Guid courseId, Guid periodId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/subject/getforcoursenotassignedtoperiod?courseId={courseId}&periodId={periodId}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var teachers = JsonConvert.DeserializeObject<IList<SubjectDto>>(contentText);
            return teachers?.Select(f => _mapper.Map<SubjectModel>(f)).ToList() ?? new List<SubjectModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<int> GetSubjectsCountAsync()
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/subject/count");
        if (response.IsSuccessStatusCode)
        {
            var total = int.Parse(await response.Content.ReadAsStringAsync());
            return total;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> ExistsSubjectAsync(Guid id)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/subject/exists?id={id}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(contentText);
            return result;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<SubjectModel> GetSubjectAsync(Guid subjectId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/subject?id={subjectId}");
        if (response.IsSuccessStatusCode)
        {
            var subject = JsonConvert.DeserializeObject<SubjectModel>(await response.Content.ReadAsStringAsync());
            return _mapper.Map<SubjectModel>(subject);
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> CreateSubjectAsync(SubjectModel newSubject)
    {
        if (newSubject is not null)
        {
            var dto = _mapper.Map<NewSubjectDto>(newSubject);
            var serializedDtos = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PutAsync("/subject", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(newSubject));
    }

    public async Task<bool> UpdateSubjectAsync(SubjectModel subject)
    {
        if (subject is not null)
        {
            var dto = _mapper.Map<EditSubjectDto>(subject);
            var serializedDto = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PostAsync("/subject/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(subject));
    }

    public async Task<bool> DeleteSubjectAsync(Guid subjectId)
    {
        if (subjectId != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.DeleteAsync($"/subject?id={subjectId}");
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(subjectId));
    }

    public async Task<IList<PeriodSubjectModel>> GetPeriodSubjectsForCourseAsync(Guid periodId, Guid courseId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/subject/periodsubjects?periodId={periodId}&courseId={courseId}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var teachers = JsonConvert.DeserializeObject<IList<PeriodSubjectDto>>(contentText);
            return teachers?.Select(f => _mapper.Map<PeriodSubjectModel>(f)).ToList() ?? new List<PeriodSubjectModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> CreatePeriodSubjectAsync(PeriodSubjectModel newPeriodSubject)
    {
        if (newPeriodSubject is not null)
        {
            var dto = _mapper.Map<NewPeriodSubjectDto>(newPeriodSubject);
            var serializedDtos = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PutAsync("/subject/periodsubject", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(newPeriodSubject));
    }

    public async Task<PeriodSubjectModel> GetPeriodSubjectAsync(Guid id)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/subject/periodsubject?id={id}");
        if (response.IsSuccessStatusCode)
        {
            var subject = JsonConvert.DeserializeObject<PeriodSubjectDto>(await response.Content.ReadAsStringAsync());
            return _mapper.Map<PeriodSubjectModel>(subject);
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> UpdatePeriodSubjectAsync(PeriodSubjectModel model)
    {
        if (model is not null)
        {
            var dto = _mapper.Map<EditPeriodSubjectDto>(model);
            var serializedDto = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PostAsync("/subject/updateperiodsubject", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(model));
    }

    public async Task<bool> DeletePeriodSubjectAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.DeleteAsync($"/subject/deleteperiodsubject?id={id}");
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(id));
    }

    #endregion

    #region Curriculums

    public async Task<IList<CurriculumModel>> GetCurriculumsAsync(int from, int to)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/curriculum/list?from={from}&to={to}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var curriculums = JsonConvert.DeserializeObject<IList<CurriculumDto>>(contentText);
            return curriculums?.Select(f => _mapper.Map<CurriculumModel>(f)).ToList() ?? new List<CurriculumModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<IList<CurriculumModel>> GetCurriculumsForCareerAsync(Guid careerId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/curriculum/listforcareer?careerId={careerId}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var curriculums = JsonConvert.DeserializeObject<IList<CurriculumDto>>(contentText);
            return curriculums?.Select(f => _mapper.Map<CurriculumModel>(f)).ToList() ?? new List<CurriculumModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<int> GetCurriculumsCountAsync()
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/curriculum/count");
        if (response.IsSuccessStatusCode)
        {
            var total = int.Parse(await response.Content.ReadAsStringAsync());
            return total;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> ExistsCurriculumAsync(Guid id)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/curriculum/exists?id={id}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(contentText);
            return result;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<CurriculumModel> GetCurriculumAsync(Guid curriculumId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/curriculum?id={curriculumId}");
        if (response.IsSuccessStatusCode)
        {
            var discipline = JsonConvert.DeserializeObject<CurriculumModel>(await response.Content.ReadAsStringAsync());
            return _mapper.Map<CurriculumModel>(discipline);
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> CreateCurriculumAsync(CurriculumModel newCurriculum)
    {
        if (newCurriculum is not null)
        {
            var dto = _mapper.Map<NewCurriculumDto>(newCurriculum);
            var serializedDtos = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PutAsync("/curriculum", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(newCurriculum));
    }

    public async Task<bool> UpdateCurriculumAsync(CurriculumModel curriculum)
    {
        if (curriculum is not null)
        {
            var dto = _mapper.Map<EditCurriculumDto>(curriculum);
            var serializedDto = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PostAsync("/curriculum/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(curriculum));
    }

    public async Task<bool> DeleteCurriculumAsync(Guid curriculumId)
    {
        if (curriculumId != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.DeleteAsync($"/curriculum?id={curriculumId}");
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(curriculumId));
    }

    #endregion

    #region SchoolYears

    public async Task<SchoolYearModel> GetCurrentSchoolYear()
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/schoolyear/current");
        if (response.IsSuccessStatusCode)
        {
            var schoolYear = JsonConvert.DeserializeObject<SchoolYearDto>(await response.Content.ReadAsStringAsync());
            return _mapper.Map<SchoolYearModel>(schoolYear);
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<SchoolYearModel> GetSchoolYearAsync(Guid id)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/schoolyear?id={id}");
        if (response.IsSuccessStatusCode)
        {
            var schoolYear = JsonConvert.DeserializeObject<SchoolYearDto>(await response.Content.ReadAsStringAsync());
            return _mapper.Map<SchoolYearModel>(schoolYear);
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<IList<SchoolYearModel>> GetSchoolYearsAsync(int from = 0, int to = 0)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/schoolyear/list?from={from}&to={to}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var schoolYears = JsonConvert.DeserializeObject<IList<SchoolYearDto>>(contentText);
            return schoolYears?.Select(f => _mapper.Map<SchoolYearModel>(f)).ToList() ?? new List<SchoolYearModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<int> GetSchoolYearTotalAsync()
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/schoolyear/count");
        if (response.IsSuccessStatusCode)
        {
            var total = int.Parse(await response.Content.ReadAsStringAsync());
            return total;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> ExistSchoolYearAsync(Guid id)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/schoolyear/exists?id={id}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(contentText);
            return result;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> CreateSchoolYearAsync(SchoolYearModel schoolYear)
    {
        if (schoolYear is not null)
        {
            var dto = _mapper.Map<NewSchoolYearDto>(schoolYear);
            var serializedDtos = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PutAsync("/schoolyear", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(schoolYear));
    }

    public async Task<bool> UpdateSchoolYearAsync(SchoolYearModel schoolYear)
    {
        if (schoolYear is not null)
        {
            var dto = _mapper.Map<EditSchoolYearDto>(schoolYear);
            var serializedDto = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PostAsync("/schoolYear/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(schoolYear));
    }

    public async Task<bool> DeleteSchoolYearAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.DeleteAsync($"/schoolyear?id={id}");
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(id));
    }

    #endregion

    #region Courses

    public async Task<Guid> CreateCourseAsync(CourseModel course)
    {
        if (course is not null)
        {
            var dto = _mapper.Map<NewCourseDto>(course);
            var serializedDtos = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PutAsync("/course", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync();
                var id = JsonConvert.DeserializeObject<Guid>(responseText);
                return id;
            }
        }
        throw new ArgumentNullException(nameof(course));
    }

    public async Task<bool> ExistsCourseAsync(Guid id)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/course/exists?id={id}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(contentText);
            return result;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> CheckCourseExistenceByCareerYearAndModality(Guid careerId, int careerYear, int modality)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/course/existsbycareeryearandmodality?careerId={careerId}&careerYear={careerYear}&modality={modality}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(contentText);
            return result;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<int> GetCoursesCountAsync()
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/course/count");
        if (response.IsSuccessStatusCode)
        {
            var total = int.Parse(await response.Content.ReadAsStringAsync());
            return total;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<IList<CourseModel>> GetCoursesAsync(int from = 0, int to = 0)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/course/list?from={from}&to={to}");
        var responseText = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var courses = JsonConvert.DeserializeObject<IList<CourseDto>>(contentText);
            return courses?.Select(f => _mapper.Map<CourseModel>(f)).ToList() ?? new List<CourseModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<IList<CourseModel>> GetCoursesAsync(Guid schoolYearId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/course/listbyschoolyear?schoolYearId={schoolYearId}");
        var responseText = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var courses = JsonConvert.DeserializeObject<IList<CourseDto>>(contentText);
            return courses?.Select(f => _mapper.Map<CourseModel>(f)).ToList() ?? new List<CourseModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<CourseModel> GetCourseAsync(Guid id)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/course?id={id}");
        if (response.IsSuccessStatusCode)
        {
            var discipline = JsonConvert.DeserializeObject<CourseDto>(await response.Content.ReadAsStringAsync());
            return _mapper.Map<CourseModel>(discipline);
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> UpdateCourseAsync(CourseModel course)
    {
        if (course is not null)
        {
            var dto = _mapper.Map<EditCourseDto>(course);
            var serializedDto = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PostAsync("/course/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(course));
    }

    public async Task<bool> DeleteCourseAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.DeleteAsync($"/course?id={id}");
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<IList<CourseModel>> GetCoursesForDepartment(Guid departmentId, Guid? schoolYearId = null)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/course/listfordepartment?departmentId={departmentId}{(schoolYearId is not null ? $"&schoolYearId={schoolYearId}" : "")}");
        var responseText = await response.Content.ReadAsStringAsync();
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var courses = JsonConvert.DeserializeObject<IList<CourseDto>>(contentText);
            return courses?.Select(f => _mapper.Map<CourseModel>(f)).ToList() ?? new List<CourseModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    #endregion

    #region Periods

    public async Task<bool> CreatePeriodAsync(PeriodModel period)
    {
        if (period is not null)
        {
            var dto = _mapper.Map<NewPeriodDto>(period);
            var serializedDtos = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PutAsync("/period", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(period));
    }

    public async Task<bool> ExistsPeriodAsync(Guid id)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/period/exists?id={id}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<bool>(contentText);
            return result;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<int> GetPeriodsCountAsync()
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/period/count");
        if (response.IsSuccessStatusCode)
        {
            var total = int.Parse(await response.Content.ReadAsStringAsync());
            return total;
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<IList<PeriodModel>> GetPeriodsAsync(int from, int to)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/period/list?from={from}&to={to}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var periods = JsonConvert.DeserializeObject<IList<PeriodDto>>(contentText);
            return periods?.Select(f => _mapper.Map<PeriodModel>(f)).ToList() ?? new List<PeriodModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<IList<PeriodModel>> GetPeriodsAsync(Guid? schoolYearId = null)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var query = schoolYearId is null ? "/period/list" : $"/period/listbyschoolyear?schoolYearId={schoolYearId}";
        var response = await client.GetAsync($"/period/listbyschoolyear{(schoolYearId is null ? "" : $"?schoolYearId={schoolYearId}")}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var periods = JsonConvert.DeserializeObject<IList<PeriodDto>>(contentText);
            return periods?.Select(f => _mapper.Map<PeriodModel>(f)).ToList() ?? new List<PeriodModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<PeriodModel> GetPeriodAsync(Guid id)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/period?id={id}");
        if (response.IsSuccessStatusCode)
        {
            var discipline = JsonConvert.DeserializeObject<PeriodDto>(await response.Content.ReadAsStringAsync());
            return _mapper.Map<PeriodModel>(discipline);
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<bool> UpdatePeriodAsync(PeriodModel period)
    {
        if (period is not null)
        {
            var dto = _mapper.Map<EditPeriodDto>(period);
            var serializedDto = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PostAsync("/period/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(period));
    }

    public async Task<bool> DeletePeriodAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.DeleteAsync($"/period?id={id}");
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(id));
    }

    #endregion

    #region TeachingPlanItems

    public async Task<bool> CreateTeachingPlanItemAsync(TeachingPlanItemModel item)
    {
        if (item is not null)
        {
            var dto = _mapper.Map<NewTeachingPlanItemDto>(item);
            var serializedDtos = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PutAsync("/period/addplanitem", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException("The item should not be null.");
    }

    public async Task<bool> ExistsTeachingPlanItemAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.GetAsync($"/period/existsplanitem?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var contentText = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<bool>(contentText);
                return result;
            }
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }
        throw new ArgumentNullException("The id must be a valid Guid.");
    }

    public Task<int> GetTeachingPlanItemsCountAsync()
        => throw new NotImplementedException();

    public async Task<int> GetTeachingPlanItemsCountAsync(Guid periodId)
    {
        if (periodId == Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.GetAsync($"/period/planitemscount?periodId={periodId}");
            if (response.IsSuccessStatusCode)
            {
                var total = int.Parse(await response.Content.ReadAsStringAsync());
                return total;
            }
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }
        throw new ArgumentNullException("The id must be a valid Guid.");
    }

    public Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(int from = 0, int to = 0) => throw new NotImplementedException();

    public async Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsAsync(Guid periodId, int from = 0, int to = 0)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/period/planitems?periodId={periodId}&from={from}&to={to}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var periods = JsonConvert.DeserializeObject<IList<TeachingPlanItemSimpleDto>>(contentText);
            return periods?.Select(f => _mapper.Map<TeachingPlanItemModel>(f)).ToList() ?? new List<TeachingPlanItemModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<TeachingPlanItemModel> GetTeachingPlanItemAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.GetAsync($"/period/planitem?id={id}");
            if (response.IsSuccessStatusCode)
            {
                var teachingPlanItem = JsonConvert.DeserializeObject<TeachingPlanItemDto>(await response.Content.ReadAsStringAsync());
                return _mapper.Map<TeachingPlanItemModel>(teachingPlanItem);
            }
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }
        throw new ArgumentNullException("The id must be a valid Guid.");
    }

    public async Task<bool> UpdateTeachingPlanItemAsync(TeachingPlanItemModel period)
    {
        if (period is not null)
        {
            var dto = _mapper.Map<EditTeachingPlanItemDto>(period);
            var serializedDto = JsonConvert.SerializeObject(dto);
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.PostAsync("/period/updateplanitem", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(period));
    }

    public async Task<bool> DeleteTeachingPlanItemAsync(Guid id)
    {
        if (id != Guid.Empty)
        {
            var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
            var response = await client.DeleteAsync($"/period/deleteplanitem?id={id}");
            return response.IsSuccessStatusCode;
        }
        throw new ArgumentNullException(nameof(id));
    }

    public async Task<IList<TeachingPlanItemModel>> GetTeachingPlanItemsOfDepartmentOnPeriodAsync(Guid departmentId, Guid periodId, Guid? courseId = null)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/department/planningitems?id={departmentId}&periodId={periodId}{(courseId is not null ? $"&courseId={courseId}" : "")}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var periods = JsonConvert.DeserializeObject<IList<TeachingPlanItemDto>>(contentText);
            return periods?.Select(f => _mapper.Map<TeachingPlanItemModel>(f)).ToList() ?? new List<TeachingPlanItemModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    #endregion

    #region Statistics

    public async Task<IList<StatisticItemModel>> GetGlobalStatisticsAsync()
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/statistics/globalstatistics");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var statisticsItems = JsonConvert.DeserializeObject<IList<StatisticItemDto>>(contentText);
            return statisticsItems?.Select(f => _mapper.Map<StatisticItemModel>(f)).ToList() ?? new List<StatisticItemModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    public async Task<IList<StatisticItemModel>> GetGlobalStatisticsForDepartmentAsync(Guid departmentId)
    {
        var client = await _apiCallerFactory.CreateApiCallerHttpClientAsync();
        var response = await client.GetAsync($"/statistics/departmentstatistics?departmentId={departmentId}");
        if (response.IsSuccessStatusCode)
        {
            var contentText = await response.Content.ReadAsStringAsync();
            var statisticsItems = JsonConvert.DeserializeObject<IList<StatisticItemDto>>(contentText);
            return statisticsItems?.Select(f => _mapper.Map<StatisticItemModel>(f)).ToList() ?? new List<StatisticItemModel>();
        }
        throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
    }

    #endregion
}