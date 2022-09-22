using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QCUniversidad.WebClient.Models.Departments
{
    public class EditDepartmentModel
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "Nombre", Prompt = "Nombre del departamento", Description = "El nombre del departamento")]
        public string Name { get; set; }
        [Display(Name = "Descripción", Prompt = "Descripción del departamento", Description = "La descripción del departamento.")]
        public string? Description { get; set; }
        public Guid FacultyId { get; set; }
        public string FacultyName { get; set; }
    }
}
