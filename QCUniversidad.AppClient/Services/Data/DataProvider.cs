using AutoMapper;
using Newtonsoft.Json;
using QCUniversidad.Api.Shared.Dtos;
using QCUniversidad.AppClient.AutoMapperProfiles;
using QCUniversidad.AppClient.Models;
using QCUniversidad.AppClient.PlataformServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.Services.Data
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
            var client = _apiCallerFactory.CreateApiCallerHttpClient();
            var response = await client.GetAsync($"/faculty/list?from={from}&to={to}");
            if (response.IsSuccessStatusCode)
            {
                var contentText = await response.Content.ReadAsStringAsync();
                var faculties = JsonConvert.DeserializeObject<IList<FacultyDto>>(contentText);
                return faculties.Select(f => _mapper.Map<FacultyModel>(f)).ToList();
            }
            throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");
        }

        public async Task<bool> CreateFacultyAsync(FacultyModel facultyModel)
        {
            if (facultyModel is not null)
            {
                var dto = _mapper.Map<FacultyDto>(facultyModel);
                var serializedDtos = JsonConvert.SerializeObject(dto);
                var client = _apiCallerFactory.CreateApiCallerHttpClient();
                var response = await client.PutAsync("/faculty", new StringContent(serializedDtos, Encoding.UTF8, "application/json"));
                return response.IsSuccessStatusCode;
            }
            throw new ArgumentNullException(nameof(facultyModel));
        }

        public async Task<FacultyModel> GetFacultyAsync(Guid id)
        {
            var client = _apiCallerFactory.CreateApiCallerHttpClient();
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
                var client = _apiCallerFactory.CreateApiCallerHttpClient();
                var response = await client.PostAsync("/faculty/update", new StringContent(serializedDto, Encoding.UTF8, "application/json"));
                return response.IsSuccessStatusCode;
            }
            throw new ArgumentNullException(nameof(facultyModel));
        }

        public async Task<bool> DeleteFacultyAsync(Guid id)
        {
            if (id != Guid.Empty)
            {
                var client = _apiCallerFactory.CreateApiCallerHttpClient();
                var response = await client.DeleteAsync($"/faculty?id={id}");
                return response.IsSuccessStatusCode;
            }
            throw new ArgumentNullException(nameof(id));
        }

        #endregion

        #region Deparments

        public async Task<IList<DepartmentModel>> GetDeparmentsAsync(Guid facultyId)
        {
            if (facultyId != Guid.Empty)
            {
                var client = _apiCallerFactory.CreateApiCallerHttpClient();
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

        public async Task<DepartmentModel> GetDeparmentAsync(Guid departmentId)
        {
            if (departmentId != Guid.Empty)
            {
                var client = _apiCallerFactory.CreateApiCallerHttpClient();
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
                var dto = _mapper.Map<DepartmentDto>(newDepartment);
                var serializedData = JsonConvert.SerializeObject(dto);
                var client = _apiCallerFactory.CreateApiCallerHttpClient();
                var response = await client.PutAsync("/department", new StringContent(serializedData, Encoding.UTF8, "application/json"));
                return response.IsSuccessStatusCode;
            }
            throw new ArgumentNullException(nameof(newDepartment));
        }

        public async Task<bool> UpdateDepartmentAsync(DepartmentModel department)
        {
            if (department is not null)
            {
                var dto = _mapper.Map<DepartmentDto>(department);
                var serializedData = JsonConvert.SerializeObject(dto);
                var client = _apiCallerFactory.CreateApiCallerHttpClient();
                var response = await client.PostAsync("/department/update", new StringContent(serializedData, Encoding.UTF8, "application/json"));
                return response.IsSuccessStatusCode;
            }
            throw new ArgumentNullException(nameof(department));
        }

        public async Task<bool> DeleteDepartmentAsync(Guid departmentId)
        {
            if (departmentId != Guid.Empty)
            {
                var client = _apiCallerFactory.CreateApiCallerHttpClient();
                var response = await client.DeleteAsync($"/department?id={departmentId}");
                return response.IsSuccessStatusCode;
            }
            throw new ArgumentNullException(nameof(departmentId));
        }

        #endregion
    }
}