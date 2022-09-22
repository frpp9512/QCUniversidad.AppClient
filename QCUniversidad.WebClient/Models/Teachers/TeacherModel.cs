using QCUniversidad.WebClient.Models.Departments;
using QCUniversidad.WebClient.Models.Disciplines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Teachers
{
    public class TeacherModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe de definir el nombre completo del profesor.")]
        [Display(Name = "Nombre completo", Prompt = "Nombre completo", Description = "El nombre completo del profesor.")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "Debe de escribir el carné de identidad del profesor.")]
        [Display(Name = "Carné de identidad", Prompt = "Carné de identidad", Description = "El carné de identidad del profesor.")]
        [MaxLength(11, ErrorMessage = "El carné de identidad debe de ser de 11 caracteres."), MinLength(11, ErrorMessage = "El carné de identidad debe de ser de 11 caracteres.")]
        public string PersonalId { get; set; }

        [Required(ErrorMessage = "Debe de cargo del profesor.")]
        [Display(Name = "Cargo", Prompt = "Cargo", Description = "El cargo del profesor.")]
        public string? Position { get; set; }

        public Guid DepartmentId { get; set; }

        public DepartmentModel? Department { get; set; }

        public Guid[]? SelectedDisciplines { get; set; }

        public IList<DisciplineModel>? Disciplines { get; set; }
    }
}