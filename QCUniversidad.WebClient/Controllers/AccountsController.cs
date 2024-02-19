﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using QCUniversidad.WebClient.Data.Helpers;
using QCUniversidad.WebClient.Models.Accounts;
using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Faculties;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Services.Contracts;
using SmartB1t.Security.Extensions.AspNetCore;
using SmartB1t.Security.WebSecurity.Local.Interfaces;
using SmartB1t.Security.WebSecurity.Local.Models;
using SmartB1t.Web.Extensions;

namespace QCUniversidad.WebClient.Controllers;

public class AccountsController : Controller
{
    #region Private members

    private readonly IAccountSecurityRepository _repository;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IFacultiesDataProvider _facultiesDataProvider;
    private readonly IDepartmentsDataProvider _departmentsDataProvider;
    private readonly IMapper _mapper;
    private readonly string _profileTmpFolder;
    private readonly string _profileDefaultPath;
    private readonly string _profilePictureFileName;

    #endregion

    #region Constructor

    public AccountsController(IAccountSecurityRepository repository,
                              IWebHostEnvironment hostEnvironment,
                              IFacultiesDataProvider facultiesDataProvider,
                              IDepartmentsDataProvider departmentsDataProvider,
                              IMapper mapper)
    {
        _repository = repository;
        _hostEnvironment = hostEnvironment;
        _facultiesDataProvider = facultiesDataProvider;
        _departmentsDataProvider = departmentsDataProvider;
        _mapper = mapper;
        _profileTmpFolder = Path.Combine(_hostEnvironment.WebRootPath, "img", "tmp");
        _profileDefaultPath = Path.Combine(_hostEnvironment.WebRootPath, "img", "layout", "default-profile-pic.jpg");
        _profilePictureFileName = Path.Combine(_hostEnvironment.WebRootPath, "img", "loggedUser", "user.jpg");
    }

    #endregion

