using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.Accounts;

public class ChangePasswordViewModel
{
    public required string Fullname { get; set; }

    [Display(Name = "Correo electrónico", Prompt = "Correo electrónico (Ej. alguien@dominio.cu)", Description = "La dirección de correo electrónico que servirá además para iniciar sesión.")]
    [EmailAddress(ErrorMessage = "Debe de escribir la dirección de correo electrónico correctamente.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Debe de escribir correctamente la contraseña.")]
    [Display(Name = "Contraseña", Prompt = "Contraseña", Description = "La contraseña usada para el inicio de sesión.")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Required(ErrorMessage = "Debe de confirmar correctamente la contraseña.")]
    [Display(Name = "Confirmar contraseña", Prompt = "Confirmar contraseña", Description = "Confirme la contraseña a usar para el inicio de sesión.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Debe de confirmar correctamente la contraseña.")]
    public required string ConfirmPassword { get; set; }

    public required string Id { get; set; }

    public bool AllowChangeEmail { get; set; } = false;
}
