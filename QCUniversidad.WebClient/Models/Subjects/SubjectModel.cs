using QCUniversidad.WebClient.Models.Disciplines;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QCUniversidad.WebClient.Models.Subjects;

public record SubjectModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Debe de definir el nombre de la asignatura.")]
    [Display(Name = "Nombre de la asignatura", Prompt = "Nombre de la asignatura", Description = "El nombre de la asignatura.")]
    public string Name { get; set; }

    [Display(Name = "Descripción", Prompt = "Descripción de la asignatura", Description = "La descripción de la asignatura.")]
    public string? Description { get; set; }

    public Guid DisciplineId { get; set; }

    public DisciplineModel? Discipline { get; set; }

    public SubjectImportAction? ImportAction { get; set; }
}