    #region Auth

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string returnUrl = "/")
    {
        if (await _repository.AnyUserAsync())
        {
            return View(model: new LoginViewModel { Email = "", Password = "", RememberSession = false, ReturnUrl = returnUrl });
        }

        List<Role> roles =
        [
            new Role { Name = "Administrador", Description = "Administrador del sistema", Active = true },
            new Role { Name = "Planificador", Description = "Planificador de carga docente", Active = true },
            new Role { Name = "Jefe de departamento", Description = "Distribuidor de carga docente", Active = true }
        ];
        foreach (Role role in roles)
        {
            await _repository.CreateRoleAsync(role);
        }

        User user = new() { Email = "admin@fis.cu", Fullname = "Default administrator", Active = true };
        await _repository.CreateUserAsync(user, "admin.123");
        Role adminRole = roles.First();
        await _repository.AssignRoleToUserAsync(user, adminRole);
        return View(model: new LoginViewModel { Email = "", Password = "", ReturnUrl = returnUrl });
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginAsync(LoginViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        User user = await _repository.GetUserAsync(viewModel.Email, true);

        if (user == null)
        {
            ModelState.AddModelError("Usuario desconocido", "El usuario no existe.");
            return View(viewModel);
        }

        if (!user.Active)
        {
            ModelState.AddModelError("Usuario desactivado", $"El usuario {user.Email} se encuentra desactivado. Contacte al administrador de la web para más información.");
            return View(viewModel);
        }

        if (!await _repository.AuthenticateUser(user, viewModel.Password))
        {
            ModelState.AddModelError("Contraseña incorrecta", "La contraseña es incorrecta.");
            return View(viewModel);
        }

        await user.SignInAsync(HttpContext, Constants.AUTH_SCHEME, viewModel.RememberSession);
        RemoveProfilePictureFile();
        if (user.ProfilePicture is not null)
        {
            using FileStream fileStream = new(_profilePictureFileName, FileMode.Create);
            await fileStream.WriteAsync(user.ProfilePicture);
        }

        return !string.IsNullOrEmpty(viewModel.ReturnUrl) ? Redirect(viewModel.ReturnUrl) : Redirect("/");
    }

    [Authorize]
    public async Task<IActionResult> LogoutAsync(string returnUrl = "/")
    {
        await HttpContext.SignOutAsync();
        RemoveProfilePictureFile();
        return RedirectPermanent(returnUrl);
    }

    private void RemoveProfilePictureFile()
    {
        if (System.IO.File.Exists(_profilePictureFileName))
        {
            System.IO.File.Delete(_profilePictureFileName);
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    #endregion

    #region Management

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> IndexAsync(int page = 1, int usersPerPage = 5, bool includeInactive = true)
    {
        RemoveTempDirectory();

        Func<(int, int)> calculateIndexes = new(() => ((page - 1) * usersPerPage, ((page - 1) * usersPerPage) + usersPerPage));
        int usersCount = await _repository.GetUsersCount(includeInactive);

        (int, int) indexes = calculateIndexes();
        if (usersCount < indexes.Item1)
        {
            page = 1;
            indexes = calculateIndexes();
        }

        IEnumerable<User> users = await _repository.GetUsersAsync(indexes.Item1, indexes.Item2, true);
        List<UserViewModel> vmUsers = users.Select(_mapper.Map<UserViewModel>).ToList();
        foreach (UserViewModel? user in vmUsers)
        {
            if (user.ProfilePicture is not null)
            {
                using FileStream fileStream = new(GetTempPhotoPath(user.Id.ToString()), FileMode.Create);
                await fileStream.WriteAsync(user.ProfilePicture);
            }

            if (user.ExtraClaims?.Any(c => c.Type == "DepartmentId") is true && Guid.TryParse(user.ExtraClaims.First(c => c.Type == "DepartmentId").Value, out Guid departmentId) && await _departmentsDataProvider.ExistsDepartmentAsync(departmentId))
            {
                user.DepartmentModel = await _departmentsDataProvider.GetDepartmentAsync(new Guid(user.ExtraClaims.First(c => c.Type == "DepartmentId").Value));
            }

            if (user.ExtraClaims?.Any(c => c.Type == "FacultyId") is true && Guid.TryParse(user.ExtraClaims.First(c => c.Type == "FacultyId").Value, out Guid facultyId) && await _facultiesDataProvider.ExistFacultyAsync(facultyId))
            {
                user.FacultyModel = await _facultiesDataProvider.GetFacultyAsync(facultyId);
            }
        }

        int totalPages = (int)Math.Ceiling((decimal)usersCount / usersPerPage);
        AccountManagamentViewModel vm = new()
        {
            Users = vmUsers,
            PagesCount = totalPages == 0 ? 1 : totalPages,
            CurrentPage = page,
            UsersPerPage = usersPerPage,
            UsersCount = usersCount
        };
        return View(vm);
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        RemoveTempDirectory();
        CreateUserViewModel vm = new()
        {
            RoleList = await GetRoleViewModelsAsync()
        };
        return View(vm);
    }

    private async Task<IEnumerable<RoleViewModel>> GetRoleViewModelsAsync()
    {
        IEnumerable<Role> roles = await _repository.GetRolesAsync();
        IEnumerable<RoleViewModel> vmRoles = GetRoleViewModels(roles);
        return vmRoles;
    }

    private async Task<IList<DepartmentModel>> GetDeparmentsModels()
    {
        IList<DepartmentModel> departments = await _departmentsDataProvider.GetDepartmentsAsync();
        return departments;
    }

    private async Task<IList<FacultyModel>> GetFacultiesModels()
    {
        IList<FacultyModel> faculties = await _facultiesDataProvider.GetFacultiesAsync();
        return faculties;
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> GetDepartmentSelect()
    {
        IList<DepartmentModel> departments = await GetDeparmentsModels();
        return PartialView("_DepartmentSelect", departments);
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> GetFacultySelect()
    {
        IList<FacultyModel> faculties = await GetFacultiesModels();
        return PartialView("_FacultySelect", faculties);
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(CreateUserViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            viewModel.RoleList = await GetRoleViewModelsAsync();
            viewModel.Departments = await GetDeparmentsModels();
            return View(viewModel);
        }

        User user = viewModel.GetModel();

        if (!string.IsNullOrEmpty(viewModel.ProfilePictureId) && ExistsTempPhoto(viewModel.ProfilePictureId))
        {
            using FileStream stream = new(GetTempPhotoPath(viewModel.ProfilePictureId), FileMode.Open);
            using MemoryStream memStream = new();
            stream.CopyTo(memStream);
            user.ProfilePicture = memStream.ToArray();
        }

        if (!(viewModel.RolesSelected?.Length > 0))
        {
            ModelState.AddModelError("NoRolesSelected", "No se ha seleccionado ningún rol a desempeñar por el usuario.");
            viewModel.RoleList = await GetRoleViewModelsAsync();
            viewModel.Departments = await GetDeparmentsModels();
            return View(viewModel);
        }

        List<UserRole> userRoles = [];
        foreach (string selectedRole in viewModel.RolesSelected)
        {
            Role? role = await _repository.GetRoleAsync(new Guid(selectedRole));
            if (role is not null)
            {
                userRoles.Add(new UserRole
                {
                    Role = role
                });
            }
        }

        if (userRoles.Any(r => r.Role.Name == "Jefe de departamento") && (viewModel.SelectedDepartment is null || viewModel.SelectedDepartment == Guid.Empty))
        {
            ModelState.AddModelError("Jefe de departamento sin departamento", "Un usuario Jefe de departamento debe de gestionar un departamento.");
            viewModel.RoleList = await GetRoleViewModelsAsync();
            viewModel.Departments = await GetDeparmentsModels();
            return View(viewModel);
        }

        if (userRoles.Any(r => r.Role.Name == "Planificador") && (viewModel.SelectedFaculty is null || viewModel.SelectedFaculty == Guid.Empty))
        {
            ModelState.AddModelError("Planficador sin facultad.", "Un usuario Planificador debe de gestionar una facultad.");
            viewModel.RoleList = await GetRoleViewModelsAsync();
            viewModel.Departments = await GetDeparmentsModels();
            return View(viewModel);
        }

        user.ExtraClaims = new List<ExtraClaim>
        {
            new() {
                Type = "FacultyId",
                Value = viewModel.SelectedFaculty.ToString()
            }
        };

        user.Roles = userRoles;
        await _repository.CreateUserAsync(user, viewModel.Password);
        TempData.SetModelCreated<User, Guid>(user.Id);
        return RedirectToActionPermanent("Index");
    }

    private bool ExistsTempPhoto(string fileId)
    {
        return System.IO.File.Exists(GetTempPhotoPath(fileId));
    }

    private string GetTempPhotoPath(string fileId)
    {
        return Path.Combine(_profileTmpFolder, $"{fileId}.jpg");
    }

    private static IEnumerable<RoleViewModel> GetRoleViewModels(IEnumerable<Role> roles)
    {
        return roles.Select(r => new RoleViewModel
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description
        });
    }

    [RequestFormLimits(MultipartBodyLengthLimit = 5242880)]
    [RequestSizeLimit(5242880)]
    public async Task<IActionResult> UploadTempUserPhoto(IFormFile profilephoto)
    {
        RemoveTempDirectory();
        if (profilephoto is not null)
        {
            string fileId = Guid.NewGuid().ToString();
            string fileName = Path.Combine(_profileTmpFolder, $"{fileId}.jpg");
            using FileStream file = new(fileName, FileMode.Create);
            await profilephoto.CopyToAsync(file);
            return System.IO.File.Exists(fileName)
                ? Ok(new { url = $"{Url.Action("ProfileTempPhoto", "Accounts")}?fileId={fileId}", fileId })
                : BadRequest("Error creando fichero en el servidor.");
        }

        return BadRequest(new { errorMessage = "Error no esperado." });
    }

    private void RemoveTempDirectory()
    {
        if (!Directory.Exists(_profileTmpFolder))
        {
            _ = Directory.CreateDirectory(_profileTmpFolder);
            return;
        }

        IEnumerable<string> files = Directory.EnumerateFiles(_profileTmpFolder);
        foreach (string file in files)
        {
            System.IO.File.Delete(file);
        }
    }

    public FileStreamResult ProfileTempPhoto(string fileId)
    {
        string fileName = Path.Combine(_profileTmpFolder, $"{fileId}.jpg");
        byte[] pictureBytes = System.IO.File.ReadAllBytes(System.IO.File.Exists(fileName) ? fileName : _profileDefaultPath);
        MemoryStream ms = new(pictureBytes);
        return new FileStreamResult(ms, new MediaTypeHeaderValue("image/jpg"))
        {
            FileDownloadName = "Profile.jpg"
        };
    }

    [AllowAnonymous]
    public async Task<IActionResult> ProfilePictureAsync(string id)
    {
        byte[] pictureBytes;
        if (id is null)
        {
            if (User.Identity.IsAuthenticated)
            {
                pictureBytes = System.IO.File.Exists(_profilePictureFileName)
                    ? System.IO.File.ReadAllBytes(_profilePictureFileName)
                    : System.IO.File.ReadAllBytes(_profileDefaultPath);
            }
            else
            {
                return BadRequest("Debe de específicar un id de usuario o estar autenticado.");
            }
        }
        else
        {
            User? user = await _repository.GetUserAsync(new Guid(id));
            if (user is null)
            {
                return BadRequest("No existe el usuario con el id específicado.");
            }

            pictureBytes = user.ProfilePicture;
            pictureBytes ??= System.IO.File.ReadAllBytes(_profileDefaultPath);
        }

        return new FileStreamResult(new MemoryStream(pictureBytes), new MediaTypeHeaderValue("image/jpeg"))
        {
            FileDownloadName = "Profile.jpg"
        };
    }

    [Authorize("Admin")]
    [HttpGet]
    public async Task<IActionResult> EditAsync(string id)
    {
        RemoveTempDirectory();
        User user = await _repository.GetUserAsync(new Guid(id), true);
        EditUserViewModel vm = user.GetEditViewModel();
        vm.RoleList = await GetRoleViewModelsAsync();
        if (user.ExtraClaims?.Any(c => c.Type == "DepartmentId") == true)
        {
            vm.SelectedDepartment = new Guid(user.ExtraClaims.First(c => c.Type == "DepartmentId").Value);
        }

        vm.Departments = await GetDeparmentsModels();
        if (user.ProfilePicture is not null)
        {
            using FileStream stream = new(GetTempPhotoPath(user.Id.ToString()), FileMode.Create);
            await stream.WriteAsync(user.ProfilePicture);
        }

        return View(vm);
    }

    [Authorize("Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(EditUserViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            User user = await _repository.GetUserAsync(new Guid(viewModel.Id), true);

            if (viewModel.ProfilePictureId != user.Id.ToString()
                && !string.IsNullOrEmpty(viewModel.ProfilePictureId)
                && ExistsTempPhoto(viewModel.ProfilePictureId))
            {
                using FileStream stream = new(GetTempPhotoPath(viewModel.ProfilePictureId), FileMode.Open);
                using MemoryStream memStream = new();
                await stream.CopyToAsync(memStream);
                user.ProfilePicture = memStream.ToArray();
            }

            user.Fullname = viewModel.Fullname;
            user.Department = viewModel.Department;
            user.Position = viewModel.Position;

            if (viewModel.RolesSelected.Length > 0)
            {
                IEnumerable<Role> newRoles = viewModel.RolesSelected.Select(r => _repository.GetRoleAsync(r).GetAwaiter().GetResult());
                if (newRoles.Any(r => r.Name == "Jefe de departamento") && (viewModel.SelectedDepartment is null || viewModel.SelectedDepartment == Guid.Empty))
                {
                    ModelState.AddModelError("Jefe de departamento sin departamento", "Un usuario Jefe de departamento debe de gestionar un departamento.");
                }
                else
                {
                    List<Role> rolesToRemove = [];
                    foreach (UserRole? userRole in user.Roles)
                    {
                        string? roleId = viewModel.RolesSelected.FirstOrDefault(r => userRole.Role.Id.ToString() == r);
                        if (string.IsNullOrEmpty(roleId))
                        {
                            rolesToRemove.Add(userRole.Role);
                        }
                    }

                    rolesToRemove.ForEach(r => _repository.RemoveRoleFromUserAsync(user, r).Wait());

                    foreach (Role? selectedRole in newRoles)
                    {
                        UserRole? userRole = user.Roles.FirstOrDefault(ur => ur.Role.Id == selectedRole.Id);
                        if (userRole is null)
                        {
                            _repository.AssignRoleToUserAsync(user, selectedRole).Wait();
                        }
                    }

                    if (user.ExtraClaims?.Any() == true)
                    {
                        foreach (ExtraClaim? ec in user.ExtraClaims)
                        {
                            await _repository.RemoveExtraClaimAsync(ec);
                        }
                    }

                    if (viewModel.SelectedDepartment is not null && viewModel.SelectedDepartment == Guid.Empty)
                    {
                        user.ExtraClaims = new List<ExtraClaim>
                        {
                            new() {
                                Type = "DepartmentId",
                                Value = viewModel.SelectedDepartment.ToString(),
                                UserId = user.Id
                            }
                        };
                    }

                    await _repository.UpdateUserAsync(user);

                    TempData.SetModelUpdated<User, Guid>(new Guid(viewModel.Id));
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError("NoRolesSelected", "No se ha seleccionado ningún rol a desempeñar por el usuario.");
            }
        }

        viewModel.RoleList = await GetRoleViewModelsAsync();
        return View();
    }

    [Authorize("Admin")]
    [HttpPost]
    public async Task<IActionResult> Activate(string id)
    {
        User? user = await _repository.GetUserAsync(new Guid(id));
        if (user is null)
        {
            return BadRequest("El usuario no existe.");
        }

        user.Active = true;
        await _repository.UpdateUserAsync(user);
        return Ok($"El usuario {user.Fullname} ha sido activado satisfactoriamente.");
    }

    [HttpPost]
    public async Task<IActionResult> Deactivate(string id)
    {
        User? user = await _repository.GetUserAsync(new Guid(id));
        if (user is null)
        {
            return BadRequest("El usuario no existe.");
        }

        user.Active = false;
        await _repository.UpdateUserAsync(user);
        return Ok($"El usuario {user.Fullname} ha sido desactivado satisfactoriamente.");
    }

    [Authorize("Admin")]
    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        User? user = await _repository.GetUserAsync(new Guid(id));
        if (user is null)
        {
            return BadRequest("El usuario no existe.");
        }

        user.PermanentDeactivation = true;
        user.Active = false;
        _repository.UpdateUserAsync(user).Wait();
        return Ok($"El usuario {user.Fullname} ha sido eliminado satisfactoriamente.");
    }

    #endregion
}
