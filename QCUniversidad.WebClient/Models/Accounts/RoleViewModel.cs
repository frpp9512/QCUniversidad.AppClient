using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.Accounts;

public class RoleViewModel
{
    public Guid Id { get; set; }

    [Display(Name = "Nombre", Prompt = "Nombre del rol", Description = "Nombre único que identifica al rol.")]
    public required string Name { get; set; }

    [Display(Name = "Descripción", Prompt = "Descripción", Description = "Descripción de las acciones que puede ejecutar quien desepmeñe el rol.")]
    public required string Description { get; set; }
}
