using AutoMapper;
using Newtonsoft.Json;
using QCUniversidad.Api.Shared.Dtos.Career;
using QCUniversidad.Api.Shared.Dtos.Department;
using QCUniversidad.Api.Shared.Dtos.Discipline;
using QCUniversidad.Api.Shared.Dtos.Faculty;
using QCUniversidad.Api.Shared.Dtos.Teacher;
using QCUniversidad.WebClient.Models.Careers;
using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Disciplines;
using QCUniversidad.WebClient.Models.Faculties;
using QCUniversidad.WebClient.Models.Teachers;
using QCUniversidad.WebClient.Services.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Services.Data
{
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

        #endregion
    }
}