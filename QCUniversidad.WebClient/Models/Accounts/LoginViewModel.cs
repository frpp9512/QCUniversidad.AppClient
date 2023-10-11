﻿using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.Accounts;

public class LoginViewModel
{
    [Required(ErrorMessage = "Debe de escribir la dirección de correo electrónico para iniciar sesión.")]
    [Display(Name = "Correo electrónico", Prompt = "Correo electrónico (Ej. alguien@dominio.cu)", Description = "La dirección de correo electrónico con que se registró el usuario.")]
    [EmailAddress(ErrorMessage = "Debe de escribir la dirección de correo electrónico correctamente.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Debe de escribir correctamente la contraseña para iniciar sesión.")]
    [Display(Name = "Contraseña", Prompt = "Contraseña", Description = "La contraseña usada para el inicio de sesión.")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Display(Name = "Recordar sesión", Prompt = "Recordar sesión", Description = "Define si el inicio sesión va a ser persistente (no expirará).")]
    public bool RememberSession { get; set; }

    [DataType(DataType.Url)]
    [Display(AutoGenerateField = false)]
    public string? ReturnUrl { get; set; }
}
