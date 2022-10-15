using QCUniversidad.WebClient.Models.Faculties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Careers
{
    public class CareerModel
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Nombre", Prompt = "Nombre de la carrera", Description = "El nombre de la carrera")]
        public string Name { get; set; }

        [Display(Name = "Descripción", Prompt = "Descripción de la carrera", Description = "La descripción de la carrera.")]
        public string? Description { get; set; }

        [Display(Name = "Postgrado", Prompt = "Es postgrado", Description = "Define si la carrera es un postgrado.")]
        public bool PostgraduateCourse { get; set; }

        public Guid FacultyId { get; set; }
        public FacultyModel? Faculty { get; set; }
    }
}
