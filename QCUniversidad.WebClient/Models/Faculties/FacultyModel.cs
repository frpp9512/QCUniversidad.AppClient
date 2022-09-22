using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Faculties
{
    public class FacultyModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Nombre", Prompt = "Nombre de la facultad", Description = "El nombre de la facultad")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Sede", Prompt = "Sede universitaria", Description = "El nombre de la sede universitaria a la cual pertenece la facultad.")]
        public string Campus { get; set; }

        public int DepartmentCount { get; set; } = 0;
        public int CareersCount { get; set; } = 0;
    }
}
