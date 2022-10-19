using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using QCUniversidad.WebClient.Data.Helpers;
using QCUniversidad.WebClient.Models.Accounts;
using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Shared;
using QCUniversidad.WebClient.Services.Data;
using SmartB1t.Security.Extensions.AspNetCore;
using SmartB1t.Security.WebSecurity.Local;
using SmartB1t.Security.WebSecurity.Local.Interfaces;
using SmartB1t.Security.WebSecurity.Local.Models;
using SmartB1t.Web.Extensions;

namespace QCUniversidad.WebClient.Controllers;

[Authorize(Roles = "Administrador")]
public class AccountsController : Controller
{
    #region Private members

    private readonly IAccountSecurityRepository _repository;
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly IDataProvider _dataProvider;
    private readonly IMapper _mapper;
    private readonly string _profileTmpFolder;
    private readonly string _profileDefaultPath;
    private readonly string _profilePictureFileName;

    #endregion

    #region Constructor

    public AccountsController(IAccountSecurityRepository repository, IWebHostEnvironment hostEnvironment, IDataProvider dataProvider, IMapper mapper)
    {
        _repository = repository;
        _hostEnvironment = hostEnvironment;
        _dataProvider = dataProvider;
        _mapper = mapper;
        _profileTmpFolder = Path.Combine(_hostEnvironment.WebRootPath, "img", "tmp");
        _profileDefaultPath = Path.Combine(_hostEnvironment.WebRootPath, "img", "layout", "default-profile-pic.jpg");
        _profilePictureFileName = Path.Combine(_hostEnvironment.WebRootPath, "img", "loggedUser", "user.jpg");
    }

    #endregion

