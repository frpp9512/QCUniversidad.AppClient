using QCUniversidad.WebClient.Models.Departments;
using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.Accounts;

public class EditUserViewModel
{
    public string? ProfilePictureId { get; set; }

    public string? ProfilePictureFileName { get; set; }

    public string? Id { get; set; }

    public required string Email { get; set; }

    [Required(ErrorMessage = "Debe de escribir en nombre completo del usuario")]
    [Display(Name = "Nombre completo", Description = "El nombre completo del usuario.", Prompt = "Nombre completo")]
    public required string Fullname { get; set; }

    [Display(Name = "Cargo", Description = "El cargo que ocupa el usuario en la empresa.", Prompt = "Cargo que desempeña")]
    public required string Position { get; set; }

    [Display(Name = "Departamento", Description = "Deparamento en el que trabajo en usuario.", Prompt = "Departamento o área")]
    public string? Department { get; set; }

    public Guid? SelectedDepartment { get; set; }

    public IList<DepartmentModel>? Departments { get; set; }

    public string[]? RolesSelected { get; set; }

    public IEnumerable<RoleViewModel>? RoleList { get; set; }
}
