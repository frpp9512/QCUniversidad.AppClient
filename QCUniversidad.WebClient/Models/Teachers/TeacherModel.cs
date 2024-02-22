using QCUniversidad.Api.Shared.Enums;
using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Disciplines;
using QCUniversidad.WebClient.Models.LoadItem;
using System.ComponentModel.DataAnnotations;

namespace QCUniversidad.WebClient.Models.Teachers;

public class TeacherModel
{
    public Guid Id { get; set; }

    public string? FirstName => Fullname?.Split(' ').First();

    [Required(ErrorMessage = "Debe de definir el nombre completo del profesor.")]
    [Display(Name = "Nombre completo", Prompt = "Nombre completo", Description = "El nombre completo del profesor.")]
    public string? Fullname { get; set; }

    [Required(ErrorMessage = "Debe de escribir el carné de identidad del profesor.")]
    [Display(Name = "Carné de identidad", Prompt = "Carné de identidad", Description = "El carné de identidad del profesor.")]
    [MaxLength(11, ErrorMessage = "El carné de identidad debe de ser de 11 caracteres."), MinLength(11, ErrorMessage = "El carné de identidad debe de ser de 11 caracteres.")]
    public string? PersonalId { get; set; }

    [Required(ErrorMessage = "Debe de especificar cargo del profesor.")]
    [Display(Name = "Cargo", Prompt = "Cargo", Description = "El cargo del profesor.")]
    public string? Position { get; set; }

    [Required(ErrorMessage = "Debe de seleccionar la categoría del profesor.")]
    [Display(Name = "Categoría", Prompt = "Categoría docente", Description = "Categoría docente del profesor.")]
    public TeacherCategory Category { get; set; }

    [Required(ErrorMessage = "Debe de seleccionar la categoría del profesor.")]
    [Display(Name = "Tipo de contrato", Prompt = "Tipo de contrato", Description = "Tipo de contrato por el cual esta vinculado el profesor al departamento.")]
    public TeacherContractType ContractType { get; set; }

    [Display(Name = "Fondo de tiempo específico (h/mes)", Prompt = "Fondo de tiempo de específico (h/mes)", Description = "Fondo de tiempo por el cuál será determinada la capacidad del profesor en cambio a la del período.")]
    public double SpecificTimeFund { get; set; }

    [Display(Name = "Correo electrónico", Prompt = "Correo electrónico", Description = "Dirección de correo electrónico del profesor.")]
    [DataType(DataType.EmailAddress, ErrorMessage = "Escriba correctamente la dirección electrónica.")]
    public string? Email { get; set; }

    [Display(Name = "Prestador de servicios", Prompt = "Prestador de servicios", Description = "Define si el profesor presta servicio a otros departamentos.")]
    public bool ServiceProvider { get; set; }

    public DateTime? Birthday { get; set; }

    public int Age { get; set; }

    public Guid DepartmentId { get; set; }

    public DepartmentModel? Department { get; set; }

    public Guid[]? SelectedDisciplines { get; set; }

    public IList<DisciplineModel>? Disciplines { get; set; }

    public TeacherLoadModel? Load { get; set; }

    public IList<LoadViewItemModel>? LoadViewItems { get; set; }

    public TeacherImportAction? ImportAction { get; set; }
}