    #region Auth

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = "/") => View(model: new LoginViewModel { ReturnUrl = returnUrl });

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginAsync(LoginViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _repository.GetUserAsync(viewModel.Email, true);
            if (user != null)
            {
                if (user.Active)
                {
                    if (await _repository.AuthenticateUser(user, viewModel.Password))
                    {
                        await user.SignInAsync(HttpContext, Constants.AUTH_SCHEME, viewModel.RememberSession);
                        using FileStream fileStream = new(_profilePictureFileName, FileMode.Create);
                        await fileStream.WriteAsync(user.ProfilePicture);
                        return !string.IsNullOrEmpty(viewModel.ReturnUrl) ? Redirect(viewModel.ReturnUrl) : Redirect("/");
                    }
                    else
                    {
                        ModelState.AddModelError("Contraseña incorrecta", "La contraseña es incorrecta.");
                    }
                }
                else
                {
                    ModelState.AddModelError("Usuario desactivado", $"El usuario {user.Email} se encuentra desactivado. Contacte al administrador de la web para más información.");
                }
            }
            else
            {
                ModelState.AddModelError("Usuario desconocido", "El usuario no existe.");
            }
        }
        return View(viewModel);
    }

    [Authorize]
    public async Task<IActionResult> LogoutAsync(string returnUrl = "/")
    {
        await HttpContext.SignOutAsync();
        if (System.IO.File.Exists(_profilePictureFileName))
        {
            System.IO.File.Delete(_profilePictureFileName);
        }
        return RedirectPermanent(returnUrl);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [AllowAnonymous]
    public IActionResult AccessDenied() => View();

    #endregion

    #region Management

    [HttpGet]
    public async Task<IActionResult> IndexAsync(int page = 1, int usersPerPage = 5, bool includeInactive = true)
    {
        RemoveTempDirectory();

        Func<(int, int)> calculateIndexes = new(() => ((page - 1) * usersPerPage, ((page - 1) * usersPerPage) + usersPerPage));
        var usersCount = await _repository.GetUsersCount(includeInactive);

        var indexes = calculateIndexes();
        if (usersCount < indexes.Item1)
        {
            page = 1;
            indexes = calculateIndexes();
        }

        var users = await _repository.GetUsersAsync(indexes.Item1, indexes.Item2, true);
        var vmUsers = users.Select(u => _mapper.Map<UserViewModel>(u)).ToList();
        foreach (var user in vmUsers)
        {
            if (user.ProfilePicture is not null)
            {
                using FileStream fileStream = new(GetTempPhotoPath(user.Id.ToString()), FileMode.Create);
                await fileStream.WriteAsync(user.ProfilePicture);
            }
            if (user.ExtraClaims?.Any(c => c.Type == "DepartmentId") == true)
            {
                if (await _dataProvider.ExistsDepartmentAsync(new Guid(user.ExtraClaims.First(c => c.Type == "DepartmentId").Value)))
                {
                    user.DepartmentModel = await _dataProvider.GetDepartmentAsync(new Guid(user.ExtraClaims.First(c => c.Type == "DepartmentId").Value));
                }
            }
        }

        var totalPages = (int)Math.Ceiling((decimal)usersCount / usersPerPage);
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

    [HttpGet]
    public async Task<IActionResult> CreateAsync()
    {
        RemoveTempDirectory();
        CreateUserViewModel vm = new()
        {
            RoleList = await GetRoleViewModelsAsync(),
            Departments = await GetDeparmentsModels()
        };
        return View(vm);
    }

    private async Task<IEnumerable<RoleViewModel>> GetRoleViewModelsAsync()
    {
        var roles = await _repository.GetRolesAsync();
        var vmRoles = GetRoleViewModels(roles);
        return vmRoles;
    }

    private async Task<IList<DepartmentModel>> GetDeparmentsModels()
    {
        var departments = await _dataProvider.GetDepartmentsAsync();
        return departments;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAsync(CreateUserViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var user = viewModel.GetModel();

            if (!string.IsNullOrEmpty(viewModel.ProfilePictureId) && ExistsTempPhoto(viewModel.ProfilePictureId))
            {
                using FileStream stream = new(GetTempPhotoPath(viewModel.ProfilePictureId), FileMode.Open);
                using MemoryStream memStream = new();
                stream.CopyTo(memStream);
                user.ProfilePicture = memStream.ToArray();
            }

            if (viewModel.RolesSelected?.Length > 0)
            {
                List<UserRole> userRoles = new();
                foreach (var selectedRole in viewModel.RolesSelected)
                {
                    var role = await _repository.GetRoleAsync(new Guid(selectedRole));
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
                }
                else
                {
                    user.Roles = userRoles;
                    if (viewModel.SelectedDepartment is not null && viewModel.SelectedDepartment != Guid.Empty)
                    {
                        user.ExtraClaims = new List<ExtraClaim>
                        {
                            new ExtraClaim
                            {
                                Type = "DepartmentId",
                                Value = viewModel.SelectedDepartment.ToString()
                            }
                        };
                    }
                    await _repository.CreateUserAsync(user, viewModel.Password);
                    TempData.SetModelCreated<User, Guid>(user.Id);
                    return RedirectToActionPermanent("Index");
                }
            }
            else
            {
                ModelState.AddModelError("NoRolesSelected", "No se ha seleccionado ningún rol a desempeñar por el usuario.");
            }
        }
        viewModel.RoleList = await GetRoleViewModelsAsync();
        viewModel.Departments = await GetDeparmentsModels();
        return View(viewModel);
    }

    private bool ExistsTempPhoto(string fileId) => System.IO.File.Exists(GetTempPhotoPath(fileId));

    private string GetTempPhotoPath(string fileId) => Path.Combine(_profileTmpFolder, $"{fileId}.jpg");

    private static IEnumerable<RoleViewModel> GetRoleViewModels(IEnumerable<Role> roles) => roles.Select(r => new RoleViewModel
    {
        Id = r.Id,
        Name = r.Name,
        Description = r.Description
    });

    [RequestFormLimits(MultipartBodyLengthLimit = 5242880)]
    [RequestSizeLimit(5242880)]
    public async Task<IActionResult> UploadTempUserPhoto(IFormFile profilephoto)
    {
        RemoveTempDirectory();
        if (profilephoto is not null)
        {
            var fileId = Guid.NewGuid().ToString();
            var fileName = Path.Combine(_profileTmpFolder, $"{fileId}.jpg");
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
        var files = Directory.EnumerateFiles(_profileTmpFolder);
        foreach (var file in files)
        {
            System.IO.File.Delete(file);
        }
    }

    public FileStreamResult ProfileTempPhoto(string fileId)
    {
        var fileName = Path.Combine(_profileTmpFolder, $"{fileId}.jpg");
        var pictureBytes = System.IO.File.ReadAllBytes(System.IO.File.Exists(fileName) ? fileName : _profileDefaultPath);
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
                pictureBytes = System.IO.File.ReadAllBytes(_profilePictureFileName);
            }
            else
            {
                return BadRequest("Debe de específicar un id de usuario o estar autenticado.");
            }
        }
        else
        {
            var user = await _repository.GetUserAsync(new Guid(id));
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

    [HttpGet]
    public async Task<IActionResult> EditAsync(string id)
    {
        RemoveTempDirectory();
        var user = await _repository.GetUserAsync(new Guid(id), true);
        var vm = user.GetEditViewModel();
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAsync(EditUserViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var user = await _repository.GetUserAsync(new Guid(viewModel.Id), true);

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
                var newRoles = viewModel.RolesSelected.Select(r => _repository.GetRoleAsync(r).GetAwaiter().GetResult());
                if (newRoles.Any(r => r.Name == "Jefe de departamento") && (viewModel.SelectedDepartment is null || viewModel.SelectedDepartment == Guid.Empty))
                {
                    ModelState.AddModelError("Jefe de departamento sin departamento", "Un usuario Jefe de departamento debe de gestionar un departamento.");
                }
                else
                {
                    List<Role> rolesToRemove = new();
                    foreach (var userRole in user.Roles)
                    {
                        var roleId = viewModel.RolesSelected.FirstOrDefault(r => userRole.Role.Id.ToString() == r);
                        if (string.IsNullOrEmpty(roleId))
                        {
                            rolesToRemove.Add(userRole.Role);
                        }
                    }

                    rolesToRemove.ForEach(r => _repository.RemoveRoleFromUserAsync(user, r).Wait());

                    foreach (var selectedRole in newRoles)
                    {
                        var userRole = user.Roles.FirstOrDefault(ur => ur.Role.Id == selectedRole.Id);
                        if (userRole is null)
                        {
                            _repository.AssignRoleToUserAsync(user, selectedRole).Wait();
                        }
                    }

                    if (user.ExtraClaims?.Any() == true)
                    {
                        foreach (var ec in user.ExtraClaims)
                        {
                            await _repository.RemoveExtraClaimAsync(ec);
                        }
                    }

                    if (viewModel.SelectedDepartment is not null && viewModel.SelectedDepartment == Guid.Empty)
                    {
                        user.ExtraClaims = new List<ExtraClaim>
                        {
                            new ExtraClaim
                            {
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

    [HttpPost]
    public async Task<IActionResult> Activate(string id)
    {
        var user = await _repository.GetUserAsync(new Guid(id));
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
        var user = await _repository.GetUserAsync(new Guid(id));
        if (user is null)
        {
            return BadRequest("El usuario no existe.");
        }
        user.Active = false;
        await _repository.UpdateUserAsync(user);
        return Ok($"El usuario {user.Fullname} ha sido desactivado satisfactoriamente.");
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _repository.GetUserAsync(new Guid(id));
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
